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
   protected override void ListIfTrackable(Target other)
   {
      // TODO this code got messed up
      //// Check whether the collider is actually a target with radar
      //Radar radar = other.gameObject.GetComponentInChildren<Radar>();
      //if(target == null || radar == null)
      //   return;

      //// Check whether the target is on
      //else if(!radar.IsOn)
      //{
      //   Debug.DrawLine(gameObject.transform.position, other.transform.position, Color.grey);
      //   _targetList.Remove(target);
      //}

      //// Check whether the target is in LOS
      ////else if(!InLOS(gameObject.transform, collider.transform))
      ////{
      ////   Debug.DrawLine(gameObject.transform.position, collider.transform.position, Color.yellow);
      ////   m_TargetList.Remove(target);
      ////}

      //// Finally add the target because it can be tracked
      //else
      //{
      //   Debug.DrawLine(gameObject.transform.position, other.transform.position, Color.green);
      //   _targetList.Add(target);
      //}
   }
}
