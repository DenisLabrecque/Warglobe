using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SensorSystem : MonoBehaviour
{
   #region Member Variables

   List<Sensor> m_Sensors = new List<Sensor>();
   SortedSet<Target> m_FusedSensorData;
   Radar m_Radar;
   Target m_ParentTarget; // Reference to the controling vehicle
   Target m_CurrentTarget = null; // The target currently selected to fire at
   int m_TrackingIndex = 0; // The list target item we are looking at

   #endregion


   #region Properties

   /// <summary>
   /// Get all targets that the weapon system is tracking at the moment.
   /// Updated at each fixed update.
   /// MAY RETURN AN EMPTY SET IF THE SYSTEM IS OFF OR IF NO TARGETS ARE IN SIGHT.
   /// </summary>
   public SortedSet<Target> FusedSensorData {
      get {
         return m_FusedSensorData;
      }
   }

   /// <summary>
   /// Get the radar. Returns null if there is no radar.
   /// </summary>
   public Radar Radar {
      get { return m_Radar; }
   }

   /// <summary>
   /// Get the target being followed as the selected target.
   /// Returns null if there are no targets (eg. all sensors are off).
   /// </summary>
   public Target TrackingTarget {
      get {
         if(m_FusedSensorData.Count > 0)
            return m_FusedSensorData.ElementAt(m_TrackingIndex); // out of range because the list is being changed, perhaps?
         else
            return null;
      }
   }

   /// <summary>
   /// The number of targets currently seen by every sensor in this sensor system.
   /// </summary>
   public int TargetCount {
      get { return m_FusedSensorData.Count; }
   }

   #endregion


   #region Unity Methods

   void Awake()
   {
      m_Sensors = GetComponentsInChildren<Sensor>().ToList();

      if(GetComponentInChildren<Radar>() != null)
         m_Radar = GetComponentInChildren<Radar>();
      else
         m_Radar = null;

      m_ParentTarget = GetComponentInParent<Target>();
      if(m_ParentTarget == null)
         Debug.LogError("Target was not found for " + gameObject);

      m_FusedSensorData = new SortedSet<Target>(new BySeekerDistance(m_ParentTarget));
   }

   void FixedUpdate()
   {
      // Re-order the targets until a selection is made (or a target becomes visible, in which case the list is empty)
      if(m_CurrentTarget == null)
         FuseSensorData();
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Change the current target to be the next closest target.
   /// </summary>
   /// <param name="increment">How many targets down the list to fetch (back or forwards)</param>
   /// <returns>A target to be tracked</returns>
   public void NextTarget() {
      if(m_CurrentTarget == null)
         EnterTrackingTarget();
      else
      {
         m_TrackingIndex++;
      }
   }

   /// <summary>
   /// Set the current target as being the current tracking target.
   /// </summary>
   /// <returns>The selected target</returns>
   public void EnterTrackingTarget()
   {
      m_CurrentTarget = m_FusedSensorData.ElementAt(m_TrackingIndex);
      m_TrackingIndex = 0; // Reset to the nearest target
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
      m_FusedSensorData.Clear();

      // Go through each sensor on the vehicle
      foreach(Sensor sensor in m_Sensors)
      {
         if(sensor.IsOn && sensor.TargetList != null)
         {
            // Go through each sensed target
            foreach(Target target in sensor.TargetList)
            {
               if(target != (UserInput.CurrentVehicle as Target)) // We already know that our vehicle exists, so don't add it to the target list
                  m_FusedSensorData.Add(target);
            }
         }
      }
   }
   
   #endregion
}
