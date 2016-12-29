#include <avr/pgmspace.h>
#include <EEPROM.h>
#include <SPI.h>
#include "Arduino.h"
#include <avr/pgmspace.h>
#include "LedControl.h"
#include <TM1637Display.h>
#include <Adafruit_NeoPixel.h>
#include <MemoryFree.h>


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

// -------------------------------------------------------------------------------------------------------
// TM1637 7 Segment modules -----------------------------------------------------------------------------
//
// -------------------------------------------------------------------------------------------------------


// Number of Connected TM1637 modules
// 0 disabled, > 0 enabled
int TM1637_ENABLEDMODULES = 1;

#define TM1637_DIO1 9
#define TM1637_CLK1 8

#define TM1637_DIO2 4
#define TM1637_CLK2 3

#define TM1637_DIO3 4
#define TM1637_CLK3 3

#define TM1637_DIO4 4
#define TM1637_CLK4 3

#define TM1637_DIO5 4
#define TM1637_CLK5 3

#define TM1637_DIO6 4
#define TM1637_CLK6 3


TM1637Display TM1637_module1(TM1637_CLK1, TM1637_DIO1);
TM1637Display TM1637_module2(TM1637_CLK2, TM1637_DIO2);
TM1637Display TM1637_module3(TM1637_CLK3, TM1637_DIO3);
TM1637Display TM1637_module4(TM1637_CLK4, TM1637_DIO4);
TM1637Display TM1637_module5(TM1637_CLK5, TM1637_DIO5);
TM1637Display TM1637_module6(TM1637_CLK6, TM1637_DIO6);

TM1637Display * TM1637_screens[] = { &TM1637_module1, &TM1637_module2, &TM1637_module3, &TM1637_module4, &TM1637_module5, &TM1637_module6 };

// -------------------------------------------------------------------------------------------------------
// MAX7219 / MAX7221 7 Segment modules -----------------------------------------------------------------------------
// http://www.dx.com/p/max7219-led-dot-matrix-digital-led-display-tube-module-cascade-391256
// -------------------------------------------------------------------------------------------------------

// 0 disabled, > 0 enabled
int MAX7221_ENABLEDMODULES = 2;
// DATA IN - pin of the first MAX7221
#define MAX7221_DATA 11
// CLK - pin of the first MAX7221
#define MAX7221_CLK 13
// LOAD(/ CS) - pin of the first MAX7221
#define MAX7221_LOAD 10
LedControl MAX7221 = LedControl(MAX7221_DATA, MAX7221_CLK, MAX7221_LOAD, MAX7221_ENABLEDMODULES);

/*
struct ScreenItem {
public:
  byte Intensity;

  ScreenItem() { }
};


ScreenItem MAX7221_screen1;
ScreenItem MAX7221_screen2;
ScreenItem MAX7221_screen3;
ScreenItem MAX7221_screen4;
ScreenItem MAX7221_screen5;
ScreenItem MAX7221_screen6;

ScreenItem * MAX7221_screens[] = { &MAX7221_screen1, &MAX7221_screen2, &MAX7221_screen3, &MAX7221_screen4, &MAX7221_screen5, &MAX7221_screen6 };
*/

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
const byte CMD_INVALID = (byte)0xef; //239d EFh

// WS2812b chained RGBLEDS count
// 0 disabled, > 0 enabled
int WS2812B_RGBLEDCOUNT = 16; 
// 0 leds will be used from left to right, 1 leds will be used from right to left
int WS2812B_RIGHTTOLEFT = 1; 
// WS2812b chained RGBLEDS pins
#define WS2812B_DATAPIN 12

Adafruit_NeoPixel WS2812B_strip = Adafruit_NeoPixel(WS2812B_RGBLEDCOUNT, WS2812B_DATAPIN, NEO_GRB + NEO_KHZ800);

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

int BUTTON_LIMITS[8][4][2] = {{{400, 900}, {-1, -1}, {-1, -1}, {-1, -1}},         //A7 Left paddle - INPUT_PULLUP
                              {{400, 900}, {-1, -1}, {-1, -1}, {-1, -1}},         //A6 Right Paddle - INPUT_PULLUP
                              {{450, 520}, {550, 620}, {625, 685}, {690, 745}},   //A5 Extra 1 - INPUT
                              {{450, 520}, {550, 620}, {625, 685}, {690, 745}},   //A4 Extra 2 - INPUT                 
                              {{-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}},           //A3
                              {{-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}},           //A2
                              {{-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}},           //A1
                              {{-1, -1}, {-1, -1}, {-1, -1}, {-1, -1}}};          //A0
                              
int GROUND_ANALOG_PIN = 15; //A1                                

