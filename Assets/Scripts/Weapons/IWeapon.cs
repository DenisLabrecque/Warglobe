﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that all sensors and launchers are based upon. This decides when a weapon is fired.
/// </summary>
public interface IWeapon {

   #region Properties

   /// <summary>
   /// Main property that tells the weapon system it can shoot within launch parameters.
   /// </summary>
   bool LaunchAuthority { get; }

   /// <summary>
   /// Lets the AI know the approximate chances of killing another vehicle in this situation.
   /// </summary>
   float HitProbability { get; }


   #endregion


   #region Methods

   /// <summary>
   /// Main method for shooting the weapon.
   /// </summary>
   void Fire();

   #endregion

}
