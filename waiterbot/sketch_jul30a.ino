#include <Arduino.h>
#include <Wire.h>
#include <SoftwareSerial.h>
#include <MeMCore.h>

#define HAND_UP 10
#define HAND_MID 90
#define HAND_DOWN 180
#define INTERVAL 100
int t;
int press;
double angle_rad = PI/180.0;
double angle_deg = 180.0/PI;
String readString;
String action;
Servo servo_4_2;
Servo servo_4_1;
MePort port_4(4);
MeLimitSwitch sw_1_1(1,1);
MeBuzzer buzzer;
MBotDCMotor motor(0);
MeLineFollower linefollower_2(2); 
MeUltrasonicSensor ultrasonic_3(3);
 //建立一個軟體模擬的序列埠; 不要接反了!

SoftwareSerial myBlueTooth(0,1); 
char BluetoothData; 

void setup() {
      myBlueTooth.begin(115200); 
      MeBuzzer buzzer;
      t = 0;
      action = "";
      servo_4_1.attach(port_4.pin1());
      servo_4_2.attach(port_4.pin2());
      servo_4_1.write(HAND_MID);
      servo_4_2.write(HAND_MID);
      pinMode(A7,INPUT);
      press=0;
}

 

void loop() { 
   if(sw_1_1.touched()){
         if(press == 0)
                {
                     myBlueTooth.write("Game_Confirm");
                      buzzer.tone(262, 500);
                     press = 1;             
                }
            }
           else{
                press = 0;
           }
     readString="";   
      if(myBlueTooth.available() > 0)
      {
          readString = myBlueTooth.readString();
      }            
      if(readString !=""){
         /* buzzer.tone(262, 1000);*/
          t = 0;
          action = readString;
      }
      punish();
      forward();
      backward();
      right();
      left();
      right_up();
      hands_down();
      hands_up();
      move_hands();
      shake_head();
      turnaround_move();
      dance();
      t++;
      delay(INTERVAL);
}
void punish(){   
    if(action =="punish"){
      if(t==1){
         servo_4_1.write(180);
      }
      else if(t==5){
        servo_4_1.write(90);
        action = "";
      }
  }
}
  
void forward(){   //向前
    if(action =="forward"){
      if(t==1){
        motor.move(1,100);
      }
      else if(t==5){
        motor.move(0,0);
        action = "";
      }
    }
  }
  
void backward(){   //向後
    if(action =="backward"){
      if(t==1){
        motor.move(2,100);
      }
      else if(t==5){
        motor.move(0,0);
        action = "";
      }
    }
  }
void left(){   //向左
    if(action =="left"){
      if(t==1){
        motor.move(3,100);
      }
      else if(t==5){
        motor.move(0,0);
        action = "";
      }
    }
  }
void right(){   //向右
    if(action =="right"){
      if(t==1){
        motor.move(4,100);
      }
      else if(t==5){
        motor.move(0,0);
        action = "";
      }
    }
  }
void right_up(){
  if(action == "right_hand_up"){
    if(t==1){  
     servo_4_1.write(HAND_UP);
    }
    else if(t==4){
       servo_4_1.write(HAND_MID);
    }
    else if(t==8){
       servo_4_1.write(HAND_UP);   
    }
    else if(t==12){
        servo_4_1.write(HAND_MID);  
    }else if(t==16){
        action = "";
    }
  }
}
void hands_down(){
    if(action == "hands_down"){
        if(t==1){  
             servo_4_1.write(HAND_DOWN);
             servo_4_2.write(HAND_DOWN);
            }
            else if(t==5){
               servo_4_1.write(HAND_MID);
               servo_4_2.write(HAND_MID);
            }
            else if(t==10){
               servo_4_1.write(HAND_DOWN); 
               servo_4_2.write(HAND_DOWN);  
            }
            else if(t==15){
                servo_4_1.write(HAND_MID);  
                servo_4_2.write(HAND_MID);
            }
           
            else if(t==20){
              action = "";
            } 
    }
}

void hands_up(){
    if(action == "hands_up"){
        if(t==1){  
             servo_4_1.write(HAND_UP);
             servo_4_2.write(HAND_UP);
            }
            else if(t==5){
               servo_4_1.write(HAND_MID);
               servo_4_2.write(HAND_MID);
            }
            else if(t==10){
               servo_4_1.write(HAND_UP); 
               servo_4_2.write(HAND_UP);  
            }
            else if(t==15){
                servo_4_1.write(HAND_MID);  
                servo_4_2.write(HAND_MID);
            }
           
            else if(t==20){
              action = "";
            } 
    }
}

void move_hands(){
   if(action == "move_hands"){
          if(t==1){  
           motor.move(1,100);
          }
          else if(t==5){
            motor.move(2,100);
          }
          else if(t==10){
             motor.move(1,100);
          }
          else if(t==15){
              motor.move(2,100);
          }
          else if(t==20){  
            motor.move(0,0);
           servo_4_1.write(HAND_UP);
           servo_4_2.write(HAND_UP);
          }
          else if(t==25){
             servo_4_1.write(HAND_MID);
             servo_4_2.write(HAND_MID);
          }
          else if(t==30){
             servo_4_1.write(HAND_UP); 
             servo_4_2.write(HAND_UP);  
          }
          else if(t==35){
              servo_4_1.write(HAND_MID);  
              servo_4_2.write(HAND_MID);
          }
         
          else if(t==40){
            action = "";
          } 
    }
}
void shake_head(){
    if(action == "shake_head"){
        if(t==1){  
         motor.move(3,100);
        }
        else if(t==5){
          motor.move(4,100);
        }
        else if(t==10){
           motor.move(3,100);
        }
        else if(t==15){
            motor.move(4,100);
        }   
        else if(t==20){
          action = "";
          motor.move(0,0);
        } 
    }
}
void turnaround_move(){
    if(action == "turnaround_move"){
        if(t==1){  
            motor.move(4,100);
        }
        else if(t==20){
            motor.move(1,100);
        }
        else if(t==25){
            motor.move(2,100);
        }
        else if(t==30){
            motor.move(1,100);
        }
        else if(t==35){  
            motor.move(2,100);
        }
        else if(t==40){  
          action = ""; 
          motor.move(0,0); 
        } 
    }
}

void dance(){
   if(action == "dance"){
        if(t==1){  
          motor.move(1,100);
        }
        else if(t==4){
          motor.move(2,100);
        }
        else if(t==8){
          servo_4_1.write(HAND_MID);
          servo_4_2.write(HAND_MID);
           motor.move(1,100);
        }
        else if(t==12){
           servo_4_1.write(HAND_UP);
           servo_4_2.write(HAND_UP);
            motor.move(2,100);
        }
        else if(t==16){
            motor.move(4,100);
        }
        else if(t==20){
            motor.move(3,100);
        }
        else if(t==24){  
            motor.move(3,150);
        }
        else if(t==44){
            motor.move(4,150);
        }
        else if(t==64){  
          servo_4_1.write(HAND_MID);
          servo_4_2.write(HAND_MID);
          motor.move(0,200);
        }
        else if(t==68){
          servo_4_1.write(HAND_UP);
          servo_4_2.write(HAND_UP);
        }
        else if(t==72){  
          servo_4_1.write(HAND_MID);
          servo_4_2.write(HAND_MID);
        }
        else if(t==76){
          servo_4_1.write(HAND_UP);
          servo_4_2.write(HAND_UP);
        }
        else if(t==80){
           action = ""; 
        } 
   }
}