int extra_button_states[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
int extra_button_last_states[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};   // the previous reading from the input pin               
long lastButtonBounce = 0;             

// -------------------- ROTARY ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

int TOTAL_ROTARY = 2; //each rotary adds 2 more buttons

const int encoderPinA[] = {2,3};
const int encoderPinB[] = {4,5};

volatile int encoderPos[] = {15000, 15000};
volatile int orientation[] = {0, 0};
int lastPos[] = {15000, 15000};
byte lastState[] = {0, 0};
long lastRotaryStateChange = 0;

volatile long lastRotaryBounce = 0;
long lastTimeReceivedByte = 0;

//bool isConnected = false;
int debugMode = 0;
const byte INVALID_COMMAND_HEADER = 0xEF;
//long lastMessageSent = 0;

int asize(byte* b) {
  return sizeof(b) / sizeof(byte);
}

int acopy(byte* orig, byte* dest, int length) {
  for(int x = 0; x < length; x++) {
    dest[x] = orig[x];
  }
}


byte MAX7221_ByteReorder(byte x)
{
  x = ((x >> 1) & 0x55) | ((x << 1) & 0xaa);
  x = ((x >> 2) & 0x33) | ((x << 2) & 0xcc);
  x = ((x >> 4) & 0x0f) | ((x << 4) & 0xf0);
  return (x >> 1) | ((x & 1) << 7);
}

void resetTM1637_MAX7221() {
  byte v[] = {' ',' ',0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111,0b00111111};
  sendToTM1637_MAX7221(v);
}

void resetWS2812B() {
  byte v[] = {' ',' ',0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}; 
  sendToWS2812B(v);
}

void testWS2812B() {
  byte v[] = {CMD_INIT,CMD_RGB_SHIFT, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 255, 1, 1, 1, 1, 255, 1, 1, 255, 1, 1, 255 };
  sendToWS2812B(v);
}

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

void sendToTM1637_MAX7221(byte *buffer) {
  //first char(0) is the command start and second char is the command header 
  uint8_t k = 2;  

  // TM1637
  for (uint8_t i = 0; i < TM1637_ENABLEDMODULES ; i++) {
    TM1637_screens[i]->setSegments(buffer,2,4,0);
    k += 4;
  }
  
  // MAX7221
  for (uint8_t i = 0; i < MAX7221_ENABLEDMODULES; i++) {     
    for (uint8_t j = 0; j < 8; j++) {
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

void sendToWS2812B(byte *buffer) {
  //first char(0) is the command start and second char is the command header 
  uint8_t k = 2;
  for (uint8_t j = 0; j < WS2812B_RGBLEDCOUNT; j++) {
    uint8_t r = buffer[k++];    
    uint8_t g = buffer[k++];
    uint8_t b = buffer[k++];
    if (WS2812B_RIGHTTOLEFT == 1) {
      WS2812B_strip.setPixelColor(WS2812B_RGBLEDCOUNT - j - 1, r, g, b);
    }
    else {
      WS2812B_strip.setPixelColor(j, r, g, b);
    }
  }
  
  if (WS2812B_RGBLEDCOUNT > 0) {
    WS2812B_strip.show();
  }
}

void setup()
{
  //Serial.begin(19200);
  Serial.begin(38400);
  // TM1637 INIT
  for (int i = 0; i < TM1637_ENABLEDMODULES; i++) {
    //TM1637_screens[i]->init();
    TM1637_screens[i]->setBrightness(0x0f);
    //TM1637_screens[i]->clearDisplay();
  }

  
  // WS2812B INIT
  if (WS2812B_RGBLEDCOUNT > 0) {
    WS2812B_strip.setBrightness(16);
    WS2812B_strip.begin();    
    WS2812B_strip.show();
  }


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

bool isValidSignal(bool isHigh, int offset) {
  if(isHigh) {    
    if(orientation[offset] > 0) {      
      return true;
    }
    orientation[offset]+=1;
  } else {
    if(orientation[offset] < 0) {      
      return true;
    }
    orientation[offset]-=1;
  }

  return false;
}

void rotEncoder1(){
  //cli(); //stop interrupts happening before we read pin values  
  int pinOffset = 0;
  if(millis() - lastRotaryBounce > 10) {
    int pinB = digitalState(encoderPinB[pinOffset]);

    //anti bounce incorrect reading. First reading is lost but prevent incorrect reading.
    if (isValidSignal(pinB, pinOffset)) {  
      if(pinB == HIGH) {    
        encoderPos[pinOffset]++;  
//        Serial.println("Anticlockwise");
//        Serial.flush();
      } else{
        encoderPos[pinOffset]--;
//        Serial.println("Clockwise");
//        Serial.flush();
      }
    }
  }  
  lastRotaryBounce = millis();
  //sei(); //restart interrupts
}

void rotEncoder2(){
  //cli(); //stop interrupts happening before we read pin values  
  int pinOffset = 1;
  if(millis() - lastRotaryBounce > 10) {
    int pinB = digitalState(encoderPinB[pinOffset]);

    //anti bounce incorrect reading. First reading is lost but prevent incorrect reading.
    if (isValidSignal(pinB, pinOffset)) {  
      if(pinB == HIGH) {    
        encoderPos[pinOffset]++;  
        //Serial.println("Clockwise");
        //Serial.flush();
      } else{
        encoderPos[pinOffset]--;
        //Serial.println("Anticlockwise");
        //Serial.flush();
      }
    }
  }  
  lastRotaryBounce = millis();
  //sei(); //restart interrupts
}


int sendRotaryState(int offset, byte *response) {  
  for(int i = 0; i < TOTAL_ROTARY; i++) {                  

    if(millis() - lastRotaryStateChange > 100) {  
      if(encoderPos[i] != lastPos[i]) {      
        if(lastPos[i] < encoderPos[i]) {  
          lastState[i] = 1;    
        } else {      
          lastState[i] = 2;       
        }
        lastPos[i] = encoderPos[i];        
        
      } else {
        lastState[i] = 0;     
      }  
      if(i == TOTAL_ROTARY - 1) {
        lastRotaryStateChange = millis();              
      }
    }

    switch(lastState[i]) {
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


int sendAnalogState(int offset, byte *response) {

  int btn = 0;
  
  for(int i = 0; i < EXTRA_BUTTONS_TOTAL; i++) {
    // read the state of the switch into a local variable:
    int reading = analogRead(EXTRA_BUTTONS_INIT[i][0]); 
/*if(i == 2 || i == 3) {
  Serial.println(reading);
  delay(500);    
}*/
    for(int x = 0; x < MAXIMUM_BUTTONS_PER_ANALOG; x++) {
      int tmpButtonState = LOW;             // the current reading from the input pin

      if((reading > BUTTON_LIMITS[i][x][0]) && (reading < BUTTON_LIMITS[i][x][1])){           
        //Read switch 1       
        tmpButtonState = 1; 
      }      
      if((tmpButtonState != extra_button_last_states[btn]) && (millis() - lastButtonBounce > 50)) {
        extra_button_last_states[btn++] = tmpButtonState; 
        response[offset++] = tmpButtonState;
        lastButtonBounce = millis(); 
      } else {
        response[offset++] = extra_button_last_states[btn++];
      }      
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

int calculateCrc(int dataLength, byte *response) {
  int crc = 0;
  
  for(int i = 0; i < dataLength; i++) {    
    crc += response[i];
  }

  crc %= 256;

  return crc;
}

void sendDataToSerial(int commandLength, byte *response) {
  Serial.write(response, commandLength);
  Serial.flush();
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
      //isConnected = true;
      sendHandshacking();  
      break;

    case CMD_7_SEGS :    
      sendToTM1637_MAX7221(buffer);      
      break;

    case CMD_RGB_SHIFT :
      sendToWS2812B(buffer);      
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


void processData() {  
  static byte buffer[100];
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
  //byte t[] = {'^',CMD_7_SEGS,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91,91};
  //byte s[] = {'^',CMD_RGB_SHIFT,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,255,0,255,129,10};
  //processCommand(s);
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

void sendHandshacking() {
  byte response[4];
  int offset = 0;
  
  //handshaking
  response[offset++] = CMD_INIT;
  response[offset++] = CMD_SYN;
  response[offset++] = calculateCrc(offset - 1, response);
  response[offset++] = CMD_END;
  
  sendDataToSerial(offset, response);
}

void sendButtonStatus(byte header) {
  byte response[100];
  int offset = 0;
  
  //return buttons state      
  response[offset++] = header;
  response[offset++] = CMD_BUTTON_STATUS;
  offset = sendButtonState(offset, response);
  offset = sendAnalogState(offset, response);
  offset = sendRotaryState(offset, response);
  response[offset++] = calculateCrc(offset - 1, response);
  response[offset++] = CMD_END;   
  
  sendDataToSerial(offset, response);
}

void loop() {  
//    Serial.print("freeMemory()=");
//    Serial.println(freeMemory());
  //haven't received Syn Ack from IDash for too long
  if(millis() - lastTimeReceivedByte > 1000 /*|| !isConnected*/) {
    resetTM1637_MAX7221();
    resetWS2812B();  
    //testWS2812B();
    //isConnected = false;  
    //isDebugMode = false;  
    debugMode = 0;  
    //sendHandshacking();   
    delay(50);
  }
  
  processData();
  
  //if(isConnected) {
    sendButtonStatus(CMD_INIT); 
  //}  

}
