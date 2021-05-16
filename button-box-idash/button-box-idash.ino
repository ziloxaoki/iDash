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


//analogic pins are mapped from 100 to 115 (comminication pins 14 and 15 cannot be used)
/*#define portOfPin(P)\
  (((P)>=100&&(P)<108)?&PORTF:((P)>=108&&(P)<116)?&PORTK:((P)>=22&&(P)<30)?&PORTA:((P)>=30&&(P)<38)?&PORTC:(((P)==38) || ((P)>=18&&(P)<22))?&PORTD:((P)>=39&&(P)<42)?&PORTG:((P)>=42&&(P)<50)?&PORTL:(((P)>=50&&(P)<54) || ((P)>=10&&(P)<14))?&PORTB:(((P)>=16&&(P)<18) || ((P)>=6&&(P)<10))?&PORTH:&PORTE)
  #define ddrOfPin(P)\
  (((P)>=100&&(P)<108)?&DDRF:((P)>=108&&(P)<116)?&DDRK:((P)>=22&&(P)<30)?&DDRA:((P)>=30&&(P)<38)?&DDRC:(((P)==38) || ((P)>=18&&(P)<22))?&DDRD:((P)>=39&&(P)<42)?&DDRG:((P)>=42&&(P)<50)?&DDRL:(((P)>=50&&(P)<54) || ((P)>=10&&(P)<14))?&DDRB:(((P)>=16&&(P)<18) || ((P)>=6&&(P)<10))?&DDRH:&DDRE)
  #define pinOfPin(P)\
  (((P)>=100&&(P)<108)?&PINF:((P)>=108&&(P)<116)?&PINK:((P)>=22&&(P)<30)?&PINA:((P)>=30&&(P)<38)?&PINC:(((P)==38) || ((P)>=18&&(P)<22))?&PIND:((P)>=39&&(P)<42)?&PING:((P)>=42&&(P)<50)?&PINL:(((P)>=50&&(P)<54) || ((P)>=10&&(P)<14))?&PINB:(((P)>=16&&(P)<18) || ((P)>=6&&(P)<10))?&PINH:&PINE)
  #define pinIndex(P)((uint8_t)(((P)>=100&&(P))<108?P-100:((P)>=22&&(P)<30)?P-22:((P)>=30&&(P)<38)?37-P:((P)==38)?7:((P)>=18&&(P)<22)?21-P:((P)>=39&&(P)<42)?41-P:((P)>=42&&(P)<50)?49-P:((P)>=50&&(P)<54)?53-P:((P)>=10&&(P)<14)?P-6:((P)>=16&&(P)<18)?17-P:((P)>=6&&(P)<10)?P-3:((P)==3)?5:((P)==2)?4:((P)==5)?3:((P)==1)?1:0))
  #define pinMask(P)((uint8_t)(1<<pinIndex(P)))

  #define pinAsInput(P) *(ddrOfPin(P))&=~pinMask(P)
  #define pinAsInputPullUp(P) *(ddrOfPin(P))&=~pinMask(P);digitalHigh(P)
  #define pinAsOutput(P) *(ddrOfPin(P))|=pinMask(P)
  #define digitalLow(P) *(portOfPin(P))&=~pinMask(P)
  #define digitalHigh(P) *(portOfPin(P))|=pinMask(P)
  #define isHigh(P)((*(pinOfPin(P))& pinMask(P))>0)
  #define isLow(P)((*(pinOfPin(P))& pinMask(P))==0)
  #define digitalRead(P)((uint8_t)isHigh(P))*/


long lastTimeReceivedByte = 0;
//long lastRotaryBounce = 0;
int debugMode = 0;
bool isInterruptDisabled[] = {false, false, false, false};
const int encoderPinA[] = {21, 20, 19, 18};
const int encoderPinB[] = {25, 24, 27, 26};


