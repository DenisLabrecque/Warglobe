using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Base class for sonars and radars.
/// A trigger sphere collider serves as maximum range.
/// The important method is ListPotentialTarget(); it should be overridden in any actual implementation to consider targets that are not only
/// within range, but also within acceptable parameters (ie. within LOS, radar on, loud enough, on the same team, etc.)
/// 
/// Denis Labrecque
/// December 2018
/// </summary>
public abstract class Sensor : MonoBehaviour {

   public static List<Target> m_SceneTargets = new List<Target>(); // All targets in the scene. When a new target is created, it must be added to this list.

   #region Member Variables

   protected SortedSet<Target> m_TargetList = new SortedSet<Target>(); // Things tracked by the sensor
   [SerializeField] protected bool m_IsOn = true;
   protected float m_Range = 5000f;

   #endregion


   #region Properties

   /// <summary>
   /// Get information from the sensor (only if the sensor is on).
   /// </summary>
   public SortedSet<Target> TargetList { get { return m_IsOn ? m_TargetList : null; } }

   /// <summary>
   /// Whether the sensor is/should be on/off.
   /// </summary>
   public bool IsOn { get { return m_IsOn; } }

   #endregion


   #region Unity Methods

   void Awake()
   {
      // Find all targets in the area and list them once
      if(m_SceneTargets.Count < 1)
         m_SceneTargets = Resources.FindObjectsOfTypeAll<Target>().ToList<Target>();
   }

   void FixedUpdate()
   {
      // Add targets to the tracking list if they can be seen
      if(m_IsOn)
      {
         foreach(Target target in m_SceneTargets)
            ListIfTrackable(target);
      }

      // Clear the tracking list if the radar is off
      else if(m_TargetList.Count != 0)
      {
         m_TargetList.Clear();
      }
   }

   ///// <summary>
   ///// Check for targets already within the sensor range and list them if they are trackable targets.
   ///// </summary>
   //void OnTriggerStay(Collider collider)
   //{
   //   if(m_IsOn)
   //      ListIfTrackable(collider);
   //}

   ///// <summary>
   ///// Delete a target from the target list.
   ///// </summary>
   //void OnTriggerExit(Collider collider)
   //{
   //   if(m_IsOn)
   //      Unlist(collider);
   //}

   #endregion


   #region Private Methods

   /// <summary>
   /// Check whether a collider is a target and whether it is in LOS before adding/removing it from the target list.
   /// Main method to customize sensor behaviour.
   /// </summary>
   /// <param name="collider">A potential target</param>
   protected abstract void ListIfTrackable(Collider collider);

   protected abstract void ListIfTrackable(Target target);
   
   /// <summary>
   /// Remove a collider/potential target from the target list (perform a null check first).
   /// </summary>
   /// <param name="collider">A potential target</param>
   private void Unlist(Collider collider)
   {
      Target target = collider.gameObject.GetComponent<Target>();
      if(target != null)
         m_TargetList.Remove(target);
   }

   /// <summary>
   /// Check whether two objects can see each other. This does not verify any maximum distance.
   /// </summary>
   /// <param name="looker">The object seeing</param>
   /// <param name="target">The object being seen</param>
   /// <returns>True if within line of sight, false if another rigidbody is in the way</returns>
   //public static bool InLOS(Transform seeker, Transform target)
   //{
   //   RaycastHit hit;
   //   Physics.Linecast(seeker.position, target.position, out hit);

   //   try
   //   {
   //      if(hit.collider.gameObject == target.gameObject) // Null error? Make sure the sensor is not within a collider/rigidbody, or that trigger is on
   //         return true;
   //      else
   //         return false;
   //   }
   //   catch
   //   {
   //      Debug.Log(hit.collider.gameObject + " is not " + target.gameObject);
   //      return false;
   //   }
   //}

   public static bool InLineOfSight(Transform radarTransform, Target target)
   {
      RaycastHit hit;
      Physics.Linecast(radarTransform.position, target.transform.position, out hit);

      if(hit.rigidbody == null)
         return false;
      else if(hit.rigidbody.gameObject.GetComponent<Target>() == target)
         return true;
      else
         return false;
   }

   #endregion

   #region Public Methods

   /// <summary>
   /// Turn on this sensor.
   /// </summary>
   public void TurnOn()
   {
      m_IsOn = true;
   }

   /// <summary>
   /// Turn off this sensor.
   /// </summary>
   public void TurnOff()
   {
      m_IsOn = false;
   }

   #endregion
}
