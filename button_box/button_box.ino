#include <avr/pgmspace.h>
#include <EEPROM.h>
#include <SPI.h>
#include "Arduino.h"
#include <avr/pgmspace.h>




const byte CMD_INIT = 200; //94d 5Eh
const byte CMD_INIT_DEBUG = 201; //95d 5Fh
const byte CMD_END = (byte)'~'; //126d 7Eh
const byte CMD_SET_DEBUG_MODE = 11;
const byte CMD_RESPONSE_SET_DEBUG_MODE = 12;
const byte CMD_SYN = (byte)'A'; //65d 41h
const byte CMD_7_SEGS = (byte)'B'; //66d 42h
const byte CMD_SYN_ACK = (byte)'a'; //97d 61h
const byte CMD_RGB_SHIFT = (byte)'C'; //67d 43h
const byte CMD_BUTTON_STATUS = (byte)'D'; //68d 44h
const byte CMD_DEBUG_BUTTON = 202; //CAh
const byte CMD_INVALID = (byte)0xef; //239d EFh
const byte INVALID_COMMAND_HEADER = 0xEF;

// 0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31 32 33 34 35 36 37 38 39 40 41 41 43 44 45 46 47 48 49 50 51 52 53
const uint8_t pinNumbers[] = {0,1,2,3,4,5,6,7,0,1,2,3,4,5,6,7,1,0,3,2,1,0,0,1,2,3,4,5,6,7,0,1,2,3,4,5,6,7,7,2,1,0,7,6,5,4,3,2,1,0,3,2,1,0};

//analogic pins are mapped from 0 to 15 (comminication pins 14 and 15 cannot be used)
#define portOfPin(P)\
  ((P)>=0&&(P)<8)?&PORTF:((P)>=8&&(P)<16)?&PORTK:((P)>=22&&(P)<30)?&PORTA:((P)>=30&&(P)<38)?&PORTC:((P)>=38)?&PORTD:((P)>=39&&(P)<42)?&PORTG:((P)>=42&&(P)<50)?&PORTL:((P)>=50&&(P)<54)?&PORTB:((P)>=16&&(P)<18)?&PORTH:&PORTD
#define ddrOfPin(P)\
  ((P)>=0&&(P)<8)?&DDRF:((P)>=8&&(P)<16)?&DDRK:((P)>=22&&(P)<30)?&DDRA:((P)>=30&&(P)<38)?&DDRC:((P)>=38)?&DDRD:((P)>=39&&(P)<42)?&DDRG:((P)>=42&&(P)<50)?&DDRL:((P)>=50&&(P)<54)?&DDRB:((P)>=16&&(P)<18)?&DDRH:&DDRD
#define pinOfPin(P)\
  ((P)>=0&&(P)<8)?&PINF:((P)>=8&&(P)<16)?&PINK:((P)>=22&&(P)<30)?&PINA:((P)>=30&&(P)<38)?&PINC:((P)>=38)?&PIND:((P)>=39&&(P)<42)?&PING:((P)>=42&&(P)<50)?&PINL:((P)>=50&&(P)<54)?&PINB:((P)>=16&&(P)<18)?&PINH:&PIND
#define pinIndex(P)((uint8_t)pinNumbers[P])
#define pinMask(P)((uint8_t)(1<<pinIndex(P)))

#define pinAsInput(P) *(ddrOfPin(P))&=~pinMask(P)
#define pinAsInputPullUp(P) *(ddrOfPin(P))&=~pinMask(P);digitalHigh(P)
#define pinAsOutput(P) *(ddrOfPin(P))|=pinMask(P)
#define digitalLow(P) *(portOfPin(P))&=~pinMask(P)
#define digitalHigh(P) *(portOfPin(P))|=pinMask(P)
#define isHigh(P)((*(pinOfPin(P))& pinMask(P))>0)
#define isLow(P)((*(pinOfPin(P))& pinMask(P))==0)
#define digitalState(P)((uint8_t)isHigh(P))







