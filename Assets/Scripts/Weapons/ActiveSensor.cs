public abstract class ActiveSensor : Sensor
{
   #region Public Methods

   public abstract void Switch();

   public abstract void Switch(bool onOff);

   #endregion
}