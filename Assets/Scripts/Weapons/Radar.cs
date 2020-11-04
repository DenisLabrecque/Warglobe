using UnityEngine;

namespace Warglobe
{
   /// <summary>
   /// A sensor that can find targets depending on their radar cross section.
   /// 
   /// Denis Labrecque
   /// December 2018
   /// </summary>
   public class Radar : ActiveSensor
   {
      [SerializeField] Spin _spin;

      #region Properties

      /// <summary>
      /// Radar altitude in meters above ground.
      /// </summary>
      public float Altitude {
         get {
            if (_isOn == false)
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
      /// <param name="target">The target to be listed if trackable</param>
      protected override void ListIfTrackable(Target target)
      {
         float distance = Vector3.Distance(gameObject.transform.position, target.transform.position);

         // Whether the target is in range
         if (distance > _range)
         {
            _targetList.Remove(target);
            return;
         }

         // Whether radar cross section is sufficient
         else if (!RCSIsSufficient(target.RadarCrossSection, _range, Vector3.Distance(gameObject.transform.position, target.transform.position)))
         {
            // Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.grey);
            _targetList.Remove(target);
            return;
         }

         //      // Whether target is in line of sight
         //      else if(!InLineOfSight(gameObject.transform, target))
         //      {
         //         Debug.Log("Not in LOS " + target);
         //         Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.yellow);
         ////         Debug.Log("LOS from " + gameObject + " on " + gameObject.GetComponentInParent<Target>().PopularName + " to " + target.PopularName);
         //         _targetList.Remove(target);
         //      }

         // Add the target because it passed all tests to be tracked
         else
         {
            Debug.DrawLine(gameObject.transform.position, target.transform.position, Color.green);
            _targetList.Add(target);
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
         if (actualDistance + ((1 - RCS) * actualDistance) <= radarRange)
            return true;
         else
            return false;
      }

      /// <summary>
      /// Switch the radar on/off.
      /// </summary>
      /// <param name="onOff">True for on, false for off.</param>
      public override void Switch(bool onOff)
      {
         _isOn = onOff;
         if (_spin != null)
            _spin.Switch(onOff);
      }

      /// <summary>
      /// Flip the radar from on to off or vice versa.
      /// </summary>
      public override void Switch()
      {
         _isOn = !_isOn;
         if (_spin != null)
            _spin.Switch(_isOn);
      }
   }
}