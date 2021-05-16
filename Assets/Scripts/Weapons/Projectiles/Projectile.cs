using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Warglobe.Assets;

namespace Warglobe
{
   /// <summary>
   /// Defines the projectile type from which all missiles (air-to-air, air-to-ground, ground-to-air, ICBM, bomb) derive from.
   /// </summary>
   public abstract class Projectile : MonoBehaviour, IFireable, ISwitchable
   {
      #region Member Variables

      [Header("Switchable")]
      [SerializeField] Switchable _switchable;

      [Tooltip("The force directing the missile off a rack. No force is a simple drop.")]
      [SerializeField] Vector3 _impulse = new Vector3(0, 0, 0);

      [Tooltip("Time in seconds before the engine is started.")]
      [SerializeField] [Range(0, 2f)] protected float _dropDelay = 0.2f;

      [SerializeField] protected ProjectileWeight _weight = ProjectileWeight.Light;
      [SerializeField] string _shortName = null;
      [SerializeField] protected float _drag = 0.3f;
      [SerializeField] protected RigidbodyInterpolation _interpolation = RigidbodyInterpolation.Interpolate;
      [SerializeField] protected CollisionDetectionMode _collisionDetection = CollisionDetectionMode.ContinuousDynamic;

      protected Rigidbody _rigidbody;
      protected bool _isArmed = true;
      protected float _firedTime = -1f;
      protected List<SimpleWing> _fins = new List<SimpleWing>();

      #endregion


      #region Properties

      /// <summary>
      /// Get the projectile's short name.
      /// </summary>
      public string Abbreviation => _shortName;

      /// <summary>
      /// The approximate projectile weight.
      /// </summary>
      public ProjectileWeight Weight => _weight;

      /// <summary>
      /// Whether the projectile has been launched yet.
      /// </summary>
      public bool IsFired => (_firedTime == -1f) ? false : true;

      /// <summary>
      /// Whether the projectile is ready for explosion.
      /// </summary>
      public bool IsArmed => _isArmed;

      /// <summary>
      /// Whether this specific projectile is locked and loaded and ready to go as a missile.
      /// </summary>
      public bool IsNextToFire { get; set; } = false;

      #endregion


      #region Unity Methods

      protected void Awake()
      {
         _rigidbody = GetComponentInParent<Rigidbody>(); // The vehicle's rigidbody

         if (_rigidbody == null)
         {
            Debug.Log("Haha, " + gameObject + " is null rigid");
         }

         _fins = GetComponentsInChildren<SimpleWing>(true).ToList<SimpleWing>();
         foreach (SimpleWing fin in _fins)
         {
            fin.enabled = false;
         }
      }

      protected void Start()
      {
         _shortName = _shortName ?? Multilang.Text["projectile_short"];
      }

      protected void FixedUpdate()
      {
         if (IsFired)
            Gravity.Gravitate(_rigidbody);
      }

      #endregion


      #region Public Methods

      /// <summary>
      /// Main method to fire any projectile.
      /// Detach the projectile and add a rigidbody to it according to its weight, with an initial impulse equal to the 
      /// original speed of the carrying vehicle.
      /// </summary>
      public virtual bool Fire()
      {
         // Get the current speed, location, and rotation
         Vector3 velocity = _rigidbody.velocity;               // The vehicle's speed
         Vector3 angularVelocity = _rigidbody.angularVelocity; // The vehicle's turn rate

         // Detach the projectile game object
         transform.parent = null;
         gameObject.transform.parent = null;

         // Add a rigidbody to the game object
         gameObject.AddComponent<Rigidbody>(); // Change rigidbody reference
         _rigidbody = gameObject.GetComponent<Rigidbody>();
         _rigidbody.useGravity = true;
         _rigidbody.isKinematic = false;
         _rigidbody.drag = _drag;
         _rigidbody.angularDrag = _drag;
         _rigidbody.mass = (float)_weight;
         _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
         _rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

         // Enable the fins and assign the rigidbody to them
         foreach (SimpleWing fin in _fins)
         {
            fin.enabled = true;
            fin.Rigidbody = _rigidbody;
         }

         // Transfer the vehicle's speed to the missile
         _rigidbody.AddForce(velocity, ForceMode.VelocityChange);
         _rigidbody.AddForce(angularVelocity, ForceMode.VelocityChange);

         // Apply the initial impulse force
         _rigidbody.AddRelativeForce(_impulse, ForceMode.VelocityChange);

         // Register the firing time (null if not fired)
         _firedTime = Time.time;

         return true;
      }

      public override string ToString()
      {
         return _shortName;
      }

      public static float TimeSince(float since)
      {
         return Time.time - since;
      }

      #endregion


      #region Fireable Interface

      public abstract bool LaunchAuthority { get; }

      public abstract float HitProbability { get; }

      #endregion


      #region Switchable Interface

      public bool IsOnOrSelected => IsNextToFire;

      public Switchable Switchable => _switchable;

      #endregion
   }
}