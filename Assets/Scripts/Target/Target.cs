using System;
using System.Collections.Generic;
using UnityEngine;
using DGL.Math;

/// <summary>
/// Abstraction that designates any potential target. All vehicles and military buildings inherit from this.
/// Defines which country the target belongs to.
/// For radar tracking purposes, every target requires a rigidbody. On buildings, that rigidbody must be set to kinematic so it's not affected by gravity.
/// 
/// Denis Labrecque
/// November 2018, December 2018
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public abstract class Target : MonoBehaviour, IComparable<Target>
{

   #region Member Variables

   [Header("Target metadata")]
   [Tooltip("Which country this target belongs to")]
   [SerializeField] public Faction.CountryName m_CountryName;

   [Tooltip("A vehicle's marketing name; should be capitalized appropriately. This is the name most people call a vehicle (eg. Raptor, Hornet, Mustang)")]
   [SerializeField] string m_GeneralName = "Target";

   [Tooltip("A general description of the target")]
   [SerializeField][TextArea] string m_Description = "A generic target.";

   [Tooltip("Radar return")]
   [Range(0f, 1.0f)]
   [SerializeField] float m_RadarCrossSection = 1f;

   [Tooltip("Hitpoints")]
   [SerializeField] [Range(0f, 50000f)] float m_Hitpoints;

   protected WeaponSystem m_WeaponSystem;
   protected SensorSystem m_SensorSystem;
   protected float m_CurrentHitpoints;
   protected Faction m_Country;
   protected Rigidbody m_Rigidbody;

   #endregion


   #region Properties

   public Rigidbody Rigidbody {
      get {
         return m_Rigidbody;
      }
   }

   /// <summary>
   /// Whether a target has a weapons controller or not.
   /// </summary>
   public bool IsMilitary {
      get {
         if(m_WeaponSystem == null)
            return false;
         else
            return true;
      }
   }

   /// <summary>
   /// The total hitpoints the target has.
   /// </summary>
   public float MaxHitpoints {
      get {
         return m_Hitpoints;
      }
   }

   /// <summary>
   /// The hitpoints that the target currently has.
   /// </summary>
   public float Hitpoints {
      get {
         return m_CurrentHitpoints;
      }
   }

   /// <summary>
   /// Current hitpoints over the maximum hitpoints (always between 0 and 1).
   /// </summary>
   public float PercentHitpoints {
      get {
         return Utility.Percent(m_CurrentHitpoints, m_Hitpoints, PercentMode.Clamp0To1);
      }
   }

   /// <summary>
   /// Whether the target has no hitpoints.
   /// </summary>
   public bool IsDead {
      get {
         if (m_CurrentHitpoints <= 0)
            return true;
         else
            return false;
      }
   }

   /// <summary>
   /// Whether the target has enough hitpoints to be alive.
   /// </summary>
   public bool IsAlive {
      get {
         if (m_CurrentHitpoints > 0)
            return true;
         else
            return false;
      }
   }

   /// <summary>
   /// Get the weapons. Can return null if this target does not have a weapon system.
   /// </summary>
   public WeaponSystem WeaponSystem { get { return m_WeaponSystem; } }

   /// <summary>
   /// Get the sensors. Can return null if there are no sensors.
   /// </summary>
   public SensorSystem SensorSystem { get { return m_SensorSystem; } }

   /// <summary>
   /// A smaller RCS shortens the percent normal range an aircraft can be detected using radar.
   /// </summary>
   public float RadarCrossSection {
      get {
         return m_RadarCrossSection;
      }
   }

   /// <summary>
   /// Target's country
   /// </summary>
   public Faction Country {
      get {
         return m_Country;
      }
   }

   /// <summary>
   /// Target's country name
   /// </summary>
   public Faction.CountryName CountryName {
      get {
         return m_CountryName;
      }
   }

   /// <summary>
   /// Target's popular name
   /// </summary>
   public string PopularName {
      get {
         return m_GeneralName;
      }
   }

   /// <summary>
   /// The objective here is to kill the target.
   /// </summary>
   public bool IsAccomplished {
      get {
         if (IsDead)
            return true;
         else
            return false;
      }
   }

   #endregion


   #region Unity Methods

   protected void Awake()
   {

      // Assign the manually selected country to this target
      m_Country = Faction.m_CountryList[m_CountryName];

      // Weapon system
      if(GetComponentInChildren<WeaponSystem>() != null)
         m_WeaponSystem = GetComponentInChildren<WeaponSystem>();
      else
         m_WeaponSystem = null;

      m_Rigidbody = GetComponent<Rigidbody>();

      // Sensor system
      if(GetComponentInChildren<SensorSystem>() != null)
      {
         m_SensorSystem = GetComponentInChildren<SensorSystem>();
//         Debug.Log("Sensor system found on " + PopularName);
      }
      else
      {
         m_SensorSystem = null;
//         Debug.Log("No sensor system found on " + PopularName);
      }
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// General attack method that subtracts hitpoints from the current target.
   /// </summary>
   public virtual void Damage(float subtract)
   {
      if (m_CurrentHitpoints > 0)
      {
         m_CurrentHitpoints -= subtract;
         if (m_CurrentHitpoints <= 0)
         {
            m_CurrentHitpoints = 0;
            Kill();
         }
      }
   }

   /// <summary>
   /// Do whatever it takes to kill this target.
   /// </summary>
   protected virtual void Kill()
   {
      if (m_WeaponSystem != null)
         m_WeaponSystem.enabled = false;
      if (m_SensorSystem != null)
         m_SensorSystem.enabled = false;
   }

   /// <summary>
   /// Determine this target's relationship to another target
   /// </summary>
   /// <param name="target">The target to compare to</param>
   /// <returns>Friend, foe, or neutral identification</returns>
   public Faction.Identification Relationship(Target target)
   {
      return m_Country.Relationship(target.Country);
   }

   /// <summary>
   /// Implement the comparable interface.
   /// </summary>
   /// <param name="other">The target to compare to.</param>
   /// <returns>The integer angle between two targets.</returns>
   public int CompareTo(Target other)
   {
      return (int)Vector3.Angle(transform.position, other.transform.position);
   }

   #endregion
}

/// <summary>
/// Defines a comparer that allows a sensor system order targets by distance from itself (the containing target).
/// </summary>
public class BySeekerDistance : IComparer<Target>
{
   /// <summary>
   /// The game object that all distances will be compared to.
   /// </summary>
   Target m_Seeker = null;

   /// <summary>
   /// Constructor of a sorted set of targets that are compared to a specific base target's distance for tracking.
   /// </summary>
   /// <param name="seeker">The target that all distances will be compared to.</param>
   public BySeekerDistance(Target seeker)
   {
      m_Seeker = seeker;
   }

   /// <summary>
   /// Implement the comparer interface.
   /// See which target is farther from the seeker.
   /// </summary>
   /// <param name="seeker">The vehicle currently tracking and setting distances.</param>
   /// <param name="target">The target to be ordered in the list.</param>
   /// <returns>A distance from the seeking target.</returns>
   public int Compare(Target x, Target y)
   {
      int distanceX = m_Seeker.CompareTo(x);
      int distanceY = m_Seeker.CompareTo(y);
      return distanceX - distanceY;
   }
}