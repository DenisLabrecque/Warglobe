using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Defines a sensor/launch position/weapon system that can destroy targets by launching the weapons. Controls targets and targeting methods and 
/// displays these on the HUD.
/// All launch positions and acoustic locators should be children of this class.
/// A laser must be a child game object.
/// Creates a slot for each projectile type contained by the hardpoints. In this way, the user can select to fire projectiles by type,
/// and knows how many projectiles of each type are left to fire.
/// 
/// Denis Labrecque
/// January 2019
/// </summary>
public class WeaponSystem : MonoBehaviour {

   #region Member Variables

   SortedSet<Target> m_TrackingList = new SortedSet<Target>();
   SensorSystem m_SensorSystem;
   Laser m_Laser;
   List<Turret> m_Turrets = new List<Turret>();
   int m_CurrentProjectileIndex = 0;
   Dictionary<Type, ProjectileSlot> m_WeaponSlots = new Dictionary<Type, ProjectileSlot>();
   List<Type> m_ProjectileTypes = new List<Type>();

   #endregion


   #region Properties

   /// <summary>
   /// Return the currently selected projectile.
   /// </summary>
   public Projectile CurrentProjectile {
      get { return m_WeaponSlots[m_ProjectileTypes[m_CurrentProjectileIndex]].Projectile; }
   }

   /// <summary>
   /// Get the current slot, which holds information like the current projectile and the number of projectiles of the selected type.
   /// </summary>
   public ProjectileSlot CurrentSlot {
      get { return m_WeaponSlots[m_ProjectileTypes[m_CurrentProjectileIndex]]; }
   }

   /// <summary>
   /// Get the weapon system's eye and ears (a sensor system is required to exist).
   /// </summary>
   public SensorSystem SensorSystem {
      get { return m_SensorSystem; }
   }

   #endregion


   #region Unity Methods

   void Awake()
   {
      m_Laser = GetComponentInChildren<Laser>();
      m_Turrets = GetComponentsInChildren<Turret>().ToList();
      m_SensorSystem = transform.parent.GetComponentInChildren<SensorSystem>();

      if(m_SensorSystem == null)
         Debug.LogError("A weapon system requires a sensor system to function properly on " + transform.parent);
   }

   void Start()
   {
      // Create a new slot for each projectile type contained the hardpoints
      List<Hardpoint> hardpoints = GetComponentsInChildren<Hardpoint>().ToList();
      foreach(Hardpoint hardpoint in hardpoints)
      {
         if(hardpoint.Projectile != null) // This code must be in the start method
         {

            // See whether a slot was already created for the type of projectile in the hardpoint
            if(m_WeaponSlots.ContainsKey(hardpoint.Projectile.GetType()))
               m_WeaponSlots[hardpoint.Projectile.GetType()].Add(hardpoint);
            else
               m_WeaponSlots.Add(hardpoint.Projectile.GetType(), new ProjectileSlot(hardpoint));
         }
      }

      // Enumerate the available weapons
      m_ProjectileTypes = new List<Type>(m_WeaponSlots.Keys);
   }

   /// <summary>
   /// Fire the selected missile or bomb.
   /// </summary>
   public void FireProjectile()
   {
      m_WeaponSlots[m_ProjectileTypes[m_CurrentProjectileIndex]].Fire();
   }

   public void FireTurret()
   {
      Debug.Log("Turret " + gameObject.name + " fired");
      foreach(Turret turret in m_Turrets)
      {
         turret.Fire();
      }
   }

   /// <summary>
   /// Fire the laser
   /// </summary>
   public void FireLaser()
   {
      if(m_Laser != null)
      {
         m_Laser.Fire();
      }
      else
      {
         m_Laser.StopFire();
      }
   }

   /// <summary>
   /// Select the next available weapon
   /// </summary>
   public void NextProjectile(int increment)
   {
      // Next weapon
      if(increment > 0)
      {
         m_CurrentProjectileIndex++;
         if(m_CurrentProjectileIndex > m_WeaponSlots.Count - 1)
            m_CurrentProjectileIndex = 0;
      }

      // Previous weapon
      else if(increment < 0)
      {
         m_CurrentProjectileIndex--;
         if(m_CurrentProjectileIndex < 0)
            m_CurrentProjectileIndex = m_WeaponSlots.Count - 1;
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
      m_TrackingList.Clear();

      // Feed the laser the closest target (this can be null)
      if (m_Laser != null)
      {
         m_Laser.SetTarget(m_SensorSystem.TrackingTarget);
      }
      if(m_Turrets != null)
      {
         foreach(Turret turret in m_Turrets)
         {
            if(m_SensorSystem.TrackingTarget != null)
               turret.SetAimpoint(m_SensorSystem.TrackingTarget.transform.position);
         }
      }
      m_TrackingList.Add(m_SensorSystem.TrackingTarget);
   }

   #endregion


   #region Private Methods



   #endregion


   /// <summary>
   /// Lists all the hardpoints on an airplane that contain a certain projectile.
   /// </summary>
   public class ProjectileSlot
   {
      Projectile m_Projectile = null;
      List<Hardpoint> m_Hardpoints = new List<Hardpoint>();
      int m_NumberLeft = 0;

      #region Properties

      /// <summary>
      /// Return the projectile exemplar type contained in this slot
      /// </summary>
      public Projectile Projectile {
         get {
            return m_Projectile;
         }
      }

      /// <summary>
      /// The number of projectiles left for this projectile type slot.
      /// </summary>
      public int NumberLeft {
         get {
            return m_NumberLeft;
         }
      }

      #endregion


      #region Constructor

      /// <summary>
      /// Create an available weapon slot by passing a projectile that represents the type of the list of projectiles.
      /// </summary>
      public ProjectileSlot(Hardpoint hardpoint)
      {
         if(hardpoint.Projectile == null)
         {
            Debug.LogError("A slot cannot determine weapon types by using a hardpoint without a weapon");
            return;
         }
         else
         {
            m_Projectile = hardpoint.Projectile as Projectile;
            m_Hardpoints = new List<Hardpoint>();
            m_Hardpoints.Add(hardpoint);
            m_NumberLeft++;
         }
      }

      #endregion

      /// <summary>
      /// Add another hardpoint to this slot
      /// </summary>
      /// <param name="hardpoint">Hardpoint to be added</param>
      public void Add(Hardpoint hardpoint)
      {
         m_Hardpoints.Add(hardpoint);
         m_NumberLeft++;
      }

      /// <summary>
      /// Fire the next available projectile
      /// </summary>
      public void Fire()
      {
         foreach(Hardpoint hardpoint in m_Hardpoints)
         {
            if(hardpoint.IsArmed)
            {
               hardpoint.Launch();
               m_NumberLeft--;
               return;
            }
         }
      }
   }
}
