using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IWeapon
{
   [SerializeField][Range(0,180)] float m_AngularLimit = 2.2f;
   [SerializeField] float m_MaxDistance = 5000f;
   [SerializeField][Range(100,10000)] float m_Power = 500f;
   Target m_LaserTarget = null;
   LineRenderer m_LineRenderer;

   void Awake()
   {
      m_LineRenderer = GetComponent<LineRenderer>();

      if(m_LineRenderer == null)
         Debug.LogError("Laser " + gameObject + " on " + transform.root + " has no line renderer component");
      else
         m_LineRenderer.enabled = false;
   }

   /// <summary>
   /// Test whether there is a target to shoot, whether it is close enough, whether it is angled correctly,
   /// and whether it is hidden behind another collider.
   /// </summary>
   public bool LaunchAuthority {
      get {
         // There is no target
         if(m_LaserTarget == null)
            return false;

         // Target is too far away
         else if(Vector3.Distance(transform.position, m_LaserTarget.transform.position) > m_MaxDistance)
            return false;

         // Target is a too great angle from forwards
         else if(!TargetIsInAngularLimit())
            return false;

         // Target cannot be hit with a raycast from the laser (not from the detection system this time)
         //else if(Sensor.InLOS(transform, m_LaserTarget.transform))
         //   return false;

         else return true;
      }
   }

   /// <summary>
   /// A laser has 100% hit probability if it can shoot, because it automatically aims towards a target.
   /// </summary>
   public float HitProbability {
      get {
         if(LaunchAuthority)
            return 1f;
         else
            return 0f;
      }
   }

   /// <summary>
   /// Method called each frame to fire.
   /// </summary>
   public bool Fire()
   {
      if (LaunchAuthority)
      {
         // Visuals
         m_LineRenderer.enabled = true;
         m_LineRenderer.gameObject.transform.LookAt(m_LaserTarget.transform);
         m_LineRenderer.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, m_LaserTarget.transform.position)));

         // Hurt the target
         m_LaserTarget.Damage(m_Power);

         return true;
      }
      else
      {
         StopFire();
         return false;
      }
   }

   /// <summary>
   /// Method that clears the gun from firing.
   /// </summary>
   public void StopFire()
   {
      if(m_LineRenderer.enabled)
      {
         m_LineRenderer.enabled = false;
      }
   }

   /// <summary>
   /// Set a target for the laser to have launch authority
   /// </summary>
   /// <param name="target">A target that makes sense the laser should shoot first</param>
   public void SetTarget(Target target)
   {
      m_LaserTarget = target;
   }

   /// <summary>
   /// Check whether the currently assigned target can be hit within the laser's parameters
   /// </summary>
   private bool TargetIsInAngularLimit()
   {
      Vector3 targetDirection = m_LaserTarget.transform.position - gameObject.transform.position;

      // Because the game object is rotated so the laser beam points towards the target, always use the parent as reference
      if(Vector3.Angle(targetDirection, gameObject.transform.parent.transform.forward) <= m_AngularLimit)
         return true;
      else
         return false;
   }
}
