using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPropeller : Motor
{
   [SerializeField]
   private float _backwardsEfficiency = 0.4f;
   private Vehicle _vehicle;

   /// <summary>
   /// The propeller is less efficient going backwards.
   /// </summary>
   private float Multiplier {
      get {
         if (_vehicle.ForwardSpeed < 0) // Backwards
            return _backwardsEfficiency;
         else
            return 1.0f;
      }
   }

   void Start()
   {
      _vehicle = GetComponentInParent<Vehicle>();
   }

   void FixedUpdate()
   {
      if (IsEnabled && Planet.Singleton.IsInWater(gameObject))
      {
         // Apply the throttle behind a vehicle
         _rigidbody.AddRelativeForce(_thrustDirection * Multiplier * Thrust);
      }
   }
}
