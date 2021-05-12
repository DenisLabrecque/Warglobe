using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using System.Text;

namespace Warglobe.Hud
{
   /// <summary>
   /// Control what is seen on a vehicle's HUD, referencing all parts of the HUD and modifying values according to the currently selected vehicle.
   /// </summary>
   public class MultiHUD : MonoBehaviour
   {

      #region Member Variables

      static List<TargetTracker> _hudTrackers = new List<TargetTracker>();
      static List<Waypoint> _waypoints = new List<Waypoint>();

      // HUD Elements
      [Header("Waypoint")]
      [SerializeField] TextMeshProUGUI _waypointText;
      [SerializeField] Slider _waypointSlider;

      [Header("Power")]
      [SerializeField] TextMeshProUGUI _speed;
      [SerializeField] Slider _thrustSlider;
      [SerializeField] TextMeshProUGUI _throttle;
      [SerializeField] Slider _throttleSlider;

      [Header("Energy")]
      [SerializeField] TextMeshProUGUI _energy;
      [SerializeField] TextMeshProUGUI _energyPercent;
      [SerializeField] Slider _energySlider;

      [Header("Health")]
      [SerializeField] TextMeshProUGUI _health;
      [SerializeField] TextMeshProUGUI _healthPercent;
      [SerializeField] Slider _healthSlider;

      [Header("Testing")]
      [SerializeField] TextMeshProUGUI m_TestingText;
      [SerializeField] SwitchableIcon _toggleWeaponInfo;
      [SerializeField] Transform _bottomPanel;

      [Header("Tracker")]
      [SerializeField] TargetTracker _targetTracker;
      [SerializeField] Image _headingMarker;
      [SerializeField] GunTracker _gunTracker;

      #endregion


      #region Unity Methods

      /// <summary>
      /// Create a tracker for each target in the scene.
      /// </summary>
      void Start()
      {
         // Find all scene targets and waypoints
         List<Target> sceneTargets = FindObjectsOfType<Target>().ToList();

         // Go through each target in the scene and make a tracker for each one
         // Add the tracker to the list of trackers
         foreach (Target target in sceneTargets)
         {
            TargetTracker tracker = Instantiate(_targetTracker, gameObject.transform);
            tracker.SetTarget(target);
            _hudTrackers.Add(tracker);
         }

         // Go through each gun in the scene and make a tracker for each one
         // Add the gun tracker to the list of trackers
         foreach(Turret turret in UserInput.Player1Vehicle.WeaponSystem.Turrets) {
            GunTracker tracker = Instantiate(_gunTracker, gameObject.transform);
            tracker.SetTurret(turret);
         }

         foreach (var keyValue in UserInput.Player1Vehicle.SwitchablesByFunction)
         {
            SwitchableIcon weaponInfo = Instantiate(_toggleWeaponInfo, _bottomPanel);
            weaponInfo.Initialize(keyValue.Value.FirstOrDefault());
         }
      }

      /// <summary>
      /// Print vehicle-related info.
      /// </summary>
      void Update()
      {
         PrintVehicleHUDInfo();

         if (UserInput.Player1Vehicle.GetType() == typeof(Airplane))
            ShowFPM();
         else if (UserInput.Player1Vehicle.GetType() == typeof(Ship))
            _headingMarker.enabled = false;
      }

      /// <summary>
      /// Update tracker information for sensed targets.
      /// </summary>
      void FixedUpdate()
      {
         TrackTargets();
      }

      #endregion


      #region Private Methods

      private void ShowFPM()
      {
         _headingMarker.enabled = true;

         // Math the vehicle's  AOA
         Vehicle vehicle = UserInput.Player1Vehicle;
         _headingMarker.transform.position = Camera.main.WorldToScreenPoint(vehicle.transform.position + (vehicle.NormalizedVelocity * 900f));

         // Match the vehicle's heading
         //m_HeadingMarker.transform.position = Camera.main.WorldToScreenPoint(vehicle.transform.position + (vehicle.transform.forward.normalized * 500f));

         // Match the earth's surface rotation
         _headingMarker.transform.eulerAngles = (Vector3.forward * vehicle.RollAngle);
      }

