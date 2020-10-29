using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SensorSystem : MonoBehaviour
{
   #region Member Variables

   [Header("The sensor setting at start")]
   [SerializeField] bool _activeSensorsOn = false;

   List<Sensor> _sensors = new List<Sensor>();
   SortedSet<Target> _fusedSensorData;
   List<ActiveSensor> _activeSensors;
   Target _parentTarget; // Reference to the controling vehicle
   Target _currentTarget = null; // The target currently selected to fire at
   int _trackingIndex = 0; // The list target item we are looking at

   #endregion


   #region Properties

   /// <summary>
   /// Get all targets that the weapon system is tracking at the moment.
   /// Updated at each fixed update. Returns null if there are no targets in sight.
   /// </summary>
   public SortedSet<Target> FusedSensorData {
      get {
         return _fusedSensorData;
      }
   }

   /// <summary>
   /// Gets active sensors attached to this system.
   /// </summary>
   protected List<ActiveSensor> ActiveSensors {
      get { return _activeSensors; }
   }

   /// <summary>
   /// Returns whether the active sensors are on or off.
   /// </summary>
   public bool ActiveSensorsOn {
      get { return _activeSensorsOn; }
   }

   /// <summary>
   /// Get the target being followed as the selected target.
   /// Returns null if there are no targets (eg. all sensors are off).
   /// </summary>
   public Target TrackingTarget {
      get {
         if (_fusedSensorData.Count > 0)
            return _fusedSensorData.ElementAt(_trackingIndex); // out of range because the list is being changed, perhaps?
         else
            return null;
      }
   }

   /// <summary>
   /// The number of targets currently seen by every sensor in this sensor system.
   /// </summary>
   public int TargetCount {
      get { return _fusedSensorData.Count; }
   }

   #endregion


   #region Unity Methods

   void Awake()
   {
      _sensors = GetComponentsInChildren<Sensor>().ToList();

      _activeSensors = GetComponentsInChildren<ActiveSensor>().ToList();
      if (_activeSensors == null)
         Debug.LogWarning("No active sensors found for " + gameObject);

      _parentTarget = GetComponentInParent<Target>();
      if (_parentTarget == null)
         Debug.LogError("Target was not found for " + gameObject);

      _fusedSensorData = new SortedSet<Target>(new BySeekerDistance(_parentTarget));
   }

   private void Start()
   {
      foreach (ActiveSensor sensor in _activeSensors)
         sensor.Switch(_activeSensorsOn);
   }

   void FixedUpdate()
   {
      // Re-order the targets until a selection is made (or a target becomes visible, in which case the list is empty)
      //if (_currentTarget == null)
         FuseSensorData();
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Change the current target to be the next closest target.
   /// </summary>
   /// <param name="increment">How many targets down the list to fetch (back or forwards)</param>
   /// <returns>A target to be tracked</returns>
   public void NextTarget()
   {
      if (_currentTarget == null)
         EnterTrackingTarget();
      else
      {
         _trackingIndex++;
      }
   }

   /// <summary>
   /// Set the current target as being the current tracking target.
   /// </summary>
   /// <returns>The selected target</returns>
   public void EnterTrackingTarget()
   {
      _currentTarget = _fusedSensorData.ElementAt(_trackingIndex);
      _trackingIndex = 0; // Reset to the nearest target
   }

   /// <summary>
   /// Switch all active sensors on/off based on their current setting.
   /// </summary>
   public void SwitchActiveSensors()
   {
      _activeSensorsOn = !_activeSensorsOn;
      foreach (ActiveSensor sensor in _activeSensors)
         sensor.Switch(_activeSensorsOn);
   }

   #endregion


   #region Private Methods

   /// <summary>
   /// Run each update.
   /// Fetch targets from each sensor and update the fused target list.
   /// </summary>
   private void FuseSensorData()
   {
      // Dump the previous frame's list
      _fusedSensorData.Clear();

      // Go through each sensor on the vehicle
      foreach (Sensor sensor in _sensors)
      {
         if (sensor.IsOn && sensor.TargetList != null)
         {
            // Go through each sensed target
            foreach (Target target in sensor.TargetList)
            {
               if (target != UserInput.Player1Vehicle) // We already know that our vehicle exists, so don't add it to the target list
                  _fusedSensorData.Add(target);
            }
         }
      }
   }

   #endregion
}