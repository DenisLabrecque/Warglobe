using DGL;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Any type of motor.
/// Physics force calculations to be added to the child script on fixed update.
/// 
/// Denis Labrecque
/// January 2019
/// </summary>
[RequireComponent(typeof(AudioSource))]
public abstract class Motor : MonoBehaviour {

   #region Member Variables

   // Non-modified member variables
   [Header("Basic Motor Data")]

   [Tooltip("Whether the engine starts on awake. When the start method is called, the rigidbody is assigned. That way, rockets propel their own rigidbody.")]
   [SerializeField] protected bool _isEnabled = true;

   [Tooltip("Battery total charge when full (unitless)")]
   [SerializeField] float _powerDurationMinutes = 11.5f;

   [Header("Power")]
   [SerializeField] protected Vector3 _thrustDirection = new Vector3(0, 0, 1);

   [Tooltip("Thrust to weight ratio of power at maximum throttle")]
   [SerializeField] protected float _thrustToWeight = 0.2f;

   [Tooltip("Percent of total thrust forward thrust that can be applied going backwards")]
   [SerializeField] [Range(0, 1)] protected float _backwardsPercentPower = 0.5f;

   [Tooltip("How quickly thrust reacts to inputs")]
   [SerializeField][Range(0, 1)] protected float _acceleration = 0.5f;

   [Tooltip("Current throttle (modified in realtime)")]
   [Range(-1,1)] [SerializeField] float _percentThrottle = 0f; // Backing variable for throttle input

   [Header("Visible Fan Properties")]

   [SerializeField] Vector3 _rotationAxis = Vector3.up;
   [SerializeField][Range(1, 5000)] float _rpm = 1000;

   [Tooltip("The mesh/meshes that are propellers/fans")]
   [SerializeField] List<GameObject> m_Fans;

   [Header("Audio")]
   [SerializeField] float _maxPitch = 2.2f;
   
   float _volume;
   protected float _percentThrust; // The throttle setting the engine has had time to reach
   protected float _currentEnergy;
   protected float _maxThrust;

   // Set at start
   protected Rigidbody _rigidbody = null;
   AudioSource _audioSource;
   private readonly float m_1_60th = 0.0166667f;
   public const float ReverseSeconds = 1f;
   private float _seconds = 0f;
   private float _reverseSeconds = 0f;
   private float _valueAtReverseBoundary;

   #endregion


   #region Properties

   /// <summary>
   /// Percent throttle that the motor was ordered to get to.
   /// Though this will accept larger/smaller values, they will always be returned clamped from -1 to 1.
   /// </summary>
   public float PercentThrottle {
      set {
         _percentThrottle = Mathf.Clamp(value, -1f, 1f);
      }
      get {
         return _percentThrottle;
      }
   }

   /// <summary>
   /// Maximum power at 100% throttle.
   /// </summary>
   public float PowerMassRatio {
      get {
         return _thrustToWeight;
      }
   }

   /// <summary>
   /// Current power output
   /// </summary>
   public float Thrust {
      get {
         return _percentThrust * _maxThrust;
      }
   }

   /// <summary>
   /// Percent of propulsion that the motor has been able to attain considering its rev up speed.
   /// From -1 to 1.
   /// </summary>
   public float PercentThrust {
      get {
         return _percentThrust;
      }
   }

   /// <summary>
   /// Battery charge percent
   /// </summary>
   public float BatteryPercent {
      get {
         return DGL.Math.Utility.Percent(_currentEnergy, _powerDurationMinutes);
      }
   }

   /// <summary>
   /// Battery charge in number of minutes left.
   /// </summary>
   public int BatteryMinutesLeft {
      get {
         return (int)_currentEnergy;
      }
   }

   /// <summary>
   /// Whether the engine is running currently, and a rigidbody has been assigned to it.
   /// </summary>
   public bool IsEnabled {
      get {
         return _isEnabled;
      }
      set {
         _isEnabled = false;
      }
   }

   #endregion


   #region Unity Methods

   void Awake()
   {
      if(_isEnabled)
         _rigidbody = GetComponentInParent<Rigidbody>();

      _audioSource = GetComponent<AudioSource>();
      _audioSource.loop = true;
      _audioSource.spatialBlend = 1;
      _audioSource.rolloffMode = AudioRolloffMode.Linear;

      _volume = _audioSource.volume;
      _currentEnergy = _powerDurationMinutes;
   }

   void Update()
   {
      if(_isEnabled)
      {
         // Move thrust to desired throttle
         _percentThrust = Mathf.Lerp(_percentThrust, _percentThrottle, _acceleration * Time.deltaTime);

         // Mass can change
         _maxThrust = _thrustToWeight * _rigidbody.mass * 400f;

         // Discharge the battery
         _currentEnergy -= _percentThrust * m_1_60th * Time.deltaTime;
         if(_currentEnergy <= 0)
         {
            _currentEnergy = 0;
            _audioSource.volume = 0;
            _isEnabled = false;
         }
         else
         {
            // Sound volume
            if(_percentThrust == 0f)
               _audioSource.volume = 0;
            else
               _audioSource.volume = _volume;

            // Sound pitch
            _audioSource.pitch = _percentThrust * _maxPitch;

            // Make each fan turn
            foreach(GameObject part in m_Fans)
            {
               try
               {
                  part.transform.Rotate(_rotationAxis, _percentThrust * _rpm * Time.deltaTime);
               }
               catch(Exception)
               {
                  Debug.LogWarning("Null reference exception when rotating engine fans");
               }
            }
         }

         // Update timer
         _seconds += Time.deltaTime;
      }
      else
      {
         _audioSource.volume = 0;
      }
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Start the engine, and assign a rigidbody on which the engine must operate.
   /// </summary>
   /// <param name="rigidbody">The body the engine must propel.</param>
   public void StartMotor(Rigidbody rigidbody)
   {
      if(_currentEnergy >= 0)
      {
         _isEnabled = true;
         _rigidbody = rigidbody;
      }
   }

   #endregion
}