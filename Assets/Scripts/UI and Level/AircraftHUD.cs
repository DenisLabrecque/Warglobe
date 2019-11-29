using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

/// <summary>
/// Control what is seen on an aircraft's HUD, referencing all parts of an aircraft HUD and modifying values according to the currently selected airplane.
/// </summary>
public class AircraftHUD : MonoBehaviour {

   #region Member Variables
   
   static List<HUDTracker> m_HUDTrackers = new List<HUDTracker>();
   static List<Waypoint> m_Waypoints = new List<Waypoint>();

   // HUD Elements
   [SerializeField] Slider m_ThrottleSlider;
   [SerializeField] TextMeshProUGUI m_AlphaText;
   [SerializeField] TextMeshProUGUI m_MachText;
   [SerializeField] TextMeshProUGUI m_AirspeedText;
   [SerializeField] TextMeshProUGUI m_AltitudeText;
   [SerializeField] TextMeshProUGUI m_RadarIOText;
   [SerializeField] TextMeshProUGUI m_SelectedWeaponText;
   [SerializeField] TextMeshProUGUI m_MessageText;
   [SerializeField] TextMeshProUGUI m_TestingText;

   [SerializeField] HUDTracker m_HUDTracker;
   [SerializeField] Image m_HeadingMarker;

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
      foreach(Target target in sceneTargets)
      {
         HUDTracker tracker = Instantiate(m_HUDTracker, gameObject.transform);
         tracker.SetTarget(target);
         m_HUDTrackers.Add(tracker);
      }

      // Find all scene waypoints
      m_Waypoints = FindObjectsOfType<Waypoint>().ToList();

      // Go through each waypoint in the scene and make a tracker for each
      foreach(Waypoint waypoint in m_Waypoints)
      {
         HUDTracker tracker = Instantiate(m_HUDTracker, gameObject.transform);
         tracker.SetWaypoint(waypoint);

      }
   }

   /// <summary>
   /// Print vehicle-related info.
   /// </summary>
   void Update()
   {
      PrintVehicleHUDInfo();
      ShowFPM();
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
      // Math the vehicle's  AOA
      Vehicle vehicle = UserInput.Player1Vehicle;
      m_HeadingMarker.transform.position = Camera.main.WorldToScreenPoint(vehicle.transform.position + (vehicle.NormalizedVelocity * 900f));

      // Match the vehicle's heading
      //m_HeadingMarker.transform.position = Camera.main.WorldToScreenPoint(vehicle.transform.position + (vehicle.transform.forward.normalized * 500f));

      // Match the earth's surface rotation
      m_HeadingMarker.transform.eulerAngles = (Vector3.forward * vehicle.RollAngle);
   }

   /// <summary>
   /// Track targets as seen by the vehicle.
   /// Sensor data is fused at every fixed update.
   /// </summary>
   private void TrackTargets()
   {
      // Manage every tracker depending on target visibility to sensors
      foreach(HUDTracker tracker in m_HUDTrackers)
      {
         // Only show the player target trackers when his vehicle is actually tracking them
         if(UserInput.Player1Vehicle.SensorSystem.FusedSensorData.Contains(tracker.Target))
            tracker.IsVisible = true;
         else
            tracker.IsVisible = false;

         // Show the player lock-on symbology
         if(UserInput.Player1Vehicle.WeaponSystem.SensorSystem.TrackingTarget == tracker.Target)
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
      Vehicle currentVehicle = UserInput.Player1Vehicle as Vehicle;

      // Throttle value
      m_ThrottleSlider.value = currentVehicle.Throttle;

      // Battery
      m_MachText.text = currentVehicle.BatteryAmperage.ToString();

      // Airspeed
      m_AirspeedText.text = (currentVehicle.ForwardSpeed).ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');



      // Radar on/off
      m_RadarIOText.text = currentVehicle.SensorSystem.Radar.IsOn ? Multilang.Text["radar_on"] : Multilang.Text["radar_off"];

      // Selected weapon
      m_SelectedWeaponText.text = currentVehicle.WeaponSystem.CurrentProjectile.Abbreviation + " " + currentVehicle.WeaponSystem.CurrentSlot.NumberLeft;



      // Testing sensor output
      TestingSensorList();

      if(currentVehicle is Airplane)
      {
         PrintAirplaneHUDInfo(currentVehicle as Airplane);
      }
      else if(currentVehicle is Ship)
      {
         PrintShipHUDInfo(currentVehicle as Ship);
      }
   }

   private void PrintAirplaneHUDInfo(Airplane currentAirplane)
   {
      // Angle
      m_AlphaText.text = currentAirplane.PitchAngle.ToString("N1");

      // Altitude
      string altitudeString;
      if (currentAirplane.SensorSystem.Radar.IsOn)
         altitudeString = currentAirplane.SensorSystem.Radar.Altitude.ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');
      else
         altitudeString = currentAirplane.AltitudeAboveSea.ToString("N0", CultureInfo.InvariantCulture).Replace(',', ' ');
      m_AltitudeText.text = altitudeString;

      // Message area
      string action;
      if (currentAirplane.SensorSystem.TrackingTarget == null)
         action = "null";
      else
         action = currentAirplane.SensorSystem.TrackingTarget.PopularName;
      string state = currentAirplane.IsUpright ? "upright" : "upside-down";
      m_MessageText.text = action + "\n" + state;
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
      if(currentVehicle.SensorSystem != null)
      {
         foreach(Target target in currentVehicle.SensorSystem.FusedSensorData)
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