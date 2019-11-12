using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleCarController : MonoBehaviour
{
   public List<AxleInfo> axleInfos; // the information about each individual axle
   public float maxMotorTorque; // maximum torque the motor can apply to wheel
   public float maxSteeringAngle; // maximum steer angle the wheel can have

   public void FixedUpdate()
   {
      float breaking = 1* Input.GetAxis("Jump");
      float motor = maxMotorTorque  * Input.GetAxis("Vertical");
      float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

      foreach(AxleInfo axleInfo in axleInfos)
      {
         if(axleInfo.steering)
         {
            axleInfo.leftWheel.steerAngle = steering;
            axleInfo.rightWheel.steerAngle = steering;
         }
         if(axleInfo.motor)
         {
            axleInfo.leftWheel.motorTorque = motor;
            axleInfo.rightWheel.motorTorque = motor;
         }
      }
   }
}

[System.Serializable]
public class AxleInfo
{
   public WheelCollider leftWheel;
   public WheelCollider rightWheel;
   public bool motor; // is this wheel attached to motor?
   public bool steering; // does this wheel apply steer angle?
}