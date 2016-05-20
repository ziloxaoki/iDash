



// Uncomment to use adafruit HT16K33 modules
//#define INCLUDE_LEDBACKPACK //{"Name":"INCLUDE_LEDBACKPACK","Type":"autodefine","Condition":"[ENABLE_ADA_HT16K33_7SEGMENTS]>0 || [ENABLE_ADA_HT16K33_Matrix]>0"}

// Uncomment to use tm1637
#define INCLUDE_TM1637 //{"Name":"INCLUDE_TM1637","Type":"autodefine","Condition":"[TM1637_ENABLEDMODULES]>0"}

#define INCLUDE_WS2812B //{"Name":"INCLUDE_WS2812B","Type":"autodefine","Condition":"[WS2812B_RGBLEDCOUNT]>0"}



// Uncomment to use Nokia 5110/3310 LCD
//#define INCLUDE_NOKIALCD //{"Name":"INCLUDE_NOKIALCD","Type":"autodefine","Condition":"[ENABLED_NOKIALCD]>0"}
#include <avr/pgmspace.h>
#include <EEPROM.h>

#include <SPI.h>
#include "Arduino.h"
#include <avr/pgmspace.h>

#include <Wire.h>
#include "LedControl.h"
#include "Adafruit_GFX.h"
#include <Servo.h>

#ifdef INCLUDE_TM1637
#include "TM1637.h"
#endif

#ifdef INCLUDE_WS2812B
#include <Adafruit_NeoPixel.h>
#endif




#define portOfPin(P)\
  (((P)>=0&&(P)<8)?&PORTD:(((P)>7&&(P)<14)?&PORTB:&PORTC))
#define ddrOfPin(P)\
  (((P)>=0&&(P)<8)?&DDRD:(((P)>7&&(P)<14)?&DDRB:&DDRC))
#define pinOfPin(P)\
  (((P)>=0&&(P)<8)?&PIND:(((P)>7&&(P)<14)?&PINB:&PINC))
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

const char COMMAND_INIT = '^';
const char COMMAND_END = '\n';

// Number of Connected TM1637 modules
// 0 disabled, > 0 enabled
int TM1637_ENABLEDMODULES = 1; //{"Group":"TM1637","Name":"TM1637_ENABLEDMODULES","Title":"TM1637 modules connected\r\nSet to 0 if none","DefaultValue":"0","Type":"integer","Template":"int TM1637_ENABLEDMODULES = {0};"}
#ifdef INCLUDE_TM1637
#define TM1637_DIO1 9 //{"Group":"TM1637","Name":"TM1637_DIO1","Title":"1st TM1637 DIO digital pin number","DefaultValue":"4","Type":"integer","Template":"#define TM1637_DIO1 {0}"}
#define TM1637_CLK1 8 //{"Group":"TM1637","Name":"TM1637_CLK1","Title":"1st TM1637 CLK digital pin number","DefaultValue":"3","Type":"integer","Template":"#define TM1637_CLK1 {0}"}

#define TM1637_DIO2 4 //{"Group":"TM1637","Name":"TM1637_DIO2","Title":"2nd TM1637 DIO digital pin number","DefaultValue":"4","Type":"integer","Template":"#define TM1637_DIO2 {0}"}
#define TM1637_CLK2 3 //{"Group":"TM1637","Name":"TM1637_CLK2","Title":"2nd TM1637 CLK digital pin number","DefaultValue":"3","Type":"integer","Template":"#define TM1637_CLK2 {0}"}

#define TM1637_DIO3 4 //{"Group":"TM1637","Name":"TM1637_DIO3","Title":"3rd TM1637 DIO digital pin number","DefaultValue":"4","Type":"integer","Template":"#define TM1637_DIO3 {0}"}
#define TM1637_CLK3 3 //{"Group":"TM1637","Name":"TM1637_CLK3","Title":"3rd TM1637 CLK digital pin number","DefaultValue":"3","Type":"integer","Template":"#define TM1637_CLK3 {0}"}

#define TM1637_DIO4 4 //{"Group":"TM1637","Name":"TM1637_DIO4","Title":"4th TM1637 DIO digital pin number","DefaultValue":"4","Type":"integer","Template":"#define TM1637_DIO4 {0}"}
#define TM1637_CLK4 3 //{"Group":"TM1637","Name":"TM1637_CLK4","Title":"4th TM1637 CLK digital pin number","DefaultValue":"3","Type":"integer","Template":"#define TM1637_CLK4 {0}"}

