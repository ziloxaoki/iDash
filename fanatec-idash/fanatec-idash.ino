#include <Keypad.h>
#include <Encoder.h>
#include <TM1637.h>

#include <Adafruit_NeoPixel.h>

//{0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
//0~9,A,b,C,d,E,F,"-"," ",degree,r,h,n
// 10 -> A, 11 -> b, 12 ->C, 13 -> d, 14 -> E, 15 -> F
// 16 -> -, 17 -> space, 18 -> degree sign, 19 -> r, 20 -> h, 21 -> n

#define CMD_INIT 200 //94d 5Eh
#define CMD_INIT_DEBUG 201 //95d 5Fh
#define CMD_END (byte)'~' //126d 7Eh
#define CMD_SET_DEBUG_MODE 11
#define CMD_RESPONSE_SET_DEBUG_MODE 12
#define CMD_SYN (byte)'A';//65d 41h
#define CMD_7_SEGS (byte)'B' //66d 42h
#define CMD_SYN_ACK (byte)'a' //97d 61h
#define CMD_RGB_SHIFT (byte)'C' //67d 43h
#define CMD_BUTTON_STATUS (byte)'D' //68d 44h
#define CMD_DEBUG_BUTTON 202 //CAh
#define CMD_INVALID (byte)0xef //239d EFh

#define ENABLED_MATRIX_COLUMNS 3
#define ENABLED_MATRIX_ROWS 4
#define TM1637_DIO A0
#define TM1637_CLK 9
#define TM1637_ENABLEDMODULES 1
#define TOTAL_ROTARY 4
#define WS2812B_DATAPIN 22
#define id 34
#define TYPE 0 //0 = Dash, 1 = Button Box
#define WS2812B_RGBLEDCOUNT 16 
// 0 leds will be used from left to right, 1 leds will be used from right to leftUU
#define WS2812B_RIGHTTOLEFT 1 
#define INVALID_COMMAND_HEADER 0xEF

byte rowPins[] = {0, 1, 11, 15}; //output
byte columnPins[] = {12, 13, 14}; //pull-up
long lastknobState[] = {0, 0, 0, 0}; 
long knobState[] = {0, 0, 0, 0}; 
long knobPosition[] = {0, 0, 0, 0};

byte thumbstick1[] = {A1, A2};
byte thumbstick2[] = {A3, A4};

TM1637 tm1637(TM1637_CLK,TM1637_DIO);

Adafruit_NeoPixel WS2812B_strip = Adafruit_NeoPixel(WS2812B_RGBLEDCOUNT, WS2812B_DATAPIN, NEO_GRB + NEO_KHZ800);
//byte ledBuffer[] = {0,0,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0,0,255,0};
byte ledBuffer[] = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

Encoder knob1(5, 16);
Encoder knob2(6, 3);
Encoder knob3(7, 4);
Encoder knob4(8, 2);

byte buttonNumber = 1;
long lastKnobStateChange = 0;
//id cannot start with Arduino
String name_ = "Wheel-Custom";

long lastTimeReceivedByte = 0;

//Flags
int debugMode = 0;
bool isAutoConfigMode = false;
unsigned long lastBlinkTime = 0;
bool isLedBlack = true;
bool isBlinking = false;

void sendMatrixState() {     
  for (byte i = 0; i < ENABLED_MATRIX_ROWS; i++) {
    initOutputPins();
    digitalWrite(rowPins[i], LOW);
    for (byte x = 0; x < ENABLED_MATRIX_COLUMNS; x++) {
      byte pinState = (digitalRead(columnPins[x]) == LOW) ? 1 : 0;
      Joystick.button(buttonNumber++, pinState);  
      Joystick.send_now();
    }  
    //delay(10);  
  }   
}

