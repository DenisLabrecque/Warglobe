using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// From Brian Hernandez.
/// Adapted November 2019 by Denis Labrecque.
/// Emplements anything that can rotate a gun.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Turret : MonoBehaviour, IWeapon
{
   [Tooltip("Should turret rotate in the FixedUpdate rather than Update?")]
   public bool m_RunInFixed = false;

   [Header("Objects")]
   [Tooltip("Transform used to provide the horizontal rotation of the turret.")]
   [SerializeField] Transform m_Base;
   [Tooltip("Transform used to provide the vertical rotation of the barrels. Must be a child of the TurretBase.")]
   public Transform m_Barrels;

   [Header("Rotation Limits")]
   [Tooltip("Turn rate of the turret's base and barrels in degrees per second.")]
   [SerializeField] float m_TurnRate = 30.0f;
   [Tooltip("When true, turret rotates according to left/right traverse limits. When false, turret can rotate freely.")]
   [SerializeField] public bool m_LimitTraverse = false;
   [Tooltip("When traverse is limited, how many degrees to the left the turret can turn.")]
   [Range(0.0f, 180.0f)]
   [SerializeField] public float m_LeftTraverse = 60.0f;
   [Tooltip("When traverse is limited, how many degrees to the right the turret can turn.")]
   [Range(0.0f, 180.0f)]
   [SerializeField] public float m_RightTraverse = 60.0f;
   [Tooltip("How far up the barrel(s) can rotate.")]
   [Range(0.0f, 90.0f)]
   [SerializeField] public float m_MaxElevation = 60.0f;
   [Tooltip("How far down the barrel(s) can rotate.")]
   [Range(0.0f, 90.0f)]
   [SerializeField] public float m_MaxDepression = 5.0f;

   [Header("Bullets")]
   [Tooltip("How long before each canon in the turret can refire")]
   [SerializeField] float m_ReloadTime = 2f;

   [Header("Utilities")]
   [Tooltip("Show the arcs that the turret can aim through.\n\nRed: Left/Right Traverse\nGreen: Elevation\nBlue: Depression")]
   [SerializeField] public bool m_ShowArcs = false;
   [Tooltip("When game is running in editor, draws a debug ray to show where the turret is aiming.")]
   [SerializeField] bool m_ShowDebugRay = true;

   List<GunMuzzle> m_Muzzles = new List<GunMuzzle>();
   private Vector3 m_AimPoint;
   private AudioSource m_Audio;
   private float m_ReloadTimer = 0f;
   private bool m_IsAiming = false;
   private bool m_IsAtRest = false;

   /// <summary>
   /// Turret is no longer aiming at anything, returns to resting position, and stops rotating.
   /// </summary>
   public bool Idle { get { return !m_IsAiming; } }

   /// <summary>
   /// Turret is idle and in a resting position.
   /// </summary>
   public bool AtRest { get { return m_IsAtRest; } }

   public bool IsReloaded {
      get {
         if (m_ReloadTimer >= m_ReloadTime)
         {
            return true;
         }
         else
            return false;
      }
   }

   bool IWeapon.LaunchAuthority => throw new System.NotImplementedException();

   float IWeapon.HitProbability => throw new System.NotImplementedException();

   private void Awake()
   {
      m_Muzzles = GetComponentsInChildren<GunMuzzle>().ToList();
      m_Audio = GetComponent<AudioSource>();
      m_Audio.playOnAwake = false;
   }

   private void Start()
   {
      if(m_Muzzles.Count == 0)
      {
         throw new System.Exception("A Turret requires at least one muzzle to fire");
      }
      if (m_IsAiming == false)
         m_AimPoint = transform.TransformPoint(Vector3.forward * 100.0f);
   }

   private void Update()
   {
      m_ReloadTimer += Time.deltaTime;

      if (!m_RunInFixed)
      {
         RotateTurret();
      }

      if (m_ShowDebugRay)
         DrawDebugRays();
   }

   private void FixedUpdate()
   {
      if (m_RunInFixed)
      {
         RotateTurret();
      }
   }

   /// <summary>
   /// Give the turret a position to aim at. If not idle, it will rotate to aim at this point.
   /// </summary>
   public void SetAimpoint(Vector3 position)
   {
      m_IsAiming = true;
      m_AimPoint = position;
   }

   /// <summary>
   /// When idle, turret returns to resting position, will not track an aimpoint, and rotations stop updating.
   /// </summary>
   public void SetIdle(bool idle)
   {
      m_IsAiming = !idle;

      if (m_IsAiming)
         m_IsAtRest = false;
   }

   /// <summary>
   /// Attempts to automatically assign the turretBase and turretBarrels transforms. Will search for a transform
   /// named "Base" for turretBase and a transform named "Barrels" for the turretBarrels.
   /// </summary>
   public void AutoPopulateBaseAndBarrels()
   {
      // Don't allow this while ingame.
      if (!Application.isPlaying)
      {
         m_Base = transform.Find("Base");
         if (m_Base != null)
            m_Barrels = m_Base.Find("Barrels");
      }
      else
      {
         Debug.LogWarning(name + ": Turret cannot auto-populate transforms while game is playing.");
      }
   }

   /// <summary>
   /// Sets the turretBase and turretBarrels transforms to null.
   /// </summary>
   public void ClearTransforms()
   {
      // Don't allow this while ingame.
      if (!Application.isPlaying)
      {
         m_Base = null;
         m_Barrels = null;
      }
      else
      {
         Debug.LogWarning(name + ": Turret cannot clear transforms while game is playing.");
      }
   }

   private void RotateTurret()
   {
      if (m_IsAiming)
      {
         RotateBase();
         RotateBarrels();
      }
      else if (!m_IsAtRest)
      {
         m_IsAtRest = RotateToIdle();
      }
   }

   private void RotateBase()
   {
      // TODO: Turret needs to rotate the long way around if the aimpoint gets behind
      // it and traversal limits prevent it from taking the shortest rotation.
      if (m_Base != null)
      {
         // Note, the local conversion has to come from the parent.
         Vector3 localTargetPos = transform.InverseTransformPoint(m_AimPoint);
         localTargetPos.y = 0.0f;

         // Clamp target rotation by creating a limited rotation to the target.
         // Use different clamps depending if the target is to the left or right of the turret.
         Vector3 clampedLocalVec2Target = localTargetPos;
         if (m_LimitTraverse)
         {
            if (localTargetPos.x >= 0.0f)
               clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * m_RightTraverse, float.MaxValue);
            else
               clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * m_LeftTraverse, float.MaxValue);
         }

         // Create new rotation towards the target in local space.
         Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
         Quaternion newRotation = Quaternion.RotateTowards(m_Base.localRotation, rotationGoal, m_TurnRate * Time.deltaTime);

         // Set the new rotation of the base.
         m_Base.localRotation = newRotation;
      }
   }

   private void RotateBarrels()
   {
      // TODO: A target position directly to the turret's right will cause the turret
      // to attempt to aim straight up. This looks silly and on slow moving turrets can
      // cause delays on targeting. This is why barrels have a boosted rotation speed.
      if (m_Base != null && m_Barrels != null)
      {
         // Note, the local conversion has to come from the parent.
         Vector3 localTargetPos = m_Base.InverseTransformPoint(m_AimPoint);
         localTargetPos.x = 0.0f;

         // Clamp target rotation by creating a limited rotation to the target.
         // Use different clamps depending if the target is above or below the turret.
         Vector3 clampedLocalVec2Target = localTargetPos;
         if (localTargetPos.y >= 0.0f)
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * m_MaxElevation, float.MaxValue);
         else
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * m_MaxDepression, float.MaxValue);

         // Create new rotation towards the target in local space.
         Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
         Quaternion newRotation = Quaternion.RotateTowards(m_Barrels.localRotation, rotationGoal, 2.0f * m_TurnRate * Time.deltaTime);

         // Set the new rotation of the barrels.
         m_Barrels.localRotation = newRotation;
      }
   }

   /// <summary>
   /// Rotates the turret to resting position.
   /// </summary>
   /// <returns>True when turret has finished rotating to resting positing.</returns>
   private bool RotateToIdle()
   {
      bool baseFinished = false;
      bool barrelsFinished = false;

      if (m_Base != null)
      {
         Quaternion newRotation = Quaternion.RotateTowards(m_Base.localRotation, Quaternion.identity, m_TurnRate * Time.deltaTime);
         m_Base.localRotation = newRotation;

         if (m_Base.localRotation == Quaternion.identity)
            baseFinished = true;
      }

      if (m_Barrels != null)
      {
         Quaternion newRotation = Quaternion.RotateTowards(m_Barrels.localRotation, Quaternion.identity, 2.0f * m_TurnRate * Time.deltaTime);
         m_Barrels.localRotation = newRotation;

         if (m_Barrels.localRotation == Quaternion.identity)
            barrelsFinished = true;
      }

      return (baseFinished && barrelsFinished);
   }

   private void DrawDebugRays()
   {
      if (m_Barrels != null)
         Debug.DrawRay(m_Barrels.position, m_Barrels.forward * 100.0f);
      else if (m_Base != null)
         Debug.DrawRay(m_Base.position, m_Base.forward * 100.0f);
   }

   /// <summary>
   /// Fire this turret. Not garanteed to work. The turret will not fire if it is not reloaded.
   /// Returns a boolean of whether it fired or not.
   /// </summary>
   public bool Fire()
   {
      if (IsReloaded)
      {
         m_ReloadTimer = 0f;

         foreach (GunMuzzle muzzle in m_Muzzles)
         {
            muzzle.Fire();
         }

         m_Audio.Play();

         return true; // Has fired
      }
      else
         return false; // Has not fired
   }
}