      /// <summary>
      /// Track targets as seen by the vehicle.
      /// Sensor data is fused at every fixed update.
      /// </summary>
      private void TrackTargets()
      {
         StringBuilder builder = new StringBuilder();
         foreach (Target target in UserInput.Player1Vehicle.SensorSystem.FusedSensorData)
         {
            builder.Append(target.ToString() + " ");
         }
         //m_MessageText.text = builder.ToString();

         // Manage every tracker depending on target visibility to sensors
         foreach (TargetTracker tracker in _hudTrackers)
         {
            // Only show the player target trackers when his vehicle is actually tracking them
            if (UserInput.Player1Vehicle.SensorSystem.FusedSensorData.Contains(tracker.Target))
               tracker.IsVisible = true;
            else
               tracker.IsVisible = false;

            // Show the player lock-on symbology
            if (UserInput.Player1Vehicle.WeaponSystem.SensorSystem.TrackingTarget == tracker.Target)
               tracker.IsLockedOn = true;
            else
               tracker.IsLockedOn = false;
         }
      }

      /// <summary>
      /// Print airplane HUD essential information like airspeed, throttle, and the like.
      /// </summary>
      private void PrintVehicleHUDInfo()
      {
         Vehicle currentVehicle = UserInput.Player1Vehicle;

         // TODO
         //m_SelectedWeaponText.text = currentVehicle.WeaponSystem.CurrentProjectile.Abbreviation + " " + currentVehicle.WeaponSystem.CurrentSlot.NumberLeft;


         // Waypoint
         _waypointText.text = "NONE";
         _waypointSlider.value = 0.5f;

         // Power
         _speed.text = (currentVehicle.ForwardSpeed).ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');
         _thrustSlider.value = Mathf.Abs(currentVehicle.Motor.PercentThrust);
         _throttle.text = (currentVehicle.Motor.PercentThrottle * 100f).ToString("N0", CultureInfo.InvariantCulture) + "%";
         _throttleSlider.value = Mathf.Abs(currentVehicle.Motor.PercentThrottle);

         // Energy
         _energy.text = (currentVehicle.Motor.BatteryMinutesLeft + 1).ToString();
         _energyPercent.text = (currentVehicle.PercentBattery * 100f).ToString("N0", CultureInfo.InvariantCulture) + "%";
         _energySlider.value = currentVehicle.PercentBattery;

         // Health
         _health.text = currentVehicle.Hitpoints.ToString();
         _healthPercent.text = (currentVehicle.HitpointPercent * 100f).ToString("N0", CultureInfo.InvariantCulture) + "%";
         _healthSlider.value = currentVehicle.HitpointPercent;


         // Testing sensor output
         TestingSensorList();

         if (currentVehicle is Airplane)
         {
            PrintAirplaneHUDInfo(currentVehicle as Airplane);
         }
         else if (currentVehicle is Ship)
         {
            PrintShipHUDInfo(currentVehicle as Ship);
         }
      }

      private void PrintAirplaneHUDInfo(Airplane currentAirplane)
      {
         // Angle
         //m_AlphaText.text = currentAirplane.PitchAngle.ToString("N1");

         // Altitude
         string altitudeString;
         //if (currentAirplane.SensorSystem.ActiveSensors.IsOn)
         //   altitudeString = currentAirplane.SensorSystem.ActiveSensors.Altitude.ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');
         //else
         altitudeString = currentAirplane.AltitudeAboveSea.ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');
         //m_AltitudeText.text = altitudeString;

         // Message area
         string action;
         if (currentAirplane.SensorSystem.TrackingTarget == null)
            action = "null";
         else
            action = currentAirplane.SensorSystem.TrackingTarget.PopularName;
         string state = currentAirplane.IsUpright ? "upright" : "upside-down";
         //m_MessageText.text = action + "\n" + state;
      }

      private void PrintShipHUDInfo(Ship currentShip)
      {
         //m_AlphaText.text = currentShip.rudder
      }

      /// <summary>
      /// Show a list of what targets the sensors are detecting.
      /// </summary>
      private void TestingSensorList()
      {
         Vehicle currentVehicle = UserInput.Player1Vehicle as Vehicle;
         string TargetList = string.Empty;
         if (currentVehicle.SensorSystem != null)
         {
            foreach (Target target in currentVehicle.SensorSystem.FusedSensorData)
            {
               TargetList += target + "\n";
            }
            m_TestingText.text = TargetList;
         }
         else
         {
            m_TestingText.text = "Null weapon system";
         }
      }

      #endregion
   }
}