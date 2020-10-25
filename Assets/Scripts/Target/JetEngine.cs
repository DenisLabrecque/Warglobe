/// <summary>
/// Motor that simply pushes a rigidbody forwards.
/// Aware of air thickness for thrust.
/// </summary>
public class JetEngine : Motor
{
   void FixedUpdate()
   {
      if (IsEnabled && _currentEnergy > 0)
      {
         // Apply the throttle behind a vehicle
         _rigidbody.AddRelativeForce(_thrustDirection * Thrust * Planet.Singleton.AirDensity(gameObject));
      }
   }
}