using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Vehicle
{
   public enum ShipType
   {
      AircraftCarrier, Battleship, Cruiser, Destroyer, Cutter
   }

   #region Member Variables

   [Header("Control surface")]
   [SerializeField] List<ControlSurface> _rudders;

   [Header("Ship subtype")]
   [SerializeField] public ShipType _type = ShipType.Cruiser;

   private bool m_HasFired = false; // Prevent a single click to count as multiple

   #endregion


   #region Unity Methods

   /// <summary>
   /// Detect missing components.
   /// </summary>
   void Start()
   {
      base.Start();

      if (_rudders == null || _rudders.Count == 0)
         Debug.LogError("A ship must have a rudder");
   }

   void Update()
   {
      // Steer through user input
      if (UserInput.Player1Vehicle == this)
      {
         HumanControl();
      }
      // Steer through AI
      else
      {
         AIControl();
      }
   }

   /// <summary>
   /// Apply physics.
   /// </summary>
   void FixedUpdate()
   {
      Gravity.Gravitate(m_Rigidbody);
      AdjustDrag();
   }

   #endregion


   #region Methods

   private void AIControl()
   {
      //throw new NotImplementedException();
   }

   private void HumanControl()
   {
      // Throttle up the engines
      if (UserInput.Throttle != 0)
         m_Motor.AdjustThrottle(UserInput.Throttle);

      // Steer
      foreach(var rudder in _rudders)
         rudder.DeflectionPercent = UserInput.Yaw;

      // Fire weapons
      if(UserInput.Gun)
      {
         if (m_HasFired == false)
         {
            WeaponSystem.FireTurret();
            m_HasFired = true;
         }
      }
      else
      {
         m_HasFired = false;
      }
   }

   #endregion
}
