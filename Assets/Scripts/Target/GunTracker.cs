using System;
using UnityEngine;
using Warglobe;

public class GunTracker : MonoBehaviour
{
   Vector3 _position;
   Turret _turret;

   #region Properties
   
   /// <summary>
   /// Update this to set the spot where the tracker should be.
   /// </summary>
   public Vector3 Position { set {
         _position = value;
      }
   }

   #endregion

   private void Update()
   {
      gameObject.transform.position = SingleCamera.Camera1.WorldToScreenPoint(_turret.PointingAt);
   }

   internal void SetTurret(Turret turret)
   {
      if (turret != null)
         _turret = turret;
      else
         Debug.LogError("Turret was assigned as null to the tracker");
   }
}