#define TM1637_DIO5 4 //{"Group":"TM1637","Name":"TM1637_DIO5","Title":"5th TM1637 DIO digital pin number","DefaultValue":"4","Type":"integer","Template":"#define TM1637_DIO5 {0}"}
#define TM1637_CLK5 3 //{"Group":"TM1637","Name":"TM1637_CLK5","Title":"5th TM1637 CLK digital pin number","DefaultValue":"3","Type":"integer","Template":"#define TM1637_CLK5 {0}"}

#define TM1637_DIO6 4 //{"Group":"TM1637","Name":"TM1637_DIO6","Title":"6th TM1637 DIO digital pin number","DefaultValue":"4","Type":"integer","Template":"#define TM1637_DIO6 {0}"}
#define TM1637_CLK6 3 //{"Group":"TM1637","Name":"TM1637_CLK6","Title":"6th TM1637 CLK digital pin number","DefaultValue":"3","Type":"integer","Template":"#define TM1637_CLK6 {0}"}


TM1637 TM1637_module1(TM1637_CLK1, TM1637_DIO1);
TM1637 TM1637_module2(TM1637_CLK2, TM1637_DIO2);
TM1637 TM1637_module3(TM1637_CLK3, TM1637_DIO3);
TM1637 TM1637_module4(TM1637_CLK4, TM1637_DIO4);
TM1637 TM1637_module5(TM1637_CLK5, TM1637_DIO5);
TM1637 TM1637_module6(TM1637_CLK6, TM1637_DIO6);

TM1637 * TM1637_screens[] = { &TM1637_module1, &TM1637_module2, &TM1637_module3, &TM1637_module4, &TM1637_module5, &TM1637_module6 };
#endif

// -------------------------------------------------------------------------------------------------------
// MAX7219 / MAX7221 7 Segment modules -----------------------------------------------------------------------------
// http://www.dx.com/p/max7219-led-dot-matrix-digital-led-display-tube-module-cascade-391256
// -------------------------------------------------------------------------------------------------------

// 0 disabled, > 0 enabled
int MAX7221_ENABLEDMODULES = 2; //{"Group":"MAX7221","Name":"MAX7221_ENABLEDMODULES","Title":"MAX7219 / MAX7221 7 Segment modules connected \r\nSet to 0 if none\r\nMultiple modules can be cascaded connected module output to next module input","DefaultValue":"0","Type":"integer","Template":"int MAX7221_ENABLEDMODULES = {0};"}
// DATA IN - pin of the first MAX7221
#define MAX7221_DATA 11 //{"Group":"MAX7221","Name":"MAX7221_DATA","Title":"DATA (DIN) digital pin number","DefaultValue":"3","Type":"integer","Template":"#define MAX7221_DATA {0}"}
// CLK - pin of the first MAX7221
#define MAX7221_CLK 13 //{"Group":"MAX7221","Name":"MAX7221_CLK","Title":"CLOCK (CLK) digital pin number","DefaultValue":"5","Type":"integer","Template":"#define MAX7221_CLK {0}"}
// LOAD(/ CS) - pin of the first MAX7221
#define MAX7221_LOAD 10 //{"Group":"MAX7221","Name":"MAX7221_LOAD","Title":"LOAD (LD) digital pin number","DefaultValue":"4","Type":"integer","Template":"#define MAX7221_LOAD {0}"}
LedControl MAX7221 = LedControl(MAX7221_DATA, MAX7221_CLK, MAX7221_LOAD, MAX7221_ENABLEDMODULES);

// WS2812b chained RGBLEDS count
// 0 disabled, > 0 enabled
int WS2812B_RGBLEDCOUNT = 16; //{"Group":"WS2812B_RGBLED","Name":"WS2812B_RGBLEDCOUNT","Title":"WS2812B RGB leds count\r\nSet to 0 if none","DefaultValue":"0","Type":"integer","Template":"int WS2812B_RGBLEDCOUNT = {0};"}
// 0 leds will be used from left to right, 1 leds will be used from right to left
int WS2812B_RIGHTTOLEFT = 1; //{"Group":"WS2812B_RGBLED","Name":"WS2812B_RIGHTTOLEFT","Title":"Reverse led order \r\n0 = No, 1 = Yes","DefaultValue":"0","Type":"boolean","Template":"int WS2812B_RIGHTTOLEFT = {0};"}
// WS2812b chained RGBLEDS pins
#define WS2812B_DATAPIN 12 //{"Group":"WS2812B_RGBLED","Name":"WS2812B_DATAPIN","Title":"Data (DIN) digital pin number","DefaultValue":"6","Type":"boolean","Template":"#define WS2812B_DATAPIN {0}"}

