using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Defines the projectile type from which all missiles (air-to-air, air-to-ground, ground-to-air, ICBM) derive from.
/// </summary>
public abstract class Projectile : MonoBehaviour, IWeapon
{
   #region Member Variables

   [Tooltip("The force directing the missile off a rack. No force is a simple drop.")]
   [SerializeField] Vector3 m_Impulse = new Vector3(0,0,0);

   [Tooltip("Time in seconds before the engine is started.")]
   [SerializeField][Range(0,2f)] protected float m_DropDelay = 0.2f;

   [SerializeField] protected ProjectileWeight m_ProjectileWeight = ProjectileWeight.Light;
   [SerializeField] string m_ShortName = null;
   [SerializeField] protected float m_Drag = 0.3f;
   [SerializeField] protected RigidbodyInterpolation m_Interpolate = RigidbodyInterpolation.Interpolate;
   [SerializeField] protected CollisionDetectionMode m_CollisionDetection = CollisionDetectionMode.ContinuousDynamic;

   protected Rigidbody m_Rigidbody;
   protected bool m_IsArmed = true;
   protected float m_FiredTime = -1f;
   protected List<SimpleWing> m_Fins = new List<SimpleWing>();

   #endregion


   #region Properties

   /// <summary>
   /// Get the projectile's short name.
   /// </summary>
   public string Abbreviation {
      get { return m_ShortName; }
   }

   /// <summary>
   /// The approximate projectile weight.
   /// </summary>
   public ProjectileWeight Weight {
      get { return m_ProjectileWeight; }
   }

   /// <summary>
   /// Whether the projectile has been launched yet.
   /// </summary>
   public bool IsFired {
      get { return (m_FiredTime == -1f) ? false : true; }
   }

   /// <summary>
   /// Whether the projectile is ready for explosion.
   /// </summary>
   public bool IsArmed {
      get { return m_IsArmed; }
   }

   #endregion


   #region Unity Methods

   protected void Awake()
   {
      m_Rigidbody = GetComponentInParent<Rigidbody>(); // The vehicle's rigidbody

      if(m_Rigidbody == null)
      {
         Debug.Log("Haha, " + gameObject + " is null rigid");
      }

      m_Fins = GetComponentsInChildren<SimpleWing>(true).ToList<SimpleWing>();
      foreach(SimpleWing fin in m_Fins)
      {
         fin.enabled = false;
      }
   }

   protected void Start()
   {
      m_ShortName = m_ShortName ?? Multilang.Text["projectile_short"];
   }

   protected void FixedUpdate()
   {
      if(IsFired)
         Gravity.Gravitate(m_Rigidbody);
   }

   #endregion


   #region Public Methods

   public abstract bool LaunchAuthority { get; }

   public abstract float HitProbability { get; }

   /// <summary>
   /// Main method to fire any projectile.
   /// Detach the projectile and add a rigidbody to it according to its weight, with an initial impulse equal to the 
   /// original speed of the carrying vehicle.
   /// </summary>
   public virtual bool Fire() {
      // Get the current speed, location, and rotation
      Vector3 velocity = m_Rigidbody.velocity;               // The vehicle's speed
      Vector3 angularVelocity = m_Rigidbody.angularVelocity; // The vehicle's turn rate

      // Detach the projectile game object
      transform.parent = null;
      gameObject.transform.parent = null;

      // Add a rigidbody to the game object
      gameObject.AddComponent<Rigidbody>(); // Change rigidbody reference
      m_Rigidbody = gameObject.GetComponent<Rigidbody>();
      m_Rigidbody.useGravity = true;
      m_Rigidbody.isKinematic = false;
      m_Rigidbody.drag = m_Drag;
      m_Rigidbody.angularDrag = m_Drag;
      m_Rigidbody.mass = (float)m_ProjectileWeight;
      m_Rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
      m_Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

      // Enable the fins and assign the rigidbody to them
      foreach(SimpleWing fin in m_Fins)
      {
         fin.enabled = true;
         fin.Rigidbody = m_Rigidbody;
      }

      // Transfer the vehicle's speed to the missile
      m_Rigidbody.AddForce(velocity, ForceMode.VelocityChange);
      m_Rigidbody.AddForce(angularVelocity, ForceMode.VelocityChange);

      // Apply the initial impulse force
      m_Rigidbody.AddForce(m_Impulse, ForceMode.VelocityChange);

      // Register the firing time (null if not fired)
      m_FiredTime = Time.time;

      return true;
   }

   public override string ToString()
   {
      return m_ShortName;
   }

   public static float TimeSince(float since)
   {
      return Time.time - since;
   }

   #endregion
}
