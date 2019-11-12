using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleCommand : MonoBehaviour
{

   #region Member Variables

   // Steering
   [Range(-1,1)] float m_Throttle;
   [Range(-1,1)] float m_Bank;
   [Range(-1,1)] float m_Pitch;
   [Range(-1,1)] float m_Yaw;

   // Commands
   bool m_FireWeapon;
   bool m_CycleWeapon;
   bool m_GearUp;

   #endregion


   #region Constructor 

   VehicleCommand(float throttle, float bank, float pitch, float yaw)
   {
      m_Throttle = throttle;
      m_Bank = bank;
      m_Pitch = pitch;
      m_Yaw = yaw;
   }

   #endregion


   #region Properties



   #endregion
}
