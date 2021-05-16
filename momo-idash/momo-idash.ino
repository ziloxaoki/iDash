#define INCLUDE_LED
#define WHEEL


#include <avr/pgmspace.h>
#include <EEPROM.h>
#include <SPI.h>
#include "Arduino.h"
#include <avr/pgmspace.h>

#ifdef INCLUDE_LED
#include "LedControl.h"
#include <TM1637Display.h>
#endif

#include <Adafruit_NeoPixel.h>
#include <MemoryFree.h>


#define RIGHT 1
#define LEFT -1
#define portOfPin(P)\
  (((P)>=0&&(P)<8)?&PORTD:(((P)>7&&(P)<14)?&PORTB:&PORTC))
#define ddrOfPin(P)\
  (((P)>=0&&(P)<8)?&DDRD:(((P)>7&&(P)<14)?&DDRB:&DDRC))
#define pinOfPin(P)\
  (((P)>=0&&(P)<8)?&PIND:((P)>7&&(P)<14)?&PINB:&PINC)
#define pinIndex(P)((uint8_t)(P>13?P-14:P&7))
#define pinMask(P)((uint8_t)(1<<pinIndex(P)))

#define pinAsInput(P) *(ddrOfPin(P))&=~pinMask(P)
#define pinAsInputPullUp(P) *(ddrOfPin(P))&=~pinMask(P);digitalHigh(P)
#define pinAsOutput(P) *(ddrOfPin(P))|=pinMask(P)
#define digitalLow(P) *(portOfPin(P))&=~pinMask(P)
#define digitalHigh(P) *(portOfPin(P))|=pinMask(P)
#define isHigh(P)((*(pinOfPin(P))& pinMask(P))>0)
#define isLow(P)((*(pinOfPin(P))& pinMask(P))==0)
#define digitalState(P)((uint8_t)isHigh(P))

// -------------------------------------------------------------------------------------------------------
// TM1637 7 Segment modules -----------------------------------------------------------------------------
//
// -------------------------------------------------------------------------------------------------------

#ifdef WHEEL
//id cannot start with Arduino
String name_ = "Wheel";
int id = 33;
//String id = "Button Box"
int TYPE = 0; //0 = Dash, 1 = Button Box
#endif

#ifdef BUTTON_BOX
//id cannot start with Arduino
String id = "Button Box"
int TYPE = 1; //0 = Dash, 1 = Button Box
#endif

#ifdef INCLUDE_LED
// Number of Connected TM1637 modules
// 0 disabled, > 0 enabled
int TM1637_ENABLEDMODULES = 1;

#define TM1637_DIO1 9
#define TM1637_CLK1 8

TM1637Display TM1637_module1(TM1637_CLK1, TM1637_DIO1);

TM1637Display * TM1637_screens[] = {&TM1637_module1};

// -------------------------------------------------------------------------------------------------------
// MAX7219 / MAX7221 7 Segment modules -----------------------------------------------------------------------------
// http://www.dx.com/p/max7219-led-dot-matrix-digital-led-display-tube-module-cascade-391256
// -------------------------------------------------------------------------------------------------------

// 0 disabled, > 0 enabled
int MAX7221_ENABLEDMODULES = 0;
// DATA IN - pin of the first MAX7221
#define MAX7221_DATA 11
// CLK - pin of the first MAX7221
#define MAX7221_CLK 13
// LOAD(/ CS) - pin of the first MAX7221
#define MAX7221_LOAD 10
LedControl MAX7221 = LedControl(MAX7221_DATA, MAX7221_CLK, MAX7221_LOAD, MAX7221_ENABLEDMODULES);

#endif

//------------------ Protocol constants

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

