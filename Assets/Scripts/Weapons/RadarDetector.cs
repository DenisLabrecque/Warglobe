using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A sensor that can find other radars that are turned on.
/// 
/// Denis Labrecque
/// December 2018
/// </summary>
public class RadarDetector : Sensor
{
   /// <summary>
   /// Perform checks before considering that a sensor can see a target.
   /// </summary>
   /// <param name="collider"></param>
   protected override void ListIfTrackable(Collider collider)
   {
      // Check whether the collider is actually a target with radar
      Target target = collider.gameObject.GetComponent<Target>();
      Radar radar = collider.gameObject.GetComponentInChildren<Radar>();
      if(target == null || radar == null)
         return;

      // Check whether the target is on
      else if(!radar.IsOn)
      {
         Debug.DrawLine(gameObject.transform.position, collider.transform.position, Color.grey);
         m_TargetList.Remove(target);
      }

      // Check whether the target is in LOS
      //else if(!InLOS(gameObject.transform, collider.transform))
      //{
      //   Debug.DrawLine(gameObject.transform.position, collider.transform.position, Color.yellow);
      //   m_TargetList.Remove(target);
      //}

      // Finally add the target because it can be tracked
      else
      {
         Debug.DrawLine(gameObject.transform.position, collider.transform.position, Color.green);
         m_TargetList.Add(target);
      }
   }

   protected override void ListIfTrackable(Target target)
   {
      
   }
}
