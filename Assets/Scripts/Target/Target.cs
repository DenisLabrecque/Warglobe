using System;
using System.Collections.Generic;
using UnityEngine;
using DGL.Math;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

namespace Warglobe
{
   /// <summary>
   /// Abstraction that designates any potential target. All vehicles and military buildings inherit from this.
   /// Defines which country the target belongs to.
   /// For radar tracking purposes, every target requires a rigidbody. On buildings, that rigidbody must be set to kinematic so it's not affected by gravity.
   /// 
   /// Denis Labrecque
   /// November 2018, December 2018, September 2020
   /// </summary>
   [RequireComponent(typeof(Rigidbody))]
   public abstract class Target : MonoBehaviour, IComparable<Target>
   {

      #region Member Variables

      [Header("Target metadata")]
      [Tooltip("Which country this target belongs to")]
      [SerializeField] public Faction.CountryName _countryName;

      [Tooltip("A vehicle's marketing name; should be capitalized appropriately. This is the name most people call a vehicle (eg. Raptor, Hornet, Mustang)")]
      [SerializeField] string _popularName = "Target";

      [Tooltip("Radar return")]
      [Range(0f, 1.0f)]
      [SerializeField] float _radarCrossSection = 1f;

      [Tooltip("Hitpoints")]
      [SerializeField] [Range(1, 50000)] protected uint _maxHitpoints = 20000;

      protected WeaponSystem _weaponSystem;
      protected SensorSystem _sensorSystem;
      protected float _currentHitpoints;
      protected Faction _faction;
      protected Rigidbody _rigidbody;

      #endregion


      #region Properties

      public Rigidbody Rigidbody => _rigidbody;

      /// <summary>
      /// Whether a target has a weapons system or not.
      /// </summary>
      public bool IsMilitary => _weaponSystem != null;

      /// <summary>
      /// The total hitpoints the target has.
      /// </summary>
      public float MaxHitpoints => _maxHitpoints;

      /// <summary>
      /// The hitpoints that the target currently has.
      /// </summary>
      public float Hitpoints {
         get {
            return _currentHitpoints;
         }
         set {
            if (value > _maxHitpoints)
               throw new ArgumentOutOfRangeException("Hitpoints cannot be more than the maximum hitpoints allowed");
            _currentHitpoints = (int)value;
         }
      }

      /// <summary>
      /// Current hitpoints over the maximum hitpoints, from 0 to 1.
      /// </summary>
      public float HitpointPercent => Utility.Percent(_currentHitpoints, _maxHitpoints, PercentMode.Clamp0To1);

      /// <summary>
      /// Whether the target has no hitpoints.
      /// </summary>
      public bool IsDead => _currentHitpoints <= 0;

      /// <summary>
      /// Whether the target has enough hitpoints to be alive.
      /// </summary>
      public bool IsAlive => _currentHitpoints > 0;

      /// <summary>
      /// Get the weapons. Can return null if this target does not have a weapon system.
      /// </summary>
      public WeaponSystem WeaponSystem => _weaponSystem;

      /// <summary>
      /// Get the sensors. Can return null if there are no sensors.
      /// </summary>
      public SensorSystem SensorSystem => _sensorSystem;

      /// <summary>
      /// A smaller RCS shortens the percent normal range an aircraft can be detected using radar.
      /// </summary>
      public float RadarCrossSection => _radarCrossSection;

      /// <summary>
      /// Target's country
      /// </summary>
      public Faction Faction => _faction;

      /// <summary>
      /// Target's country name
      /// </summary>
      public Faction.CountryName CountryName => _countryName;

      /// <summary>
      /// Target's popular name
      /// </summary>
      public string PopularName => _popularName;

      #endregion


      #region Standard Methods

      //protected override void NetworkStart()
      //{
      //   base.NetworkStart();


      //   if(!networkObject.IsOwner)
      //   {
      //      // this network object is the enemy player, not the one we control
      //   }
      //}

      protected void Awake()
      {

         // Assign the manually selected country to this target
         _faction = Faction.m_CountryList[_countryName];

         // Weapon system
         if (GetComponentInChildren<WeaponSystem>() != null)
            _weaponSystem = GetComponentInChildren<WeaponSystem>();
         else
            _weaponSystem = null;

         _rigidbody = GetComponent<Rigidbody>();

         // Sensor system
         if (GetComponentInChildren<SensorSystem>() != null)
         {
            _sensorSystem = GetComponentInChildren<SensorSystem>();
            //         Debug.Log("Sensor system found on " + PopularName);
         }
         else
         {
            _sensorSystem = null;
            //         Debug.Log("No sensor system found on " + PopularName);
         }
      }

      private void Update()
      {
         //// Server (owner)
         //if (networkObject.IsServer)
         //{
         //   // Send this data over the network
         //   networkObject.position = transform.position;
         //   networkObject.rotation = transform.rotation;
         //}
         //// Client
         //else
         //{
         //   // Get data from the server
         //   transform.position = networkObject.position;
         //   transform.rotation = networkObject.rotation;

         //   return;
         //}
      }

      #endregion


      #region Public Methods

      /// <summary>
      /// General attack method that subtracts hitpoints from the current target.
      /// </summary>
      public virtual void Damage(float subtract)
      {
         if (_currentHitpoints > 0)
         {
            _currentHitpoints -= subtract;
            if (_currentHitpoints <= 0)
            {
               _currentHitpoints = 0;
               Kill();
            }
         }
      }

      /// <summary>
      /// Do whatever it takes to kill this target.
      /// </summary>
      protected virtual void Kill()
      {
         if (_weaponSystem != null)
            _weaponSystem.enabled = false;
         if (_sensorSystem != null)
            _sensorSystem.enabled = false;
      }

      /// <summary>
      /// Determine this target's relationship to another target
      /// </summary>
      /// <param name="target">The target to compare to</param>
      /// <returns>Friend, foe, or neutral identification</returns>
      public Faction.Identification Relationship(Target target)
      {
         return _faction.Relationship(target.Faction);
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
      Target _seeker = null;

      /// <summary>
      /// Constructor of a sorted set of targets that are compared to a specific base target's distance for tracking.
      /// TODO looks incomplete
      /// </summary>
      /// <param name="seeker">The target that all distances will be compared to.</param>
      public BySeekerDistance(Target seeker)
      {
         _seeker = seeker;
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
         int distanceX = _seeker.CompareTo(x);
         int distanceY = _seeker.CompareTo(y);
         return distanceX - distanceY;
      }
   }
}