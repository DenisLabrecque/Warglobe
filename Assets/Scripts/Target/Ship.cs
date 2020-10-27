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


   #endregion


   #region Unity Methods

   /// <summary>
   /// Detect missing components.
   /// </summary>
   void Start()
   {
      base.Start();

      if (_rudders == null || _rudders.Count == 0)
         Debug.LogWarning("A ship is expected to have a rudder!");
   }

   void Update()
   {
      base.Update();

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
      Gravity.Gravitate(_Rigidbody);
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
      _motor.PercentThrottle = UserInput.Throttle;

      // Steer
      foreach(var rudder in _rudders)
         if(rudder != null)
            rudder.DeflectionPercent = UserInput.Yaw;

      // Fire weapons
      if(UserInput.Gun)
         WeaponSystem.FireTurrets();
   }

   #endregion
}