#ifdef INCLUDE_WS2812B
Adafruit_NeoPixel WS2812B_strip = Adafruit_NeoPixel(WS2812B_RGBLEDCOUNT, WS2812B_DATAPIN, NEO_GRB + NEO_KHZ800);
#endif

// ----------------------- ADDITIONAL BUTTONS ---------------------------------------------------------------
// https://www.arduino.cc/en/Tutorial/InputPullupSerial
// ----------------------------------------------------------------------------------------------------------
int ENABLED_BUTTONS_COUNT = 2; //{"Group":"ADDITIONAL BUTTONS","Name":"ENABLED_BUTTONS_COUNT","Title":"Additional buttons (directly connected to the arduino) buttons count\r\n0 = disabled, >0  = enabled","DefaultValue":"0","Type":"integer","Template":"int ENABLED_BUTTONS_COUNT = {0};"}
int BUTTON_PIN_1 = 6; //{"Group":"ADDITIONAL BUTTONS","Name":"BUTTON_PIN_1","Title":"1'st Additional button digital pin","DefaultValue":"3","Type":"integer","Template":"int BUTTON_PIN_1 = {0};"}
int BUTTON_PIN_2 = 7; //{"Group":"ADDITIONAL BUTTONS","Name":"BUTTON_PIN_2","Title":"2'nd Additional button digital pin","DefaultValue":"3","Type":"integer","Template":"int BUTTON_PIN_2 = {0};"}
int BUTTON_PIN_3 = 7; //{"Group":"ADDITIONAL BUTTONS","Name":"BUTTON_PIN_3","Title":"3'rd Additional button digital pin","DefaultValue":"3","Type":"integer","Template":"int BUTTON_PIN_3 = {0};"}
int BUTTON_PIN_4 = 7; //{"Group":"ADDITIONAL BUTTONS","Name":"BUTTON_PIN_4","Title":"4'th Additional button digital pin","DefaultValue":"3","Type":"integer","Template":"int BUTTON_PIN_4 = {0};"}
int BUTTON_PIN_5 = 7; //{"Group":"ADDITIONAL BUTTONS","Name":"BUTTON_PIN_5","Title":"5'th Additional button digital pin","DefaultValue":"3","Type":"integer","Template":"int BUTTON_PIN_5 = {0};"}
int BUTTON_PIN_6 = 7; //{"Group":"ADDITIONAL BUTTONS","Name":"BUTTON_PIN_6","Title":"6'th Additional button digital pin","DefaultValue":"3","Type":"integer","Template":"int BUTTON_PIN_6 = {0};"}
int BUTTON_PIN_7 = 7; //{"Group":"ADDITIONAL BUTTONS","Name":"BUTTON_PIN_7","Title":"7'th Additional button digital pin","DefaultValue":"3","Type":"integer","Template":"int BUTTON_PIN_7 = {0};"}
int BUTTON_PIN_8 = 7; //{"Group":"ADDITIONAL BUTTONS","Name":"BUTTON_PIN_8","Title":"8'th Additional button digital pin","DefaultValue":"3","Type":"integer","Template":"int BUTTON_PIN_8 = {0};"}
int BUTTON_PINS[] = { BUTTON_PIN_1, BUTTON_PIN_2, BUTTON_PIN_3, BUTTON_PIN_4, BUTTON_PIN_5, BUTTON_PIN_6, BUTTON_PIN_7, BUTTON_PIN_8 };


// -------------------- ANALOG ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

const int EXTRA_BUTTONS_TOTAL = 4;  //Extra pins. Each pin can handle multiple buttons

int EXTRA_BUTTONS_INIT[8][4] = {{21, INPUT_PULLUP}, //A7 Left paddle - INPUT_PULLUP
                                {20, INPUT_PULLUP}, //A6 Right Paddle - INPUT_PULLUP
                                {19, INPUT},        //A5 Extra 1 - INPUT
                                {18, INPUT},        //A4 Extra 2 - INPUT
                                {17, INPUT},        //A3
                                {16, INPUT},        //A2
                                {15, INPUT},        //A1
                                {14, INPUT}};       //A0

int MAXIMUM_BUTTONS_PER_ANALOG = 4;

