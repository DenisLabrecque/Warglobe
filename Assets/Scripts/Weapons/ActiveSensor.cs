using UnityEngine;
using Warglobe.Assets;

namespace Warglobe
{
   /// <summary>
   /// Active sensors cause a detectable signal that can be passively detected.
   /// Game logic should take this into account.
   /// 
   /// Denis Labrecque, October 2020
   /// </summary>
   public abstract class ActiveSensor : Sensor, ISwitchable
   {
      [Header("Switchable")]
      [SerializeField] protected Switchable _switchable;

      #region Public Methods

      public abstract void Switch();

      public abstract void Switch(bool onOff);

      #endregion

      #region Switchable Interface

      public bool IsOnOrSelected => _isOn;

      public Switchable Switchable => _switchable;

      #endregion
   }
}