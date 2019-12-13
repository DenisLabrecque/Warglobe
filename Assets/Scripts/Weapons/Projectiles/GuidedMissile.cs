using UnityEngine;
using DGL.Math;

/// <summary>
/// Defines a missile that is launched at a target to destroy it, and can match the target's movements.
/// 
/// Denis Labrecque
/// May 2019
/// Inspired heavily by Brian Hernandez's AAMissile.cs script.
/// </summary>
public class GuidedMissile : Projectile
{
   #region Member Variables

   Motor m_Motor;
   [SerializeField] Target m_Target = null;

   [Tooltip("Pursuit flies directly towards the target. Lead will fly ahead to intercept, making it significantly more difficult to dodge.")]
   public GuidanceType m_GuidanceType = GuidanceType.Pursuit;

   [Tooltip("How tightly the missile can turn in radians per second. More is faster")]
   [SerializeField] float m_TurnRate = 2f;

   [Tooltip("How far off boresight the missile can see the target. Also restricts how far the missile can lead.")]
   public float m_SeekerCone = 45.0f;

   [Tooltip("How far off boresight the missile can see the target. Also restricts how far the missile can lead.")]
   public float m_SeekerRange = 5000.0f;

   private Vector3 m_TargetPosLastFrame;
   private Quaternion m_GuidedRotation;

   #endregion


   #region Properties

   /// <summary>
   /// Whether the target is within the seeker cone angle of the missile head.
   /// </summary>
   public bool TargetIsWithinCone {
      get {
         if(AngleToTarget < m_SeekerCone)
            return false;
         else
            return true;
      }
   }

   /// <summary>
   /// Angle the target is away from the seeker head's center vector.
   /// </summary>
   public float AngleToTarget {
      get {
         // Get a vector to the target, use it to find angle to target for seeker cone check.
         return Mathf.Abs(Vector3.Angle(transform.forward.normalized, VectorToTarget.normalized));
      }
   }

   /// <summary>
   /// Whether the target can still be seen by the missile.
   /// </summary>
   public bool TargetIsWithinRange {
      get {
         if(DistanceToTarget <= m_SeekerRange)
            return true;
         else
            return false;
      }
   }

   /// <summary>
   /// Distance between seeker cone and target.
   /// </summary>
   public float DistanceToTarget {
      get {
         return Vector3.Distance(m_Target.transform.position, transform.position);
      }
   }

   /// <summary>
   /// Direction towards the target.
   /// </summary>
   public Vector3 VectorToTarget {
      get {
         return m_Target.transform.position - transform.position;
      }
   }

   /// <summary>
   /// Whether the target is set, within range, and within the missile's targeting cone.
   /// </summary>
   public override bool LaunchAuthority {
      get {
         if(m_Target != null && TargetIsWithinRange && TargetIsWithinCone)
            return true;
         else
            return false;
      }
   }

   /// <summary>
   /// A hit is 100% probable if it is at zero distance and zero degrees off center.
   /// </summary>
   public override float HitProbability {
      get {
         return (Utility.Percent(DistanceToTarget, m_SeekerRange, PercentMode.Clamp0To1) + Utility.Percent(AngleToTarget, m_SeekerCone, PercentMode.Clamp0To1)) / 2.0f;
      }
   }

   /// <summary>
   /// Whether the missile is done freefalling and can now turn on its engine.
   /// </summary>
   public bool IsDropDelayPassed {
      get {
         return TimeSince(m_FiredTime) >= m_DropDelay;
      }
   }

   #endregion


   #region Unity Methods

   void Awake()
   {
      base.Awake();

      m_Motor = GetComponentInChildren<Motor>();
      m_Motor.IsEnabled = false;
   }

