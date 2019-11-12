using UnityEngine;

/// <summary>
/// A sensor that can find targets depending on their radar cross section.
/// 
/// Denis Labrecque
/// December 2018
/// </summary>
public class Radar : Sensor
{
   const float RADAR_NOT_ON_ALTITUDE = -1f;

   #region Properties

   /// <summary>
   /// Radar altitude in meters above ground.
   /// Returns a negative if the radar is off.
   /// </summary>
   public float Altitude {
      get {
         if(m_IsOn == false)
            return -1f;
         else
         {
            // Raycast towards the earth
            RaycastHit hitInfo = new RaycastHit();
            Physics.Raycast(transform.position, Planet.Singleton.transform.position - transform.position, out hitInfo, Mathf.Infinity);
            Debug.DrawRay(transform.position, Planet.Singleton.transform.position - transform.position, Color.magenta);
            return hitInfo.distance;
         }
      }
   }

   #endregion


   /// <summary>
   /// Perform checks before considering that a sensor can see a target.
   /// </summary>
   /// <param name="other">The collider of the game object that has entered the radar.</param>
   protected override void ListIfTrackable(Collider other)
   {
      // Check whether the collider is actually a target
      Target target = other.transform.GetComponent<Target>();

      if(target == null)
      {
         // Debug.DrawLine(gameObject.transform.position, other.transform.position, Color.cyan);
         return;
      }

      // Check radar cross section
      else if(!RCSIsSufficient(target.RadarCrossSection, m_Range, Vector3.Distance(gameObject.transform.position, other.transform.position)))
      {
         Debug.DrawLine(gameObject.transform.position, other.transform.position, Color.grey);
         m_TargetList.Remove(target);
      }

      // Check whether the target is in LOS
      //else if(!InLOS(gameObject.transform, other.transform))
      //{
      //   Debug.DrawLine(gameObject.transform.position, other.transform.position, Color.yellow);
      //   m_TargetList.Remove(target);
      //}

      // Finally add the target because it can be tracked
      else
      {
         Debug.DrawLine(gameObject.transform.position, other.transform.position, Color.green);
         m_TargetList.Add(target);
      }
   }


   /// <summary>
   /// Perform checks before considering that a sensor can see a target.
   /// </summary>
   /// <param name="target">The target to be listed if trackable</param>
   protected override void ListIfTrackable(Target target)
   {
      float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);

      // Whether the target is in range
      if(distance > m_Range)
      {
         m_TargetList.Remove(target);
         return;
      }

      // Whether radar cross section is sufficient
      else if(!RCSIsSufficient(target.RadarCrossSection, m_Range, Vector3.Distance(gameObject.transform.position, target.transform.position)))
      {
         Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.grey);
         m_TargetList.Remove(target);
         return;
      }

      // Whether target is in line of sight
      else if(!InLineOfSight(gameObject.transform, target))
      {
         Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.yellow);
//         Debug.Log("LOS from " + gameObject + " on " + gameObject.GetComponentInParent<Target>().PopularName + " to " + target.PopularName);
         m_TargetList.Remove(target);
      }

      // Add the target because it passed all tests to be tracked
      else
      {
         Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.green);
         m_TargetList.Add(target);
         return;
      }
   }


   /// <summary>
   /// Whether a target with a specific radar return can be spotted at a certain distance.
   /// </summary>
   /// <param name="RCS">A percent of 1</param>
   /// <param name="radarRange">The sensor's maximum effective range</param>
   /// <param name="actualDistance">Distance between radar and target</param>
   /// <returns></returns>
   private bool RCSIsSufficient(float RCS, float radarRange, float actualDistance)
   {
      // An RCS of .75 gives a target a .25 * distance advantage
      if(actualDistance + ((1 - RCS) * actualDistance) <= radarRange)
         return true;
      else
         return false;
   }
}