using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPropeller : Motor
{
   void FixedUpdate()
   {
      if (IsEnabled && Planet.Singleton.IsInWater(gameObject))
      {
         // Apply the throttle behind a vehicle
         m_Rigidbody.AddRelativeForce(m_ThrustDirection * CurrentThrust);
      }
   }
}