//id cannot start with Arduino
String name_ = "Button Box";
int id = 22;
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
int lastButtonState[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
int currentButtonState[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
long lastButtonDebounce[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
int rowPins[] = {35, 34, 33, 32, 31, 30, 29};
int columnPins[] = {46, 45, 44, 43, 42, 41, 40};


const int TOTAL_ROTARY = 4;
volatile int encoderPos[] = {15000, 15000, 15000, 15000};
volatile int orientation[] = {0, 0, 0, 0};
int lastRotaryPos[] = {15000, 15000, 15000, 15000};
byte lastRotaryState[] = {0, 0, 0, 0};
long lastRotaryStateChange = 0;
long lastHandshakeSent = 0;
long lastButtonStateSent = 0;

byte oldButtonState[100];

void rotEncoder1() {
  int pinOffset = 0;
  detachInterrupt(digitalPinToInterrupt(encoderPinA[pinOffset]));
  isInterruptDisabled[pinOffset] = true;
  //  if(millis() - lastRotaryBounce > 10) {
  int pinB = digitalRead(encoderPinB[pinOffset]);
  if (pinB == HIGH) {
    encoderPos[pinOffset]++;
  } else {
    encoderPos[pinOffset]--;
  }
  //  }
  //  lastRotaryBounce = millis();
}

void rotEncoder2() {
  int pinOffset = 1;
  detachInterrupt(digitalPinToInterrupt(encoderPinA[pinOffset]));
  isInterruptDisabled[pinOffset] = true;
  //  if(millis() - lastRotaryBounce > 10) {
  int pinB = digitalRead(encoderPinB[pinOffset]);
  if (pinB == HIGH) {
    encoderPos[pinOffset]++;
  } else {
    encoderPos[pinOffset]--;
  }
  //  }
  //  lastRotaryBounce = millis();
}

void rotEncoder3() {
  int pinOffset = 2;
  detachInterrupt(digitalPinToInterrupt(encoderPinA[pinOffset]));
  isInterruptDisabled[pinOffset] = true;
  //  if(millis() - lastRotaryBounce > 10) {
  int pinB = digitalRead(encoderPinB[pinOffset]);
  if (pinB == HIGH) {
    encoderPos[pinOffset]++;
  } else {
    encoderPos[pinOffset]--;
  }
  //  }
  //  lastRotaryBounce = millis();
}

void rotEncoder4() {
  int pinOffset = 3;
  detachInterrupt(digitalPinToInterrupt(encoderPinA[pinOffset]));
  isInterruptDisabled[pinOffset] = true;
  //  if(millis() - lastRotaryBounce > 10) {
  int pinB = digitalRead(encoderPinB[pinOffset]);
  if (pinB == HIGH) {
    encoderPos[pinOffset]++;
  } else {
    encoderPos[pinOffset]--;
  }
  //  }
  //  lastRotaryBounce = millis();
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
      return rpos - 1; //last byte is the crc so it is not part of command	  
    default:
		if (len == 100) {
			pos = 0;
			rpos = 0;
		}  
        buffer[pos++] = readch;
        buffer[pos] = 0;
  }

  //flag to show that arduino is receiving data from App.    
  lastTimeReceivedByte = millis();
  
  // No end of command has been found, so return -1.
  return -1;
}


void initBuffer(byte *buffer, int size) {
  for (int x = 0; x < size; x++) {
    buffer[x] = 0;
  }

  return buffer;
}

void arduinoReset() // Restarts program from beginning but does not reset the peripherals and registers
{
  asm volatile ("  jmp 0");    
}

int calculateCrc(int dataLength, byte *response) {
  int crc = 0;

  for (int i = 0; i < dataLength; i++) {
    crc += response[i];
  }
  //Serial.print("sum:");
  //Serial.println(crc);
  crc %= 256;
  //Serial.print("crc:");
  //Serial.println(crc);
  return crc;
}

void sendDebugModeState(byte header, byte state) {
  byte response[6];
  int offset = 0;

  //handshaking
  response[offset++] = header;
  response[offset++] = id;
  //set debug mode response
  response[offset++] = CMD_RESPONSE_SET_DEBUG_MODE;
  response[offset++] = state;
  response[offset++] = calculateCrc(offset, response);
  response[offset++] = CMD_END;

  sendDataToSerial(offset, response);  
}


void sendDataToSerial(int commandLength, byte *response) {
  Serial.write(response, commandLength);
  //DataReceived event will trigger only when a few characters are sent, including ‘0x0A’ (‘\n’)
  //Serial.write('\n');
  Serial.flush();
  /*for(int x=0; x<commandLength; x++) {
  Serial.print(response[x]);
  Serial.print('-');
  }
  Serial.print('=');
  Serial.print(commandLength);
  Serial.println();
  delay(500);*/
}

int appendArduinoId(int offset, byte *buffer) {
  for (int x = 0; x < name_.length(); x++) {
    buffer[offset++] = name_[x];
  }

  return offset;
}

void sendHandshacking() {
  if (millis() - lastHandshakeSent > 100) {
    byte response[100];
    int offset = 0;

    //handshaking
    response[offset++] = CMD_INIT;
    response[offset++] = id;
    response[offset++] = CMD_SYN;
    response[offset++] = TYPE;
    offset = appendArduinoId(offset, response);
    response[offset++] = calculateCrc(offset, response);
    response[offset++] = CMD_END;

    sendDataToSerial(offset, response);
    lastHandshakeSent = millis();
  }
}


void processCommand(byte *buffer, int commandLength) {
  //debug
  switch (buffer[2]) {
    case CMD_SET_DEBUG_MODE :
      debugMode = buffer[3];
      sendDebugModeState(buffer[0], debugMode);
      break;

    //syn ack
    case CMD_SYN_ACK :
      sendHandshacking();
      break;
  }

  if (debugMode > 0) {
    sendDataToSerial(commandLength, buffer);
  }

  buffer[0] = 0;
}

void processData() {  
  static byte buffer[100];
  static int byteRead = 0;
  
  int commandLength = 0;

  long startReading = millis();
  while (Serial.available() || byteRead == 100) {
    commandLength = readline(Serial.read(), buffer, byteRead++);       
	if (commandLength > 0) {
		int crc = calculateCrc(commandLength, buffer); 	  	

		if(crc == buffer[commandLength]) {
			processCommand(buffer, commandLength);
		} else {
			//sendInvalidDataBack(buffer, commandLength);
		}
		break;
	}
  }

  buffer[0] = 0;
  byteRead = 0;
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
  for (int i = 0; i < TOTAL_ROTARY; i++) {

    if (millis() - lastRotaryStateChange > 50) {
      if (encoderPos[i] != lastRotaryPos[i]) {
        if (lastRotaryPos[i] < encoderPos[i]) {
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

    switch (lastRotaryState[i]) {
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
    response[offset++] = digitalRead(BUTTON_PINS[i]) == HIGH ? 0 : 1;
  }

  return offset;
}


int sendMatrixState(int offset, byte *response) {
  int aux = 0;

  for (int i = 0; i < ENABLED_MATRIX_COLUMNS; i++) {

    digitalWrite(columnPins[i], LOW);
    for (int x = 0; x < ENABLED_MATRIX_ROWS; x++) {
      byte pinState = (digitalRead(rowPins[x]) == LOW) ? 1 : 0;
      //if(pinState == 1) {
      //  Serial.print(columnPins[i]);
      //  Serial.print(" - ");
      //  Serial.println(rowPins[x]);
      //  delay(1000);
      //}
      if (lastButtonState[aux] != pinState) {
        lastButtonDebounce[aux] = millis();
        lastButtonState[aux] = pinState;
      }

      if (lastButtonDebounce[aux] > 0 && millis() - lastButtonDebounce[aux]  > 15) {
        currentButtonState[aux] = lastButtonState[aux];
        lastButtonDebounce[aux] = 0;
      }

      response[offset++] = currentButtonState[aux++];

    }

    digitalWrite(columnPins[i], HIGH);
    //delay(10);
  }

  return offset;
}

int sendAxisState(int offset, byte *response) {
  for (int x = 0; x < 4; x++) {
    response[offset++] = 0;
  }

  return offset;
}

boolean stateHasChanged(int offset, byte* resp) {
  boolean result = false;
  for (int i = 3; i < offset - 5; i++) {
    if (resp[i] != oldButtonState[i]) {
      oldButtonState[i] = resp[i];
      result = true;
    }
  }

  return result;
}

void sendButtonStatus() {
  
  byte response[100];
  initBuffer(response, 100);
  int offset = 0;
  //Serial.println("pqp");
  response[offset++] = CMD_INIT;
  response[offset++] = id;
  response[offset++] = CMD_BUTTON_STATUS;
  //axis has to be the first 4 bytes in the array
  offset = sendAxisState(offset, response);
  offset = sendButtonState(offset, response);
  offset = sendRotaryState(offset, response);
  offset = sendMatrixState(offset, response);
  response[offset++] = calculateCrc(offset - 1, response);
  response[offset++] = CMD_END;

  /*if (response[45] == 1 && response[44] == 1) {
    arduinoReset();
    return;  
  }*/
  
  //send the command at least once a second to keep button state in app up-to-date
  if (stateHasChanged(offset - 1, response) || millis() - lastButtonStateSent > 10) {
    sendDataToSerial(offset, response);
    lastButtonStateSent = millis();
  }
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
    digitalWrite(columnPins[x], HIGH);        // initiate high
    //pinAsOutput(columnPins[x]);
  }

  for (int x = 0; x < ENABLED_MATRIX_ROWS; x++) {
    pinMode(rowPins[x], INPUT_PULLUP);
    //pinAsInputPullUp(rowPins[x]);
  }

  // Rotary
  for (int i = 0; i < TOTAL_ROTARY; i++) {
    pinMode(encoderPinA[i], INPUT_PULLUP);
    pinMode(encoderPinB[i], INPUT_PULLUP);
  }

  attachInterrupt(digitalPinToInterrupt(encoderPinA[0]), rotEncoder1, LOW);
  attachInterrupt(digitalPinToInterrupt(encoderPinA[1]), rotEncoder2, LOW);
  attachInterrupt(digitalPinToInterrupt(encoderPinA[2]), rotEncoder3, LOW);
  attachInterrupt(digitalPinToInterrupt(encoderPinA[3]), rotEncoder4, LOW);
}

void loop() {

  //processData();

  sendButtonStatus();
  sendHandshacking();
  reAttachInterrupts();
}
