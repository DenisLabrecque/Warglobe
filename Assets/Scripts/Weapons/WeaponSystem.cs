using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Rendering.HighDefinition;

namespace Warglobe
{
   /// <summary>
   /// Defines a sensor/launch position/weapon system that can destroy targets by launching the weapons. Controls targets and targeting methods and 
   /// displays these on the HUD.
   /// All launch positions and acoustic locators should be children of this class.
   /// A laser must be a child game object.
   /// Creates a slot for each projectile type contained by the hardpoints. In this way, the user can select to fire projectiles by type,
   /// and knows how many projectiles of each type are left to fire.
   /// 
   /// This will not work until fixed!
   /// 
   /// Denis Labrecque
   /// January 2019
   /// </summary>
   public class WeaponSystem : MonoBehaviour
   {
      #region Member Variables

      [SerializeField] float _aimDistance = 500f;

      SortedSet<Target> _trackingList = new SortedSet<Target>();
      SensorSystem _sensorSystem;
      Laser _laser;
      List<Turret> _turrets = new List<Turret>();
      int _currentProjectileIndex = 0;
      Dictionary<Type, ProjectileSlot> _weaponSlots = new Dictionary<Type, ProjectileSlot>();
      List<Type> _projectileTypes = new List<Type>();

      #endregion


      #region Properties
            
