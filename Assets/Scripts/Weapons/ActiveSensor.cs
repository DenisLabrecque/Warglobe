public abstract class ActiveSensor : Sensor, ISwitchable
{
   #region Public Methods

   public abstract void Switch();

   public abstract void Switch(bool onOff);

   #endregion

   #region Switchable Interface

   public string Name => "Sensor"; // TODO return radar or sonar or whatever

   public bool IsOnOrSelected => _isOn;

   public string Keystroke => "R";

   public Group Group => Group.Accessories;

   #endregion
}