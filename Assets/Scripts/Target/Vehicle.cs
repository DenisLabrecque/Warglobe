﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// This class is inherited by all vehicle types. It sets their main components, such as cost, physical density, and camera emplacements.
/// </summary>
public abstract class Vehicle : Target {

   #region Constants

   public const float AIR_DRAG = 0.25f; // Minimum drag in air
   public const float SUPERSONIC_DRAG = 7.0f; // Maximum rotational drag in air
   public const float WATER_DRAG = 5f;
   public const float SUPERSONIC_SPEED = 3000f; // Speed of sound, at which supersonic drag applies

   #endregion

   #region Member Variables

   // Non-modified shared values

   [Header("Vehicle data")]
   [Tooltip("Vehicle cost in dollars (not including components)")]
   [SerializeField][Range(25000,308000000)] int  m_DollarValue = 32000;
   
   [SerializeField] int m_MaxHitpoints = 1500;

   [Tooltip("Density for flotation")]
   [SerializeField]
   [Range(0,1)] float m_PercentDensity = 0.4f;

   [Header("Pre-game settings")]
   [SerializeField][Range(0,1500)] int m_PregameSpeed = 0;

   List<CameraEmplacement> m_CameraEmplacements;
   protected FlotationArea m_FlotationArea = null;
   protected Motor _motor;
   private int m_CurrentCameraIndex = 0;

   #endregion


   #region Enumerations

   public enum DrivingMode { General, Aggressive, Precise }

   #endregion


   #region Properties

   /// <summary>
   /// Return the forward speed
   /// </summary>
   public float ForwardSpeed {
      get {
         return transform.InverseTransformDirection(_Rigidbody.velocity).z;
      }
   }

   /// <summary>
   /// Current throttle percent
   /// </summary>
   public float Throttle {
      get { return _motor.PercentThrottle; }
   }

   /// <summary>
   /// Return the vertical speed (for g-force and such)
   /// </summary>
   public float VerticalSpeed {
      get {
         return transform.InverseTransformDirection(_Rigidbody.velocity).y;
      }
   }

   /// <summary>
   /// Return sideways speed (for slip)
   /// </summary>
   public float LateralSpeed {
      get {
         return transform.InverseTransformDirection(_Rigidbody.velocity).x;
      }
   }

   /// <summary>
   /// Return how alive a vehicle is as a percent of the original hitpoints.
   /// </summary>
   public float HitpointPercent {
      get {
         return m_CurrentHitpoints / m_MaxHitpoints;
      }
   }

   /// <summary>
   /// Return how alive a vehicle is according to the total hitpoints it has left.
   /// </summary>
   public float Hitpoints {
      get {
         return m_CurrentHitpoints;
      }
      set {
         m_CurrentHitpoints = (int)value;
      }
   }

   /// <summary>
   /// Motor battery charge percentage
   /// </summary>
   public float PercentBattery {
      get {
         return _motor.BatteryPercent;
      }
   }

   /// <summary>
   /// Motor battery charge in amperes
   /// </summary>
   public int BatteryAmperage {
      get {
         return _motor.BatteryMinutesLeft;
      }
   }

   /// <summary>
   /// Returned this rigidbody's normalized velocity
   /// </summary>
   public Vector3 NormalizedVelocity {
      get {
         return _Rigidbody.velocity.normalized;
      }
   }

   /// <summary>
   /// Vehicle's roll relative to a perpendicular to the earth's core
   /// </summary>
   public float RollAngle {
      get {
         return Planet.Singleton.RollAngle(gameObject);
      }
   }

   /// <summary>
   /// Get forward speed as a value of 0-1 to the supersonic speed.
   /// Useful for applying drag.
   /// </summary>
   public float PercentSupersonic {
      get {
         return Mathf.Clamp(ForwardSpeed, 0, SUPERSONIC_SPEED) / SUPERSONIC_SPEED;
      }
   }

   /// <summary>
   /// Rotational drag increases as speed increases towards supersonic.
   /// </summary>
   public float RotationalSpeedDrag {
      get {
         return Mathf.Clamp(SUPERSONIC_DRAG * PercentSupersonic, AIR_DRAG, SUPERSONIC_DRAG);
      }
   }

