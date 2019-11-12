using UnityEngine;
/// <summary>
/// Defines a simple rocket that detaches from the holding ship and gets propelled straight forwards.
/// 
/// Denis Labrecque
/// </summary>
public class UnguidedMissile : Projectile
{
   Motor m_Motor;

   float m_AerodynamicEffect = 1f;

   #region Unity Methods

   void Awake()
   {
      base.Awake();

      m_Motor = GetComponent<Motor>();
      m_Motor.IsEnabled = false;
   }

   void Update()
   {
      // Fly the missile
      ComputeLift();
      FaceMotionVector();
   }

   #endregion


   #region Properties

   public override bool LaunchAuthority => true; // Simple rockets are always authorized to launch

   public override float HitProbability => throw new System.NotImplementedException();

   public float ForwardSpeed {
      get {
         // Not the same as velocity (if falling in a stall)
         var localVelocity = transform.InverseTransformDirection(m_Rigidbody.velocity);
         return Mathf.Max(0, localVelocity.z);
      }
   }

   #endregion


   #region Methods

   /// <summary>
   /// Separate the projectile and fire the engine.
   /// </summary>
   public override void Fire()
   {
      base.Fire();

      // Start the rocket
      m_Motor.StartMotor(m_Rigidbody); // Verify that the motor was not checked as started
   }

   private void ComputeLift()
   {
      var forces = Vector3.zero;

      // The direction that the lift force is applied is at right angles to the plane's velocity (away from the planet)
      var liftDirection = Vector3.Cross(
         m_Rigidbody.velocity,
         Planet.Singleton.gameObject.transform.position - gameObject.transform.position).normalized;

      // Calculate and add the lift power; attitude changes add drag, but also add lift when pointing up
      var liftPower = ForwardSpeed * ForwardSpeed * Planet.Singleton.AirDensity(gameObject);
      forces += (liftPower - m_Rigidbody.drag) * liftDirection;

      // Apply the calculated forces to the the Rigidbody
      m_Rigidbody.AddForce(forces);
   }

   private void FaceMotionVector()
   {
      // "Aerodynamic" calculations. This is a very simple approximation of the effect that a plane
      // will naturally try to align itself in the direction that it's facing when moving at speed.
      // Without this, the plane would behave a bit like the asteroids spaceship!
      if(m_Rigidbody.velocity.magnitude > 0)
      {
         // Compare the direction we're pointing with the direction we're moving:
         float aeroFactor = Vector3.Dot(transform.forward, m_Rigidbody.velocity.normalized);
         // multipled by itself results in a desirable rolloff curve of the effect
         aeroFactor *= aeroFactor;
         // Finally we calculate a new velocity by bending the current velocity direction towards
         // the the direction the plane is facing, by an amount based on this aeroFactor
         var newVelocity = Vector3.Lerp(
            m_Rigidbody.velocity,
            transform.forward * ForwardSpeed,
            aeroFactor * ForwardSpeed * m_AerodynamicEffect * Time.deltaTime);
         m_Rigidbody.velocity = newVelocity;

         // also rotate the plane towards the direction of movement - this should be a very small effect, but means the plane ends up
         // pointing downwards in a stall
         m_Rigidbody.rotation = Quaternion.Slerp(
            m_Rigidbody.rotation,
            Quaternion.LookRotation(m_Rigidbody.velocity, transform.up),
            m_AerodynamicEffect * m_AerodynamicEffect * m_AerodynamicEffect * Time.deltaTime);
      }
   }

   #endregion
}
