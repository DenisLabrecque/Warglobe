/// <summary>
/// Motor that simply pushes a rigidbody forwards.
/// Aware of air thickness for thrust.
/// </summary>
public class JetEngine : Motor
{
   void FixedUpdate()
   {
      if (IsEnabled && m_CurrentBattery > 0)
      {
         // Apply the throttle behind a vehicle
         m_Rigidbody.AddRelativeForce(m_ThrustDirection * CurrentThrust * Planet.Singleton.AirDensity(gameObject));
      }
   }
}