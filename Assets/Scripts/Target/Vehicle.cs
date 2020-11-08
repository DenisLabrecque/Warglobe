using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Warglobe
{
   /// <summary>
   /// This class is inherited by all vehicle types. It sets their main components, such as cost, physical density, and camera emplacements.
   /// </summary>
   public abstract class Vehicle : Target
   {

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
      [SerializeField] [Range(25000, 308000000)] int _dollarValue = 32000;

      [Tooltip("Density for flotation")]
      [SerializeField] [Range(0, 1)] float _percentDensity = 0.4f;

      [Header("Pre-game settings")]
      [SerializeField] [Range(0, 1500)] int _pregameSpeed = 0;

      List<CameraEmplacement> _cameraEmplacements;
      protected FlotationArea _flotationArea = null;
      protected Motor _motor;
      private int _currentCameraIndx = 0;
      protected List<ISwitchable> _switchables;
      protected Dictionary<Function, List<ISwitchable>> _switchablesByFunction = new Dictionary<Function, List<ISwitchable>>();

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
            return transform.InverseTransformDirection(_rigidbody.velocity).z;
         }
      }

      public Motor Motor {
         get {
            return _motor;
         }
      }

      /// <summary>
      /// Return the vertical speed (for g-force and such)
      /// </summary>
      public float VerticalSpeed {
         get {
            return transform.InverseTransformDirection(_rigidbody.velocity).y;
         }
      }

      /// <summary>
      /// Return sideways speed (for slip)
      /// </summary>
      public float LateralSpeed {
         get {
            return transform.InverseTransformDirection(_rigidbody.velocity).x;
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
            return _rigidbody.velocity.normalized;
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
         return _rigidbody.transform.position + _rigidbody.velocity * seconds;
         //return new Vector3(m_Rigidbody.velocity.x * seconds, m_Rigidbody.velocity.y * seconds, m_Rigidbody.velocity.z * seconds);
      }

      /// <summary>
      /// Gets all switchables.
      /// </summary>
      public Dictionary<Function, List<ISwitchable>> SwitchablesByFunction {
         get => _switchablesByFunction;
      }

      #endregion


      #region Unity Methods

      protected void Awake()
      {
         base.Awake();

         // List children
         _cameraEmplacements = GetComponentsInChildren<CameraEmplacement>().ToList<CameraEmplacement>();
         _switchables = GetComponentsInChildren<ISwitchable>().ToList();
         foreach (ISwitchable switchable in _switchables)
         {
            if (switchable == null || switchable.Switchable == null)
               Debug.LogError("Switchable " + switchable.ToString() + " has not been assigned a switchable enum value and is null. Please fix for the HUD to work.");
            else
            {

               // Add the key if it doesn't exist
               if (_switchablesByFunction.ContainsKey(switchable.Switchable.Function) == false)
                  _switchablesByFunction.Add(switchable.Switchable.Function, new List<ISwitchable>());

               // Add the element
               _switchablesByFunction[switchable.Switchable.Function].Add(switchable);
            }
         }

         // Initialize hitpoints
         _currentHitpoints = _maxHitpoints;

         // Assign the child items
         _motor = GetComponentInChildren<Motor>();
         _flotationArea = GetComponentInChildren<FlotationArea>();

         _rigidbody.velocity = Vector3.forward * _pregameSpeed;

         // Drag
         _rigidbody.drag = AIR_DRAG;
         _rigidbody.angularDrag = AIR_DRAG;
      }

      protected void Start()
      {
         if (_cameraEmplacements.Count == 0 || _cameraEmplacements == null)
            Debug.LogError("Vehicle " + PopularName + " must absolutely have at least one camera emplacement as a child");
         else if (_motor == null)
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
         if (signedOne == 0)
            return;

         // Set the current camera in the list of camera emplacements
         _currentCameraIndx += signedOne;

         if (_currentCameraIndx < 0)
         {
            _currentCameraIndx = _cameraEmplacements.Count - 1;
         }
         else if (_currentCameraIndx > _cameraEmplacements.Count - 1)
         {
            _currentCameraIndx = 0;
         }

         // Attach the camera to the current emplacement
         var cameraEmplacement = _cameraEmplacements[_currentCameraIndx];
         cameraEmplacement.Attach(camera);
      }

      /// <summary>
      /// Attach the camera to this vehicle (so we be in vehicle view of this vehicle).
      /// Automatically go to the active camera emplacement child of this vehicle.
      /// </summary>
      /// <param name="camera">The camera to attach</param>
      public void AttachCamera(Camera camera)
      {
         _cameraEmplacements[_currentCameraIndx].Attach(camera);
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
         if (_flotationArea == null)
         {
            _rigidbody.angularDrag = RotationalSpeedDrag;
         }
         else
         {
            float waterDrag = _flotationArea.PercentSubmerged * WATER_DRAG;
            float airDrag = _flotationArea.PercentNotSubmerged * AIR_DRAG;
            float airRotationalDrag = _flotationArea.PercentNotSubmerged * RotationalSpeedDrag;

            _rigidbody.drag = waterDrag + airDrag;
            _rigidbody.angularDrag = waterDrag + airRotationalDrag;
         }
      }

      protected override void Kill()
      {
         base.Kill();
         _flotationArea.Sink();
      }

      #endregion
   }
}