   private void FixedUpdate()
   {
      // Gravitate
      base.FixedUpdate();

      // Turn towards target
      if (m_Motor.IsEnabled)
      {
         //MissileGuidance();
         //transform.LookAt(m_Target.transform);
         //m_Rigidbody.AddRelativeTorque()
         //transform.rotation = Quaternion.RotateTowards(transform.rotation, m_GuidedRotation, m_TurnRate * Time.deltaTime);

         // The step size is equal to speed times frame time.
         float singleStep = m_TurnRate * Time.deltaTime;

         // Rotate the forward vector towards the target direction by one step
         Vector3 newDirection = Vector3.RotateTowards(transform.right, VectorToTarget, singleStep, 0.0f);

         // Draw a ray pointing at our target in
         Debug.DrawRay(transform.position, newDirection, Color.red);

         // Calculate a rotation a step closer to the target and applies rotation to this object
         transform.rotation = Quaternion.LookRotation(newDirection);
      }
   }

   //void Update()
   //{
   //   if(!m_Motor.IsEnabled && IsDropDelayPassed)
   //      m_Motor.StartMotor(m_Rigidbody);
   //}

   #endregion


   #region Methods

   /// <summary>
   /// Fire the guided missile towards the target if it has launch authorization and a target is acquired.
   /// TODO this is for testing. Firing should be at a target.
   /// </summary>
   public override bool Fire()
   {
      base.Fire();

      m_Motor.StartMotor(m_Rigidbody);

      return true;
   }

   /// <summary>
   /// Fire a guided missile towards a target.
   /// </summary>
   /// <param name="target">The target to hit</param>
   public void Fire(Target target)
   {
      base.Fire();

      target = m_Target;
      Debug.Log("Missile fired at " + target + "!");
   }   

   /// <summary>
   /// Intelligent algorithm to direct the missile towards the target.
   /// </summary>
   private void MissileGuidance()
   {
      // When the target gets out of line of sight of the seeker's FOV or out of range, it can no longer track.
      if(!TargetIsWithinRange || !TargetIsWithinCone)
      {
         m_Target = null;
         return;
      }

      // Turn the missile
      else
      {
         // Pursuit guidance
         if(m_GuidanceType == GuidanceType.Pursuit)
         {
            m_GuidedRotation = Quaternion.LookRotation(VectorToTarget, transform.up); // TODO that's wrong because of planet
         }

         // Lead guidance
         else
         {
            // Get where target will be in one second.
            Vector3 targetVelocity = m_Target.transform.position - m_TargetPosLastFrame;
            targetVelocity /= Time.deltaTime;

            //=====================================================

            // Figure out time to impact based on distance.                
            //float dist = Mathf.Max(Vector3.Distance(target.position, transform.position), missileSpeed);
            //float predictedSpeed = Mathf.Min(initialSpeed + acceleration * motorLifetime, missileSpeed + acceleration * TimeSince(m_DropTime));
            //float timeToImpact = dist / Mathf.Max(predictedSpeed, MINIMUM_GUIDE_SPEED);

            // Create lead position based on target velocity and time to impact.                
            Vector3 leadPos = m_Target.transform.position + targetVelocity;// * timeToImpact;
            Vector3 leadVec = leadPos - transform.position;

            //print(leadVec.magnitude.ToString());

            //=====================================================

            // It's very easy for the lead position to be outside of the seeker head. To prevent
            // this, only allow the target direction to be 90% of the seeker head's limit.
            Vector3 vectorToTarget = Vector3.RotateTowards(VectorToTarget.normalized, leadVec.normalized, m_SeekerCone * Mathf.Deg2Rad * 0.9f, 0.0f);
            m_GuidedRotation = Quaternion.LookRotation(vectorToTarget, transform.up);

            Debug.DrawRay(m_Target.transform.position, targetVelocity, Color.red);
            //Debug.DrawRay(m_Target.transform.position, targetVelocity * timeToHit, Color.red);
            Debug.DrawRay(transform.position, leadVec, Color.red);

            m_TargetPosLastFrame = m_Target.transform.position;
         }
      }
   }

   #endregion
}
