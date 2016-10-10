#include <Arduino.h>
#include <Wire.h>
#include <Servo.h>
#include <SoftwareSerial.h>

#include <MeMCore.h>

double angle_rad = PI/180.0;
double angle_deg = 180.0/PI;
MeDCMotor leftWheel(9);
MeDCMotor rightWheel(10);
int leftSpeed = 0;
int rightSpeed = 0;

MeBuzzer buzzer;

int press = 0;
String readString;
char tmp;
SoftwareSerial myBlueTooth(0,1); 
void setup(){
    myBlueTooth.begin(115200); 
    pinMode(A7,INPUT);
}

void loop(){
    readString="";
    if((0^(analogRead(A7)>10?0:1))){
          if(press ==0){
               buzzer.tone(262, 500);
               myBlueTooth.write("hello");
               press = 1;
          }
     }else{
        press = 0;
     } 
    
    while(myBlueTooth.available() > 0)
    {
        tmp = myBlueTooth.read();
        readString.concat(tmp);
        if(tmp == ';'){
            break;
        }
    }

    spliteCommand(readString);
    fixMove();
    delay(100);
}

void fixMove(){
    leftWheel.run((9)==M1?-(leftSpeed):(leftSpeed));
    rightWheel.run((10)==M1?-(rightSpeed):(rightSpeed));
}

void spliteCommand(String data){
    String cmd;
    int cmdIndex = 0;
    cmd = getValue(data, ';', cmdIndex);
    while(cmd != ""){
        execCommand(cmd);
        cmdIndex ++;
        cmd = getValue(data, ';', cmdIndex);
    }  
}

void execCommand(String cmd){
    String action =  getValue(cmd, ' ', 0);
    if(action == "wheel"){
        setWheel(getValue(cmd, ' ', 1).toInt(),  getValue(cmd, ' ', 2).toInt());
    }
}

void setWheel(int left, int right){
    leftSpeed = left;
    rightSpeed = right;
}


String getValue(String data, char separator, int index)
{
    int found = 0;
    int strIndex[] = {0, -1};
    int maxIndex = data.length()-1;
  
    for(int i=0; i <= maxIndex && found<=index; i++){
        if(data.charAt(i)==separator || i==maxIndex){
            found++;
            strIndex[0] = strIndex[1]+1;
            strIndex[1] = (i == maxIndex) ? i+1 : i;
        }
    }
    return found>index ? data.substring(strIndex[0], strIndex[1]) : "";
}
