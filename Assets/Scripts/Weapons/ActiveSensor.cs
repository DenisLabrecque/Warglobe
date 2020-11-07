using UnityEngine;
using UnityEngine.UI;

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
      #region Member Variables

      [Header("Switchable")]
      [SerializeField] string _name = "Radars";
      [SerializeField] Sprite _hudIcon;

      #endregion

      #region Public Methods

      public abstract void Switch();

      public abstract void Switch(bool onOff);

      #endregion

      #region Switchable Interface

      public Sprite HudIcon { get => _hudIcon; }

      public string Name => _name;

      public bool IsOnOrSelected => _isOn;

      public string Keystroke => "R";

      public Group Group => Group.Accessories;

      public Function Function => Function.Radar;

      #endregion
   }
}