// WS2812b chained RGBLEDS count
// 0 disabled, > 0 enabled
int WS2812B_RGBLEDCOUNT = 16; 
// 0 leds will be used from left to right, 1 leds will be used from right to left
int WS2812B_RIGHTTOLEFT = 1; 
// WS2812b chained RGBLEDS pins
#define WS2812B_DATAPIN 12
unsigned long lastBlinkTime = 0;
bool isLedBlack = true;
bool isBlinking = false;
byte ledBuffer[] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

Adafruit_NeoPixel WS2812B_strip = Adafruit_NeoPixel(WS2812B_RGBLEDCOUNT, WS2812B_DATAPIN, NEO_GRB + NEO_KHZ800);
         

int lastButtonState[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
int currentButtonState[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
long lastButtonDebounce[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};                              

int rowPins[] = {6,7,18,19};
int columnPins[] = {14,15,16,17};
int ENABLED_MATRIX_COLUMNS = 4;
int ENABLED_MATRIX_ROWS = 4;

// -------------------- ROTARY ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

int TOTAL_ROTARY = 2; //each rotary adds 2 more buttons

const int encoderPinA[] = {2,3};
const int encoderPinB[] = {4,5};

volatile int encoderPos[] = {15000, 15000};
volatile int orientation[] = {0, 0};
int lastRotaryPos[] = {15000, 15000};
byte lastRotaryState[] = {0, 0};
long lastRotaryStateChange = 0;

//volatile long lastRotaryBounce = 0;
long lastTimeReceivedByte = 0;

int debugMode = 0;
const byte INVALID_COMMAND_HEADER = 0xEF;
bool isInterruptDisabled[] = {false,false};
bool isAutoConfigMode = false;
long lastHandshakeSent = 0;
long lastButtonStateSent = 0;
byte oldButtonState[100];
bool isButtonDebug = false;


void convertHexToString(int offset, byte *buffer) {
  int i = 0;
  Serial.println();
  Serial.print('*');
  while (i < offset) {
     Serial.print(String(buffer[i++], DEC));        
     Serial.print('-');   
  }   
 
  Serial.println('*'); 
}

#ifdef INCLUDE_LED
byte MAX7221_ByteReorder(byte x)
{
  x = ((x >> 1) & 0x55) | ((x << 1) & 0xaa);
  x = ((x >> 2) & 0x33) | ((x << 2) & 0xcc);
  x = ((x >> 4) & 0x0f) | ((x << 4) & 0xf0);
  return (x >> 1) | ((x & 1) << 7);
}

void resetTM1637_MAX7221() {
  byte v[] = {' ',' ',' ',0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111};
  sendToTM1637_MAX7221(v, 22);
}

void autoConfigTM1637_MAX7221() {
  byte v[] = {' ',' ',' ',0b01000000,0b00111001,0b01110001,0b01111101,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111};
  sendToTM1637_MAX7221(v, 22);
}

void sendToTM1637_MAX7221(byte *buffer, int commandLength) {
  //first char(0) is the command start and second char is the command header 
  uint8_t k = 3;  
  
  // TM1637
  for (uint8_t i = 0; i < TM1637_ENABLEDMODULES ; i++) {
    TM1637_screens[i]->setSegments(buffer,3,4,0);
    k += 4;
  }
  
  // MAX7221
  for (uint8_t i = 0; i < MAX7221_ENABLEDMODULES; i++) {     
    for (uint8_t j = 0; j < 8 && k < commandLength; j++) {
      if(buffer[k] > 0) {
        //48 used as white space because 0 is used as end of msg
        if(buffer[k] == 48) {
          MAX7221.setRow(i, 7 - j, 0);
          k++;
        } else {
          MAX7221.setRow(i, 7 - j, MAX7221_ByteReorder(buffer[k++]));
        }
      }
    }
  }
}
#endif

void resetWS2812B() {
  byte v[] = {' ',' ',' ',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
  updateLedBuffer(v);
}

void testWS2812B() {
  byte v[] = {' ', ' ', ' ',1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1};
  updateLedBuffer(v);
}

void updateLedBuffer(byte *buffer) {
  //first char(0) is the command start, second char char is the device id and third is the command header
  uint8_t k = 3;
  uint8_t l = 0;
  //last byte indicates blink mode, 0 = no blink and 1 = blink
  isBlinking = buffer[(WS2812B_RGBLEDCOUNT * 3) + k] == 1;  
    
  for (uint8_t j = 0; j < WS2812B_RGBLEDCOUNT; j++) {
    if(buffer[k] < 256 && buffer[k + 1] < 256 && buffer[k + 2] < 256) {
      ledBuffer[l++] = buffer[k++];    
      ledBuffer[l++] = buffer[k++];
      ledBuffer[l++] = buffer[k++];
    }
  }  
}

void sendToWS2812B() {
  uint8_t k = 0;
  
  if (WS2812B_RGBLEDCOUNT > 0) {

    if(isBlinking) {
      if(millis() - lastBlinkTime > 100) {
        isLedBlack = !isLedBlack;
        lastBlinkTime = millis();
      }
    }
    
    for (uint8_t j = 0; j < WS2812B_RGBLEDCOUNT; j++) {
  
      uint8_t r = 0;
      uint8_t g = 0;
      uint8_t b = 0;
  
      if(!isBlinking || isLedBlack) {
        r = ledBuffer[k++];    
        g = ledBuffer[k++];
        b = ledBuffer[k++];
      } 
      
      if (WS2812B_RIGHTTOLEFT == 1) {
        WS2812B_strip.setPixelColor(WS2812B_RGBLEDCOUNT - j - 1, r, g, b);
      } else {
        WS2812B_strip.setPixelColor(j, r, g, b);
      }
    }  
  
    WS2812B_strip.show();
  }  
}

void setup()
{
  //Serial.begin(19200);
  Serial.begin(38400);
  // TM1637 INIT
#ifdef INCLUDE_LED  
  for (int i = 0; i < TM1637_ENABLEDMODULES; i++) {
    //TM1637_screens[i]->init();
    TM1637_screens[i]->setBrightness(0x0f);
    //TM1637_screens[i]->clearDisplay();
  }
  
  // MAX7221 7SEG INIT
  for (int i = 0; i < MAX7221_ENABLEDMODULES; i++) {
    MAX7221.shutdown(i, false);
    MAX7221.setIntensity(i, 15);
    MAX7221.clearDisplay(i);
  }
#endif  

  // WS2812B INIT
  if (WS2812B_RGBLEDCOUNT > 0) {
    WS2812B_strip.setBrightness(16);
    WS2812B_strip.begin();    
    WS2812B_strip.show();
  }

  // Rotary
  for(int i = 0; i < TOTAL_ROTARY; i++) {
    pinAsInputPullUp(encoderPinA[i]);      
    pinAsInputPullUp(encoderPinB[i]);        
  } 

  attachInterrupt(digitalPinToInterrupt(encoderPinA[0]), rotEncoder1, LOW); 
  attachInterrupt(digitalPinToInterrupt(encoderPinA[1]), rotEncoder2, LOW); 

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

}

bool isValidSignal(bool isHigh, int pinOffset) {
  if(isHigh) {    
    if(orientation[pinOffset] == RIGHT) {      
      return true;
    }
    orientation[pinOffset]=RIGHT;
  } else {
    if(orientation[pinOffset] == LEFT) {      
      return true;
    }
    orientation[pinOffset]=LEFT;
  }

  return false;
}

void arduinoReset() // Restarts program from beginning but does not reset the peripherals and registers
{
  asm volatile ("  jmp 0");    
}

void(* resetFunc) (void) = 0;

void rotEncoder1(){
  detachInterrupt(digitalPinToInterrupt(encoderPinA[0]));
  isInterruptDisabled[0] = true;
  int pinOffset = 0;
  int pinB = digitalState(encoderPinB[pinOffset]);
  if(pinB == HIGH) {    
    encoderPos[pinOffset]++;  
  } else{
    encoderPos[pinOffset]--;
  }
}

void rotEncoder2(){
  detachInterrupt(digitalPinToInterrupt(encoderPinA[1]));
  isInterruptDisabled[1] = true;
  int pinOffset = 1;
  int pinB = digitalState(encoderPinB[pinOffset]);
  if(pinB == HIGH) {    
    encoderPos[pinOffset]++;  
  } else{
    encoderPos[pinOffset]--;
  }
}


int sendRotaryState(int offset, byte *response) {  
  for(int i = 0; i < TOTAL_ROTARY; i++) {                  

    if(millis() - lastRotaryStateChange > 10) {  
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


int sendMatrixState(int offset, byte *response) {   
  int aux = 0;
  
  for (int i = 0; i < ENABLED_MATRIX_COLUMNS; i++) {
    
    digitalWrite(columnPins[i], LOW);
    for (int x = 0; x < ENABLED_MATRIX_ROWS; x++) {
      byte pinState = (digitalRead(rowPins[x]) == LOW) ? 1 : 0;
//if(pinState == 1) {
//  Serial.print(rowPins[i]);
//  Serial.print(" - ");
//  Serial.println(columnPins[x]);
//  delay(1000);
//}
      if(lastButtonState[aux] != pinState) {
        lastButtonDebounce[aux] = millis();
        lastButtonState[aux] = pinState;           
      }
  
      if(lastButtonDebounce[aux] > 0 && millis() - lastButtonDebounce[aux]  > 15) {
        currentButtonState[aux] = lastButtonState[aux]; 
        lastButtonDebounce[aux] = 0;  
      }
  
      response[offset++] = currentButtonState[aux++];
     
    }  
    
    digitalWrite(columnPins[i], HIGH); 
    //delay(10);      
  }     
  
/*  if(offset > 11 && response[11] == 1 && response[14] == 1) {
//      Serial.print(" RESET ");
//      Serial.flush();
    arduinoReset();
  }*/
  return offset;
}

int calculateCrc(int dataLength, byte *response) {
  int crc = 0;
  
  for(int i = 0; i < dataLength; i++) {    
    crc += response[i];
  }

  crc %= 256;

  return crc;
}

void sendDataToSerial(int commandLength, byte *response) {
  if (!isButtonDebug) {
    Serial.write(response, commandLength);  
    //DataReceived event will trigger only when a few characters are sent, including ‘0x0A’ (‘\n’)
    //Serial.write('\n');
    Serial.flush();
  } else {
    for(int x=0; x<commandLength; x++)
      Serial.print(response[x]);
    Serial.println();
    delay(1500);     
  }
}

void processCommand(byte *buffer, int commandLength) {  
  //debug
  switch(buffer[2]) {
    case CMD_SET_DEBUG_MODE :
      debugMode = buffer[3];      
      sendDebugModeState(buffer[0], debugMode);       
      break;
    
    //syn ack
    case CMD_SYN_ACK : 
      sendHandshacking();  
      break;
      
#ifdef INCLUDE_LED  
    case CMD_7_SEGS : 
      if (!isAutoConfigMode)   
        sendToTM1637_MAX7221(buffer, commandLength);      
      break;
#endif

    case CMD_RGB_SHIFT :
      updateLedBuffer(buffer);   
      break;   
  }  

  if(debugMode > 0) {
    sendDataToSerial(commandLength, buffer);  
  }
  
  buffer[0] = 0;
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

void copyArray(byte* src, int from, byte* dest, int to, int length) {
	for (int x = 0; x < length; x++) {
		dest[x + to] = src[from + x];
	}
}

void sendInvalidDataBack(byte* buffer, int commandLength) {
	byte response[commandLength + 5];
	int offset = 0;

	//handshaking
	response[offset++] = CMD_INIT;
	response[offset++] = id;
	//set debug mode response
	response[offset++] = INVALID_COMMAND_HEADER;
	copyArray(buffer, 0, response, offset, commandLength);
	offset = offset + commandLength;
	response[offset++] = calculateCrc(offset, response);
	response[offset++] = CMD_END;

	sendDataToSerial(offset, response);
}

void processData() {  
  static byte buffer[100];
  static int byteRead = 0;
  
  int commandLength = 0;

  long startReading = millis();
  while (Serial.available() || byteRead == 100) {
    commandLength = readline(Serial.read(), buffer, byteRead++);       
	if (commandLength > 0) {
/*for (int x=0; x<commandLength; x++) {
Serial.print(buffer[x]);
Serial.print(" ");
}*/	
		int crc = calculateCrc(commandLength, buffer); 	  	
/*Serial.println(commandLength);
Serial.print("calculate:");
Serial.println(crc);
Serial.println(buffer[commandLength]);*/
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
  //byte t[] = {'^',CMD_7_SEGS,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91};
  //byte s[] = {'^',CMD_RGB_SHIFT,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,129,10};
  //processCommand(s);
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

int appendArduinoId(int offset, byte *buffer) {
  for (int x = 0; x < name_.length(); x++) {
    buffer[offset++] = name_[x];   
  }

  return offset;
}

void sendHandshacking() {
  if (millis() - lastHandshakeSent > 500) {
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

boolean stateHasChanged(int offset, byte* resp) {
  boolean result = false;
  for (int i = 0; i < offset; i++) {
    if (resp[i] != oldButtonState[i]) {
      oldButtonState[i] = resp[i];
      result = true;
    }
  }

  return result;
}

void sendButtonStatus(byte header) {
  byte response[100];
  int offset = 0;

  //return buttons state      
  response[offset++] = header;
  response[offset++] = id;
  response[offset++] = CMD_BUTTON_STATUS;
  offset = sendRotaryState(offset, response);  
  offset = sendMatrixState(offset, response);
  response[offset++] = calculateCrc(offset, response);
  response[offset++] = CMD_END; 

  /*if (isButtonDebug) {
    for(int x=0; x<offset; x++)  {
      Serial.print(response[x]);
      Serial.print(" - ");
    }
    Serial.println();
    delay(2000);
  }*/

   if (stateHasChanged(offset - 1, response) || millis() - lastButtonStateSent > 10) {
    sendDataToSerial(offset, response);
    lastButtonStateSent = millis();
  }
}

void reAttachInterrupts() {
  if (isInterruptDisabled[0] == true && digitalState(encoderPinA[0]) != LOW) {
    //lastRotaryBounce = millis();
    attachInterrupt(digitalPinToInterrupt(encoderPinA[0]), rotEncoder1, LOW);
    isInterruptDisabled[0] = false;
  } 

  if (isInterruptDisabled[1] == true && digitalState(encoderPinA[1]) != LOW) {
    //lastRotaryBounce = millis();
    attachInterrupt(digitalPinToInterrupt(encoderPinA[1]), rotEncoder2, LOW);
    isInterruptDisabled[1] = false;
  }
}

bool isSerialConnected() {
  return millis() - lastTimeReceivedByte < 1000;
}

void loop() {  
  //haven't received Syn Ack from IDash for too long
  if(!isSerialConnected() && !isAutoConfigMode) {
#ifdef INCLUDE_LED      
    resetTM1637_MAX7221();
#endif    
    resetWS2812B();  
    //testWS2812B();  
    debugMode = 0;     
    delay(50);
  }  

  processData();
  
  if(!isAutoConfigMode) {        
    sendButtonStatus(CMD_INIT); 
    if (!isButtonDebug) {
      sendToWS2812B();
      sendHandshacking();
      reAttachInterrupts();
    }    
  } else {
    autoConfigTM1637_MAX7221();
  }
}
