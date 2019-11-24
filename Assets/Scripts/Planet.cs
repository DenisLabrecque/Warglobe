using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Confirms that this game object is a planet, allowing measurements like altitude, distance, etc.
/// Requires that this game object have a flotation script parented to it (as an empty sub-object with a sphere collider).
/// </summary>
[ExecuteAlways]
public class Planet : MonoBehaviour {

   public const int MAX_ATMOSPHERE = 10000;

   #region Member Variables

   float m_AtmosphereHeight;

   #endregion

   #region Properties

   public static Planet Singleton {
      get;
      private set; }

   /// <summary>
   /// Get how distant the planet's sea is from the center
   /// </summary>
   public float SeaRadius { get; private set; }

   /// <summary>
   /// Get where the planet is located
   /// </summary>
   public Transform Center {
      get {
         return gameObject.transform;
      }
   }

   #endregion


   #region Unity Methods

   /// <summary>
   /// Save a reference to the singleton instance
   /// </summary>
   void Awake()
   {
      // Instantiate the singleton reference
      if(Singleton == null)
         Singleton = this;
   }

   void Start()
   {
      SeaRadius = gameObject.GetComponentInChildren<SphereCollider>().radius;

      m_AtmosphereHeight = Gravity.GRAVITY_RADIUS - SeaRadius;
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Find an object's sea level altitude above this planet
   /// </summary>
   public float AltitudeAboveSea(GameObject vehicle) {
      return Vector3.Distance(Center.position, vehicle.transform.position) - SeaRadius;
   }

   /// <summary>
   /// Find how low or high a position is compared to sea level.
   /// </summary>
   /// <param name="position">The position above or below sea level</param>
   /// <returns>Above sea level is positive and below is negative.</returns>
   public float AltitudeAboveSea(Vector3 position)
   {
      return Vector3.Distance(Center.position, position) - SeaRadius;
   }

   public bool IsInWater(GameObject gameObject)
   {
      if(AltitudeAboveSea(gameObject) < 0)
      {
         return true;
      }
      else
      {
         return false;
      }
   }

   /// <summary>
   /// Find air density at a game object's altitude
   /// </summary>
   /// <returns>A percent</returns>
   public float AirDensity(GameObject vehicle)
   {
      return (1f - Mathf.Clamp(AltitudeAboveSea(vehicle), 0f, MAX_ATMOSPHERE) / MAX_ATMOSPHERE);
   }

   /// <summary>
   /// Find the angle a vehicle is rolled above the planet
   /// </summary>
   /// <param name="vehicle">The vehicle to measure the angle for</param>
   /// <returns>0 for flat, -90 for banking left, +90 for banked right right, and 180 for being upside down</returns>
   public float RollAngle(GameObject vehicle) {
      Vector3 towardsEarth = transform.position - vehicle.transform.position;
      Vector3 awayFromEarth = vehicle.transform.position - transform.position;
      return Vector3.SignedAngle(vehicle.transform.up, awayFromEarth, vehicle.transform.forward);
      // Mathf.Sign(Vector3.Dot(towardsEarth, vehicle.transform.right)) * 
   }

   /// <summary>
   /// Find the angle a vehicle is climbing
   /// </summary>
   /// <param name="vehicle">The vehicle</param>
   /// <returns>0 for pointing to the horizon, 90 for pointing directly upwards, and -90 for pointing directly downwards</returns>
   public float PitchAngle(GameObject vehicle)
   {
      Vector3 towardsEarth = vehicle.transform.position - transform.position;
      return -Vector3.Angle(vehicle.transform.forward, towardsEarth) + 90;
   }

   #endregion
}