long lastTimeReceivedByte = 0;
long lastRotaryBounce = 0;
int debugMode = 0;
bool isInterruptDisabled[] = {false,false,false,false};
const int encoderPinA[] = {21,20,19,18};
const int encoderPinB[] = {25,24,27,26};



//id cannot start with Arduino
String id = "Button Box";
int TYPE = 1; //0 = Dash, 1 = Button Box
// ----------------------- ADDITIONAL BUTTONS ---------------------------------------------------------------
// https://www.arduino.cc/en/Tutorial/InputPullupSerial
// ----------------------------------------------------------------------------------------------------------
int ENABLED_BUTTONS_COUNT = 2; 
int BUTTON_PIN_1 = 22; 
int BUTTON_PIN_2 = 23; 
int BUTTON_PINS[] = { BUTTON_PIN_1, BUTTON_PIN_2 };

int ENABLED_MATRIX_COLUMNS = 7;
int ENABLED_MATRIX_ROWS = 7;
int columnPins[] = {41,40,39,38,37,36,35};
int rowPins[] = {53,52,51,50,49,48,47};


const int TOTAL_ROTARY = 4;
volatile int encoderPos[] = {15000, 15000, 15000, 15000};
volatile int orientation[] = {0, 0, 0, 0};
int lastRotaryPos[] = {15000, 15000, 15000, 15000};
byte lastRotaryState[] = {0, 0, 0, 0};
long lastRotaryStateChange = 0;

void rotEncoder1(){
  int pinOffset = 0;
  detachInterrupt(digitalPinToInterrupt(encoderPinA[pinOffset]));
  isInterruptDisabled[pinOffset] = true;  
  if(millis() - lastRotaryBounce > 10) {
    int pinB = digitalRead(encoderPinB[pinOffset]);
    if(pinB == HIGH) {    
      encoderPos[pinOffset]++;  
    } else{
      encoderPos[pinOffset]--;
    }
  }  
  lastRotaryBounce = millis();
}

void rotEncoder2(){
  int pinOffset = 1;
  detachInterrupt(digitalPinToInterrupt(encoderPinA[pinOffset]));
  isInterruptDisabled[pinOffset] = true;  
  if(millis() - lastRotaryBounce > 10) {
    int pinB = digitalRead(encoderPinB[pinOffset]);
    if(pinB == HIGH) {    
      encoderPos[pinOffset]++;  
    } else{
      encoderPos[pinOffset]--;
    }
  }  
  lastRotaryBounce = millis();
}

void rotEncoder3(){
  int pinOffset = 2;
  detachInterrupt(digitalPinToInterrupt(encoderPinA[pinOffset]));
  isInterruptDisabled[pinOffset] = true;  
  if(millis() - lastRotaryBounce > 10) {
    int pinB = digitalRead(encoderPinB[pinOffset]);
    if(pinB == HIGH) {    
      encoderPos[pinOffset]++;  
    } else{
      encoderPos[pinOffset]--;
    }
  }  
  lastRotaryBounce = millis();
}

void rotEncoder4(){
  int pinOffset = 3;
  detachInterrupt(digitalPinToInterrupt(encoderPinA[pinOffset]));
  isInterruptDisabled[pinOffset] = true;  
  if(millis() - lastRotaryBounce > 10) {
    int pinB = digitalRead(encoderPinB[pinOffset]);
    if(pinB == HIGH) {    
      encoderPos[pinOffset]++;  
    } else{
      encoderPos[pinOffset]--;
    }
  }  
  lastRotaryBounce = millis();
}

int readline(int readch, byte *buffer, int len)
{  
  static int pos = 0;
  int rpos;

  switch (readch) {
    case CMD_INIT_DEBUG:
    case CMD_INIT: // command init found
      pos = 0;
      buffer[pos++] = readch;
      buffer[pos] = 0;
      break;        
    case CMD_END: // End command
      rpos = pos;
      pos = 0;  // Reset position index ready for next time
      return rpos - 1; //last char is the crc
    default:
      if (pos < len-1) {
        buffer[pos++] = readch;
        buffer[pos] = 0;
      }
  }

  //flag to show that arduino is receiving data from App.    
  lastTimeReceivedByte = millis();
  
  // No end of command has been found, so return -1.
  return -1;
}