int BUTTON_LIMITS[8][4][2] = {{{350, 900}, {-1, -1}, {-1, -1}, {-1, -1}},         //A7 Left paddle - INPUT_PULLUP
                              {{350, 900}, {-1, -1}, {-1, -1}, {-1, -1}},         //A6 Right Paddle - INPUT_PULLUP
                              {{605, 625}, {500, 525}, {665, 695}, {715, 745}},   //A5 Extra 1 - INPUT
                              {{500, 525}, {600, 625}, {715, 745}, {665, 705}},   //A4 Extra 2 - INPUT                 
                              {{-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}},           //A3
                              {{-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}},           //A2
                              {{-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}},           //A1
                              {{-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}}};          //A0
                              
int GROUND_ANALOG_PIN = 15; //A1                                

int extra_button_states[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
int extra_button_last_states[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};   // the previous reading from the input pin                            

// -------------------- ROTARY ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

int TOTAL_ROTARY = 2; //each rotary adds 2 more buttons

const int encoderPinA[] = {2,3};
const int encoderPinB[] = {4,5};

volatile int encoderPos[] = {15000, 15000};
int lastPos[] = {15000, 15000};

long lastRotaryBounce = 0;
long lastSynAck = 0;

bool syn_ack = false;

boolean isDebug = false;



int asize(byte* b) {
  return sizeof(b) / sizeof(byte);
}

void setup()
{
  Serial.begin(19200);
  
  // TM1637 INIT
  #ifdef INCLUDE_TM1637
    for (int i = 0; i < TM1637_ENABLEDMODULES; i++) {
      TM1637_screens[i]->init();
      TM1637_screens[i]->set(BRIGHT_TYPICAL);
      TM1637_screens[i]->clearDisplay();
    }
  #endif

  
  #ifdef INCLUDE_WS2812B
    // WS2812B INIT
    if (WS2812B_RGBLEDCOUNT > 0) {
      WS2812B_strip.begin();
      WS2812B_strip.show();
    }
  #endif

  // WS2801 INIT
  #ifdef INCLUDE_WS2801
    if (WS2801_RGBLEDCOUNT > 0) {
      WS2801_strip.begin();
      WS2801_strip.show();
    }
  #endif

  // MAX7221 7SEG INIT
  for (int i = 0; i < MAX7221_ENABLEDMODULES; i++) {
    MAX7221.shutdown(i, false);
    MAX7221.setIntensity(i, 15);
    MAX7221.clearDisplay(i);
  }

  // EXTERNAL BUTTONS INIT
  for (int btnIdx = 0; btnIdx < ENABLED_BUTTONS_COUNT; btnIdx++) {
    pinAsInputPullUp(BUTTON_PINS[btnIdx]);
  }






  // Extra buttons
  for(int i = 0; i < EXTRA_BUTTONS_TOTAL; i++) { 
    if(EXTRA_BUTTONS_INIT[i][1] == INPUT_PULLUP) {
      pinAsInputPullUp(EXTRA_BUTTONS_INIT[i][0]);
    } else {
      pinAsInput(EXTRA_BUTTONS_INIT[i][0]);
    }
  }

  // Rotary
  for(int i = 0; i < TOTAL_ROTARY; i++) {
    pinAsInputPullUp(encoderPinA[i]);      
    pinAsInputPullUp(encoderPinB[i]);        
  } 
  attachInterrupt(digitalPinToInterrupt(encoderPinA[0]), rotEncoder1, LOW); 
  attachInterrupt(digitalPinToInterrupt(encoderPinA[1]), rotEncoder2, LOW); 

}


void rotEncoder1(){
  int pinB = digitalState(encoderPinB[0]);
  if(millis() - lastRotaryBounce > 20) {
    if (pinB == LOW) {
      encoderPos[0]--;  
    } else {
      encoderPos[0]++;
    }
  }
  lastRotaryBounce = millis();
}

void rotEncoder2(){
  int pinB = digitalState(encoderPinB[1]);
  if(millis() - lastRotaryBounce > 20) {
    if (pinB == LOW) {
      encoderPos[1]--;  
    } else {
      encoderPos[1]++;
    }
  }
  
  lastRotaryBounce = millis();
}


int sendRotaryState(int offset, byte *response) {  
  for(int i = 0; i < TOTAL_ROTARY; i++) {                  
    
    if(encoderPos[i] != lastPos[i]) {      
      if(lastPos[i] < encoderPos[i]) {      
        response[offset++] = 0;
        response[offset++] = 1;
      } else {        
        response[offset++] = 1;
        response[offset++] = 0;        
      }
      lastPos[i] = encoderPos[i];        
      
    } else {
      response[offset++] = 0;
      response[offset++] = 0;       
    }              
  }

  return offset;
}


int sendAnalogState(int offset, byte *response) {

  for(int i = 0; i < EXTRA_BUTTONS_TOTAL; i++) {
    // read the state of the switch into a local variable:
    int reading = analogRead(EXTRA_BUTTONS_INIT[i][0]);  

    for(int x = 0; x < MAXIMUM_BUTTONS_PER_ANALOG; x++) {
    
      int tmpButtonState = LOW;             // the current reading from the input pin
       
      if(reading > BUTTON_LIMITS[i][x][0] && reading < BUTTON_LIMITS[i][x][1]){
        //Read switch 1
        tmpButtonState = 1;
      }
             
      response[offset++] = tmpButtonState;                
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

int calculateCrc(int commandLength, byte *response) {
  int crc = 0;
  
  for(int i = 0; i < commandLength; i++) {    
    crc += response[i];
  }

  crc %= 256;

  return crc;
}

void sendDataToSerial(int commandLength, byte *response) {
  for(int i = 0; i < commandLength; i++) {
    if(isDebug) {      
      Serial.print(response[i]);
    } else {
      Serial.write(response[i]);
    }
  }

  if(isDebug) {
      Serial.println();
      delay(2000);
  }

  Serial.flush();
}

void processCommand(byte *buffer) {
  //debug
  switch(buffer[0]) {
    case 1 :
      returnDebugData(buffer);
      break;
    
    //syn ack
    case 'a' : 
      syn_ack = true;
      lastSynAck = millis();
      break;
  }  
  buffer[0] = 0;
}

void echo(byte *buffer) {
  byte response[50];
  int offset = 0;
  
  //return buttons state      
  response[offset++] = COMMAND_INIT;
  response[offset++] = 1; 
  for(int i = 0; buffer[i] != '\n'; i++) { 
    response[offset++] = buffer[i];
  }  
  response[offset++] = '\n';
  response[offset++] = calculateCrc(offset - 1, response);
  response[offset++] = COMMAND_END;   
  
  sendDataToSerial(offset, response);   
}

int returnDebugData(byte *buffer) {
  switch (buffer[1]) {    
    case 'A': // command init found (this is ignored and not appended to the buffer)
      sendHandshacking(true); 
      break;
    case 'a':
      { byte r[] = {'S','Y','N',' ','A','C','K',' ','r','e','c','e','i','v','e','d','.'};      
        echo(r); }
      break;
    case 'D':  
      sendButtonStatus(true);  
      break;    
    default:
      echo(buffer);
      break;
  }
}

int readline(int readch, byte *buffer, int len)
{  
  static int pos = 0;
  int rpos;

  if (readch > 0) {
    switch (readch) {
      case '^': // command init found (this is ignored and not appended to the buffer)
        pos = 0;
        break;        
      case '\n': // End command
        rpos = pos;
        pos = 0;  // Reset position index ready for next time
        return rpos;
      default:
        if (pos < len-1) {
          buffer[pos++] = readch;
          buffer[pos] = 0;
        }
    }
  }
  // No end of line has been found, so return -1.
  return -1;
}


void processData() {  
  static byte buffer[50];
  int commandLength = 0;
  
  if(isDebug) {
    commandLength = 3;
  } else {
    commandLength = readline(Serial.read(), buffer, 50);    
  }
  
  if ( commandLength > 0) {   
    processCommand(buffer);    
  }
  
}

void sendHandshacking(bool isDebug) {
  byte response[5];
  int offset = 0;
  
  //handshaking
  response[offset++] = COMMAND_INIT;
  if(isDebug) {
    response[offset++] = 1;
  }
  response[offset++] = 'A';
  response[offset++] = calculateCrc(offset - 1, response);
  response[offset++] = COMMAND_END;
  
  sendDataToSerial(offset, response);
}

void sendButtonStatus(bool isDebug) {
  byte response[50];
  int offset = 0;
  
  //return buttons state      
  response[offset++] = COMMAND_INIT;
  if(isDebug) {
    response[offset++] = 1;  
  }
  response[offset++] = 'D';
  offset = sendButtonState(offset, response);
  offset = sendAnalogState(offset, response);
  offset = sendRotaryState(offset, response);
  response[offset++] = calculateCrc(offset - 1, response);
  response[offset++] = COMMAND_END;   
  
  sendDataToSerial(offset, response);   
}

void loop() {  
   
  //haven't received Syn Ack from IDash for too long
  if(millis() - lastSynAck > 5000) {
    syn_ack = false;
  }
  if(syn_ack) {
    sendButtonStatus(false);       
  } else {
    //keep sending SYN until SYN-ACK received
    sendHandshacking(false);
    delay(100);
  }

  processData();
}





