using UnityEngine;
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
         Debug.Log("Air density: " + Planet.Singleton.AirDensity(gameObject));
         _rigidbody.AddRelativeForce(_thrustDirection * Thrust * Planet.Singleton.AirDensity(gameObject));
      }
   }
}