void initBuffer(byte *buffer, int size) {
  for(int x = 0; x < size; x++) {
    buffer[x] = 0;
  }

  return buffer;
}


int calculateCrc(int dataLength, byte *response) {
  int crc = 0;
  
  for(int i = 0; i < dataLength; i++) {    
    crc += response[i];
  }

  crc %= 256;

  return crc;
}

void sendDebugModeState(byte header, byte state) {
  byte response[5];
  int offset = 0;
  
  //handshaking
  response[offset++] = header;
  //set debug mode response
  response[offset++] = CMD_RESPONSE_SET_DEBUG_MODE;
  response[offset++] = state;
  response[offset++] = calculateCrc(offset - 1, response);
  response[offset++] = CMD_END;
  
  sendDataToSerial(offset, response);
}


void sendDataToSerial(int commandLength, byte *response) {
  Serial.write(response, commandLength);  
  Serial.flush();
//  for(int x=0; x<commandLength; x++)
//  Serial.print(response[x]);
//  Serial.println();
//  delay(500);
}

int appendArduinoId(int offset, byte *buffer) {
  for (int x = 0; x < id.length(); x++) {
    buffer[offset++] = id[x];   
  }

  return offset;
}

void sendHandshacking() {
  byte response[4];
  int offset = 0;
  
  //handshaking
  response[offset++] = CMD_INIT;
  response[offset++] = CMD_SYN;
  response[offset++] = TYPE;
  offset = appendArduinoId(offset, response);
  response[offset++] = calculateCrc(offset - 1, response);
  response[offset++] = CMD_END;
  
  sendDataToSerial(offset, response);
}


void processCommand(byte *buffer, int commandLength) {  
  //debug
  switch(buffer[1]) {
    case CMD_SET_DEBUG_MODE :
      debugMode = buffer[2];      
      sendDebugModeState(buffer[0], debugMode);       
      break;
    
    //syn ack
    case CMD_SYN_ACK : 
      sendHandshacking();  
      break;
  }  

  if(debugMode > 0) {
    sendDataToSerial(commandLength, buffer);  
  }
  
  buffer[0] = 0;
}


void processData() {  
  static byte buffer[100]; 
  initBuffer(buffer, 100);
  
  int commandLength = 0;

  long startReading = millis();
  while (Serial.available()) {
    commandLength = readline(Serial.read(), buffer, 100);       
    if (commandLength > 0) {  
      int crc = calculateCrc(commandLength, buffer); 

      if(crc == buffer[commandLength]) {    
        processCommand(buffer, commandLength);     
      } else {
        if(debugMode > 0) {          
          Serial.write(INVALID_COMMAND_HEADER);
          sendDataToSerial(commandLength, buffer);
          Serial.write(CMD_END);
        }
      }
    }    
    if(millis() - startReading > 10) {
      break;
    }
  }
}

void reAttachInterrupts() {
  if (isInterruptDisabled[0] == true && digitalRead(encoderPinA[0]) != LOW) {
    attachInterrupt(digitalPinToInterrupt(encoderPinA[0]), rotEncoder1, LOW);
    isInterruptDisabled[0] = false;
  } 

  if (isInterruptDisabled[1] == true && digitalRead(encoderPinA[1]) != LOW) {
    attachInterrupt(digitalPinToInterrupt(encoderPinA[1]), rotEncoder2, LOW);
    isInterruptDisabled[1] = false;
  }

  if (isInterruptDisabled[2] == true && digitalRead(encoderPinA[2]) != LOW) {
    attachInterrupt(digitalPinToInterrupt(encoderPinA[2]), rotEncoder3, LOW);
    isInterruptDisabled[2] = false;
  } 

  if (isInterruptDisabled[3] == true && digitalRead(encoderPinA[3]) != LOW) {
    attachInterrupt(digitalPinToInterrupt(encoderPinA[3]), rotEncoder4, LOW);
    isInterruptDisabled[3] = false;
  }
}