      /// <summary>
      /// Where the mouse is pointing from a projection plane.
      /// </summary>
      public Vector3 AimPoint {
         get {
            return SingleCamera.Camera1.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _aimDistance));
         }
      }

      public bool HasTurret {
         get {
            return _turrets.Any();
         }
      }

      /// <summary>
      /// Return the currently selected projectile.
      /// </summary>
      public Projectile CurrentProjectile {
         get {
            try
            {
               return _weaponSlots[_projectileTypes[_currentProjectileIndex]].Projectile;
            }
            catch
            {
               return null;
            }
         }
      }

      /// <summary>
      /// Get the current slot, which holds information like the current projectile and the number of projectiles of the selected type.
      /// </summary>
      public ProjectileSlot CurrentSlot {
         get { return _weaponSlots[_projectileTypes[_currentProjectileIndex]]; }
      }

      /// <summary>
      /// Get the weapon system's eye and ears (a sensor system is required to exist).
      /// </summary>
      public SensorSystem SensorSystem {
         get { return _sensorSystem; }
      }

      public IEnumerable<Turret> Turrets { get { return _turrets; } }

      #endregion


      #region Unity Methods

      void Awake()
      {
         _laser = GetComponentInChildren<Laser>(true);
         _turrets = GetComponentsInChildren<Turret>(true).ToList();
         _sensorSystem = transform.parent.GetComponentInChildren<SensorSystem>(true);

         if (_sensorSystem == null)
            Debug.LogWarning("A weapon system requires a sensor system to function properly on " + transform.parent);
      }

      void Start()
      {
         // Create a new slot for each projectile type contained the hardpoints
         List<Hardpoint> hardpoints = GetComponentsInChildren<Hardpoint>().ToList();
         foreach (Hardpoint hardpoint in hardpoints)
         {
            if (hardpoint.Projectile != null) // This code must be in the start method
            {

               // See whether a slot was already created for the type of projectile in the hardpoint
               if (_weaponSlots.ContainsKey(hardpoint.Projectile.GetType()))
                  _weaponSlots[hardpoint.Projectile.GetType()].Add(hardpoint);
               else
                  _weaponSlots.Add(hardpoint.Projectile.GetType(), new ProjectileSlot(hardpoint));
            }
         }

         // Enumerate the available weapons
         _projectileTypes = new List<Type>(_weaponSlots.Keys);
      }

      /// <summary>
      /// Fire the selected missile or bomb.
      /// </summary>
      public void FireProjectile()
      {
         _weaponSlots[_projectileTypes[_currentProjectileIndex]].Fire();
      }

      /// <summary>
      /// Fire the first available turret that is reloaded.
      /// Returns whether or not a turret has fired.
      /// </summary>
      public bool FireTurrets()
      {
         bool hasFired = false;

         foreach (Turret turret in _turrets)
         {
            hasFired = turret.Fire();
         }

         return hasFired;
      }

      /// <summary>
      /// Fire the laser
      /// </summary>
      public void FireLaser()
      {
         if (_laser != null)
         {
            _laser.Fire();
         }
         else
         {
            _laser.StopFire();
         }
      }

      /// <summary>
      /// Select the next available weapon
      /// </summary>
      public void NextProjectile(int increment)
      {
         // Next weapon
         if (increment > 0)
         {
            _currentProjectileIndex++;
            if (_currentProjectileIndex > _weaponSlots.Count - 1)
               _currentProjectileIndex = 0;
         }

         // Previous weapon
         else if (increment < 0)
         {
            _currentProjectileIndex--;
            if (_currentProjectileIndex < 0)
               _currentProjectileIndex = _weaponSlots.Count - 1;
         }

         // Increment input error
         else
         {
            Debug.LogError("The next or previous weapon was incremented by zero");
         }
      }

      void FixedUpdate()
      {
         // Empty the tracking list
         _trackingList.Clear();

         // Feed the laser the closest target (this can be null)
         if (_laser != null)
         {
            _laser.SetTarget(_sensorSystem.TrackingTarget);
         }
         if (_turrets != null)
         {
            foreach (Turret turret in _turrets)
            {
               if (UserInput.Player1Vehicle == gameObject) // TODO probably won't work
               {
                  // Automatic cheapo turret aim
                  if (_sensorSystem.TrackingTarget != null)
                     turret.AimPosition = _sensorSystem.TrackingTarget.transform.position;
               }
               else
               {
                  // By hand turret aim
                  turret.AimPosition = AimPoint;
               }
            }
         }

         //m_TrackingList.Add(m_SensorSystem.TrackingTarget); // TODO handle nulls
      }

      #endregion


      #region Private Methods



      #endregion


      /// <summary>
      /// Lists all the hardpoints on an airplane that contain a certain projectile.
      /// </summary>
      public class ProjectileSlot
      {
         Projectile _projectile = null;
         List<Hardpoint> _hardpoints = new List<Hardpoint>();
         int _numberLeft = 0;

         #region Properties

         /// <summary>
         /// Return the projectile exemplar type contained in this slot
         /// </summary>
         public Projectile Projectile {
            get {
               return _projectile;
            }
         }

         /// <summary>
         /// The number of projectiles left for this projectile type slot.
         /// </summary>
         public int NumberLeft {
            get {
               return _numberLeft;
            }
         }

         #endregion


         #region Constructor

         /// <summary>
         /// Create an available weapon slot by passing a projectile that represents the type of the list of projectiles.
         /// </summary>
         public ProjectileSlot(Hardpoint hardpoint)
         {
            if (hardpoint.Projectile == null)
            {
               Debug.LogError("A slot cannot determine weapon types by using a hardpoint without a weapon");
               return;
            }
            else
            {
               _projectile = hardpoint.Projectile as Projectile;
               _hardpoints = new List<Hardpoint>();
               _hardpoints.Add(hardpoint);
               _numberLeft++;
            }
         }

         #endregion

         /// <summary>
         /// Add another hardpoint to this slot
         /// </summary>
         /// <param name="hardpoint">Hardpoint to be added</param>
         public void Add(Hardpoint hardpoint)
         {
            _hardpoints.Add(hardpoint);
            _numberLeft++;
         }

         /// <summary>
         /// Fire the next available projectile
         /// </summary>
         public void Fire()
         {
            foreach (Hardpoint hardpoint in _hardpoints)
            {
               if (hardpoint.IsArmed)
               {
                  hardpoint.Launch();
                  _numberLeft--;
                  return;
               }
            }
         }
      }
   }
}