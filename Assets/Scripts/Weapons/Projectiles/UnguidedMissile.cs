using UnityEngine;
/// <summary>
/// Defines a simple rocket that detaches from the holding ship and gets propelled straight forwards.
/// 
/// Denis Labrecque
/// </summary>
public class UnguidedMissile : Projectile
{
   Motor _motor;

   float _aerodynamicEffect = 1f;

   #region Unity Methods

   void Awake()
   {
      base.Awake();

      _motor = GetComponentInChildren<Motor>();
      _motor.IsEnabled = false;
   }

   void Update()
   {
      // Fly the missile
      //ComputeLift();
      //FaceMotionVector();
   }

   #endregion


   #region Properties

   public override bool LaunchAuthority => true; // Simple rockets are always authorized to launch

   public override float HitProbability => throw new System.NotImplementedException();

   public float ForwardSpeed {
      get {
         // Not the same as velocity (if falling in a stall)
         var localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
         return Mathf.Max(0, localVelocity.z);
      }
   }

   #endregion


   #region Methods

   /// <summary>
   /// Separate the projectile and fire the engine.
   /// </summary>
   public override bool Fire()
   {
      base.Fire();

      // Start the rocket
      _motor.StartMotor(_rigidbody); // Verify that the motor was not checked as started

      return true;
   }

   private void ComputeLift()
   {
      var forces = Vector3.zero;

      // The direction that the lift force is applied is at right angles to the plane's velocity (away from the planet)
      var liftDirection = Vector3.Cross(
         _rigidbody.velocity,
         Planet.Singleton.gameObject.transform.position - gameObject.transform.position).normalized;

      // Calculate and add the lift power; attitude changes add drag, but also add lift when pointing up
      var liftPower = ForwardSpeed * ForwardSpeed * Planet.Singleton.AirDensity(gameObject);
      forces += (liftPower - _rigidbody.drag) * liftDirection;

      // Apply the calculated forces to the the Rigidbody
      _rigidbody.AddForce(forces);
   }

   private void FaceMotionVector()
   {
      // "Aerodynamic" calculations. This is a very simple approximation of the effect that a plane
      // will naturally try to align itself in the direction that it's facing when moving at speed.
      // Without this, the plane would behave a bit like the asteroids spaceship!
      if(_rigidbody.velocity.magnitude > 0)
      {
         // Compare the direction we're pointing with the direction we're moving:
         float aeroFactor = Vector3.Dot(transform.forward, _rigidbody.velocity.normalized);
         // multipled by itself results in a desirable rolloff curve of the effect
         aeroFactor *= aeroFactor;
         // Finally we calculate a new velocity by bending the current velocity direction towards
         // the the direction the plane is facing, by an amount based on this aeroFactor
         var newVelocity = Vector3.Lerp(
            _rigidbody.velocity,
            transform.forward * ForwardSpeed,
            aeroFactor * ForwardSpeed * _aerodynamicEffect * Time.deltaTime);
         _rigidbody.velocity = newVelocity;

         // also rotate the plane towards the direction of movement - this should be a very small effect, but means the plane ends up
         // pointing downwards in a stall
         _rigidbody.rotation = Quaternion.Slerp(
            _rigidbody.rotation,
            Quaternion.LookRotation(_rigidbody.velocity, transform.up),
            _aerodynamicEffect * _aerodynamicEffect * _aerodynamicEffect * Time.deltaTime);
      }
   }

   #endregion
}