int sendRotaryState(int offset, byte *response) {  
  for(int i = 0; i < TOTAL_ROTARY; i++) {                  

    if(millis() - lastRotaryStateChange > 30) {  
      if(encoderPos[i] != lastRotaryPos[i]) {      
        if(lastRotaryPos[i] < encoderPos[i]) {  
          //turn left
          lastRotaryState[i] = 1;    
        } else {     
          //turn right          
          lastRotaryState[i] = 2;       
        }
        lastRotaryPos[i] = encoderPos[i];        

        lastRotaryStateChange = millis();
      } else {
        //not pressed
        lastRotaryState[i] = 0;     
      }  
    }

    switch(lastRotaryState[i]) {
      case 0:
        response[offset++] = 0;
        response[offset++] = 0; 
        break;
      case 1:
        response[offset++] = 0;
        response[offset++] = 1; 
        break;
      case 2:
        response[offset++] = 1;
        response[offset++] = 0; 
        break;
    }
  }
    
  return offset;
}


int sendButtonState(int offset, byte *response) {  
  for (int i = 0; i < ENABLED_BUTTONS_COUNT; i++) {      
    response[offset++] = digitalState(BUTTON_PINS[i]) == HIGH ? 0 : 1;
  }   
  
  return offset;
}

int sendMatrixState(int offset, byte *response) {  
  for (int i = 0; i < ENABLED_MATRIX_COLUMNS; i++) {
    digitalWrite(columnPins[i], LOW);
    for (int x = 0; x < ENABLED_MATRIX_ROWS; x++) {
      response[offset++] = (digitalRead(rowPins[x]) == LOW) ? 1 : 0;
    }
    digitalWrite(columnPins[i], HIGH);
  }   
  
  return offset;
}

int sendAxisState(int offset, byte *response) {
  for(int x=0; x < 4; x++) {
    response[offset++]=0;
  }

  return offset;
}

void sendButtonStatus(byte header) {
  byte response[100];
  initBuffer(response, 100);
  int offset = 0;
  
  //return buttons state      
  response[offset++] = header;
  response[offset++] = CMD_BUTTON_STATUS;
  //axis has to be the last bytes in the array
  offset = sendAxisState(offset, response);
  offset = sendButtonState(offset, response);
  //offset = sendAnalogState(offset, response);
  offset = sendRotaryState(offset, response);  
  //offset = sendAnalogState2(offset, response);
  offset = sendMatrixState(offset, response);
  response[offset++] = calculateCrc(offset - 1, response);
  response[offset++] = CMD_END;   
  
  sendDataToSerial(offset, response);  
}



void setup()
{
  Serial.begin(38400);
  // EXTERNAL BUTTONS INIT
  for (int btnIdx = 0; btnIdx < ENABLED_BUTTONS_COUNT; btnIdx++) {
    pinMode(BUTTON_PINS[btnIdx], INPUT_PULLUP);
  }

  // MATRIX
  for (int x = 0; x < ENABLED_MATRIX_COLUMNS; x++) {
    pinMode(columnPins[x], OUTPUT);           // set pin to input    
    //pinAsOutput(columnPins[x]);    
  }

  for (int x = 0; x < ENABLED_MATRIX_ROWS; x++) {
    pinMode(rowPins[x], INPUT_PULLUP);
    //pinAsInputPullUp(rowPins[x]);    
  }

  // Rotary
  for(int i = 0; i < TOTAL_ROTARY; i++) {
    pinMode(encoderPinA[i], INPUT_PULLUP);
    pinMode(encoderPinB[i], INPUT_PULLUP);
  }

  attachInterrupt(digitalPinToInterrupt(encoderPinA[0]), rotEncoder1, LOW);
  attachInterrupt(digitalPinToInterrupt(encoderPinA[1]), rotEncoder2, LOW);
  attachInterrupt(digitalPinToInterrupt(encoderPinA[2]), rotEncoder3, LOW);
  attachInterrupt(digitalPinToInterrupt(encoderPinA[3]), rotEncoder4, LOW);
}

void loop() {  
  
  processData();
  
  sendButtonStatus(CMD_INIT);

  reAttachInterrupts();
}