void senKnobState() {  
  long newKnobPosition[] = {0, 0, 0, 0};
  
  newKnobPosition[0] = knob1.read();
  newKnobPosition[1] = knob2.read();
  newKnobPosition[2] = knob3.read();
  newKnobPosition[3] = knob4.read();

  for (byte i = 0; i < TOTAL_ROTARY; i++) {
    if(millis() - lastKnobStateChange > 50) {
      if (knobPosition[i] > newKnobPosition[i] + 3 || knobPosition[i] < newKnobPosition[i] - 3) {
        if(knobPosition[i] < newKnobPosition[i]) {  
          //turn left
          knobState[i] = 1;    
        } else {     
          //turn right          
          knobState[i] = 2;       
        }
        knobPosition[i] = newKnobPosition[i];        

        lastKnobStateChange = millis();  
      } else {
        //not pressed
        knobState[i] = 0; 
      }
    }

    switch(knobState[i]) {
      case 0:
        Joystick.button(buttonNumber++, 0);  
        Joystick.button(buttonNumber++, 0);
        Joystick.send_now(); 
        break;
      case 1:
        Joystick.button(buttonNumber++, 0);  
        Joystick.button(buttonNumber++, 1);
        Joystick.send_now(); 
        break;
      case 2:
        Joystick.button(buttonNumber++, 1);  
        Joystick.button(buttonNumber++, 0);
        Joystick.send_now(); 
        break;
    }
  }
}

void initOutputPins() {
  for (byte x = 0; x < ENABLED_MATRIX_ROWS; x++) {
    pinMode(rowPins[x], OUTPUT);    
    digitalWrite(rowPins[x], HIGH);        // initiate high   
  }
}

void sendToTM1637(byte *buffer) {
  byte offset = 0;
  //first char(0) is the command start, second char is the device id and third char is the command header  
  for(byte i = 3; i < 7; i++) {
    tm1637.display(offset++, buffer[i]);
  }
}

void updateLedBuffer(byte *buffer) {
  //first char(0) is the command start and second char is the command header 
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

void sendDataToSerial(int commandLength, byte *response) {
  Serial.write(response, commandLength);  
  Serial.flush();
  /*for(int x=0; x<commandLength; x++) 
  Serial.print(response[x]);
  Serial.println();*/
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

int calculateCrc(int dataLength, byte *response) {
  int crc = 0;
  
  for(int i = 0; i < dataLength; i++) {    
    crc += response[i];
  }

  crc %= 256;

  return crc;
}

int appendArduinoId(int offset, byte *buffer) {
  for (int x = 0; x < name_.length(); x++) {
    buffer[offset++] = name_[x];   
  }

  return offset;
}

void sendHandshacking() {
  byte response[20];
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
			//todo: send invalid data to serial for debugging
		}
		break;
	}
  }

  buffer[0] = 0;
  byteRead = 0;
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
      
    case CMD_7_SEGS : 
      if (!isAutoConfigMode)   
        sendToTM1637(buffer);      
      break;

    case CMD_RGB_SHIFT :
      updateLedBuffer(buffer);   
      break;   
  }  

  if(debugMode > 0) {
    sendDataToSerial(commandLength, buffer);  
  }
  
  buffer[0] = 0;
}

void setup()
{
  Serial.begin(38400);
  Joystick.useManualSend(true);
  
  for (byte x = 0; x < ENABLED_MATRIX_COLUMNS; x++) {
    pinMode(columnPins[x], INPUT_PULLUP);           // set pin to input    
  }

  tm1637.set(5); 
  tm1637.init();
  tm1637.clearDisplay();

  // WS2812B INIT
  if (WS2812B_RGBLEDCOUNT > 0) {
    WS2812B_strip.setBrightness(8);
    WS2812B_strip.begin();    
    WS2812B_strip.show();
  }
}

void loop() {  
  buttonNumber = 1;
  sendMatrixState();
  senKnobState();
  Joystick.X(1023 - analogRead(A1));
  Joystick.Y(1023 - analogRead(A2));
  Joystick.Z(analogRead(A3));
  Joystick.Zrotate(1023 - analogRead(A4));
  processData();
  sendToWS2812B();  
  sendHandshacking();
}