   /// <summary>
   /// Predicted position considering this vehicle's speed.
   /// </summary>
   /// <param name="seconds">Seconds in which to find the position.</param>
   /// <returns>World space position</returns>
   public Vector3 PositionInSeconds(float seconds)
   {
      //return m_Rigidbody.transform.position + m_Rigidbody.transform.InverseTransformDirection(m_Rigidbody.velocity) * seconds;
      return _Rigidbody.transform.position + _Rigidbody.velocity * seconds;
      //return new Vector3(m_Rigidbody.velocity.x * seconds, m_Rigidbody.velocity.y * seconds, m_Rigidbody.velocity.z * seconds);
   }

   #endregion


   #region Unity Methods

   protected void Awake()
   {
      base.Awake();

      // List camera emplacements
      m_CameraEmplacements = GetComponentsInChildren<CameraEmplacement>().ToList<CameraEmplacement>();

      // Initialize hitpoints
      m_CurrentHitpoints = m_MaxHitpoints;

      // Assign the child items
      _motor = GetComponentInChildren<Motor>();
      m_FlotationArea = GetComponentInChildren<FlotationArea>();

      _Rigidbody.velocity = Vector3.forward * m_PregameSpeed;

      // Drag
      _Rigidbody.drag = AIR_DRAG;
      _Rigidbody.angularDrag = AIR_DRAG;
   }

   protected void Start()
   {
      if(m_CameraEmplacements.Count == 0 || m_CameraEmplacements == null)
         Debug.LogError("Vehicle " + PopularName + " must absolutely have at least one camera emplacement as a child");
      else if(_motor == null)
         Debug.LogError("Vehicle " + PopularName + " must have a motor");
   }

   protected void Update()
   {
      if (UserInput.Radar)
      {
         _sensorSystem.SwitchActiveSensors();
      }
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Move to the next available camera emplacement.
   /// </summary>
   public void ChangeCamera(int signedOne, Camera camera)
   {
      if(signedOne == 0)
         return;

      // Set the current camera in the list of camera emplacements
      m_CurrentCameraIndex += signedOne;

      if(m_CurrentCameraIndex < 0)
      {
         m_CurrentCameraIndex = m_CameraEmplacements.Count -1;
      }
      else if(m_CurrentCameraIndex > m_CameraEmplacements.Count -1)
      {
         m_CurrentCameraIndex = 0;
      }

      // Attach the camera to the current emplacement
      var cameraEmplacement = m_CameraEmplacements[m_CurrentCameraIndex];
      cameraEmplacement.Attach(camera);
   }

   /// <summary>
   /// Attach the camera to this vehicle (so we be in vehicle view of this vehicle).
   /// Automatically go to the active camera emplacement child of this vehicle.
   /// </summary>
   /// <param name="camera">The camera to attach</param>
   public void AttachCamera(Camera camera)
   {
      m_CameraEmplacements[m_CurrentCameraIndex].Attach(camera);
   }

   /// <summary>
   /// Overridden string method that gives the vehicle name.
   /// </summary>
   /// <returns>The vehicle subtype name.</returns>
   public override string ToString()
   {
      return GetType().Name;
   }

   /// <summary>
   /// Increase drag until it approaches supersonic speed.
   /// Adjust drag when underwater.
   /// </summary>
   protected void AdjustDrag()
   {
      if (m_FlotationArea == null)
      {
         _Rigidbody.angularDrag = RotationalSpeedDrag;
      }
      else
      {
         float waterDrag = m_FlotationArea.PercentSubmerged * WATER_DRAG;
         float airDrag = m_FlotationArea.PercentNotSubmerged * AIR_DRAG;
         float airRotationalDrag = m_FlotationArea.PercentNotSubmerged * RotationalSpeedDrag;

         _Rigidbody.drag = waterDrag + airDrag;
         _Rigidbody.angularDrag = waterDrag + airRotationalDrag;
      }
   }

   protected override void Kill()
   {
      base.Kill();
      m_FlotationArea.Sink();
   }

   #endregion
}
