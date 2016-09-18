#include "Arduino.h"
#include "limits.h"
#include <Adafruit_PWMServoDriver.h>
//#include <SoftwareSerial.h>
#include <SerialCommand.h>
#include <Wire.h>

Adafruit_PWMServoDriver pwm = Adafruit_PWMServoDriver();
SerialCommand sCmd;

typedef struct {
  unsigned long expireTime;
  int port;
} buzz_queue_element;

buzz_queue_element queue[1024] = {ULONG_MAX, -1};
int queueLength = 0;

bool demoEnabled = false;

void buzzHandler() {
  char *sPort = sCmd.next();
  char *sStrength = sCmd.next();
  char *sDuration = sCmd.next();
  if (sPort && sStrength && sDuration) {
    int port = atoi(sPort) % 16;
    int strength = (atoi(sStrength) * 4095 / 100);
    if(strength > 4095) strength = 4095;
    unsigned long expireTime = atoi(sDuration) + millis();

    bool foundSlot = false;
    for(int i = 0; i < queueLength; ++i) {
      if(queue[i].port == -1) {
        queue[i].expireTime = expireTime;
        queue[i].port = port;

        foundSlot = true;
      }
    }

    if(!foundSlot) {
      queue[queueLength].expireTime = expireTime;
      queue[queueLength].port = port;
      ++queueLength;
    }

    pwm.setPWM(port, 0, 4000);
    //Serial.println("nothing to echo");
  } else {
    Serial.println("nothing to echo");
  }
}

void setup() {
  Serial.begin(9600);
  Serial.println("16 channel PWM test!");
  #ifdef ESP8266
  Wire.pins(2, 14);   // ESP8266 can use any two pins, such as SDA to #2 and SCL to #14
  #endif

  pwm.begin();
  pwm.setPWMFreq(1600);  // This is the maximum PWM frequency

  // if you want to really speed stuff up, you can go into 'fast 400khz I2C' mode
  // some i2c devices dont like this so much so if you're sharing the bus, watch
  // out for this!
#ifdef TWBR
  // save I2C bitrate
  //uint8_t twbrbackup = TWBR;
  // must be changed after calling Wire.begin() (inside pwm.begin())
  TWBR = 12; // upgrade to 400KHz!
#endif
  // initialize LED digital pin as an output.
  pinMode(LED_BUILTIN, OUTPUT);
  //pinMode(0, INPUT);

  for (uint8_t pwmnum=0; pwmnum < 16; pwmnum++) {
    pwm.setPWM(pwmnum, 0, 0);
  }

  sCmd.addCommand("BUZZ", buzzHandler);
}

void loop()
{
  /*if(digitalRead(0) == HIGH) {
    demoEnabled = !demoEnabled;

    if(!demoEnabled) {
      for (uint8_t pwmnum=0; pwmnum < 16; pwmnum++) {
        pwm.setPWM(pwmnum, 0, 0);
      }
    }
    delay(1000);
  }
  if(demoEnabled) {
    for (uint16_t i=0; i<4096; i += 8    ) {
        #ifdef ESP8266
        yield();
        #endif
        for (uint8_t pwmnum=0; pwmnum < 16; pwmnum++) {
          pwm.setPWM(pwmnum, 0, (i + (4096/16)*pwmnum) % 4096 );
        }
    }
  }*/

  unsigned long currentTime = millis();

  for(int i = 0; i < queueLength; ++i) {
    if(queue[i].port >= 0 && queue[i].expireTime <= currentTime) {
      pwm.setPWM(queue[i].port, 0, 0);
      queue[i].expireTime = ULONG_MAX;
      queue[i].port = -1;
    }
  }
  if (Serial.available() > 0)
    sCmd.readSerial();
}
