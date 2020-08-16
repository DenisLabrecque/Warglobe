using DGL;
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
   [SerializeField] protected bool m_IsEnabled = true;

   [Tooltip("Battery total charge when full (unitless)")]
   [SerializeField] float _powerDurationMinutes = 11.5f;

   [Header("Power")]
   [SerializeField] protected Vector3 _thrustDirection = new Vector3(0, 0, 1);

   [Tooltip("Thrust to weight ratio of power at maximum throttle")]
   [SerializeField] protected float _thrustToWeight = 0.2f;

   [Tooltip("Percent of total thrust forward thrust that can be applied going backwards")]
   [SerializeField] [Range(0, 1)] protected float m_BackwardsPercentPower = 0.5f;

   [Tooltip("How quickly thrust reacts to inputs")]
   [SerializeField][Range(0, 1)] protected float m_ThrustChangeSpeed = 0.5f;

   [Tooltip("Current throttle (modified in realtime)")]
   [Range(-1,1)] [SerializeField] float m_ThrottleInput = 0; // The throttle setting that has been ordered

   [Header("Visible Fan Properties")]

   [SerializeField] Vector3 m_RotationAxis = Vector3.up;
   [SerializeField][Range(1, 5000)] float m_RPM = 1000;

   [Tooltip("The mesh/meshes that are propellers/fans")]
   [SerializeField] List<GameObject> m_Fans;

   [Header("Audio")]
   [SerializeField] float m_MaxPitch = 2.2f;
   
   float m_Volume;
   protected float m_ActualThrottle; // The throttle setting the engine has had time to reach
   protected float m_CurrentBattery;
   protected float m_MaxThrust;

   // Set at start
   protected Rigidbody _rigidbody = null;
   AudioSource _audioSource;
   private readonly float m_1_60th = 0.0166667f;

   #endregion


   #region Properties

   /// <summary>
   /// Percent throttle that the motor has gotten to.
   /// </summary>
   public float ActualThrottle {
      get {
         return m_ActualThrottle;
      }
   }

   /// <summary>
   /// Percent throttle that the motor was ordered to get to.
   /// </summary>
   public float InputThrottle {
      get {
         return m_ThrottleInput;
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
   /// Power output
   /// </summary>
   public float CurrentThrust {
      get {
         return m_ActualThrottle * m_MaxThrust;
      }
   }

   /// <summary>
   /// Battery charge percent
   /// </summary>
   public float BatteryPercent {
      get {
         return DGL.Math.Utility.Percent(m_CurrentBattery, _powerDurationMinutes);
      }
   }

   /// <summary>
   /// Battery charge in number of minutes left.
   /// </summary>
   public int BatteryMinutesLeft {
      get {
         return (int)m_CurrentBattery;
      }
   }

   /// <summary>
   /// Whether the engine is running currently, and a rigidbody has been assigned to it.
   /// </summary>
   public bool IsEnabled {
      get {
         return m_IsEnabled;
      }
      set {
         m_IsEnabled = false;
      }
   }

   #endregion


   #region Unity Methods

   void Awake()
   {
      if(m_IsEnabled)
         _rigidbody = GetComponentInParent<Rigidbody>();

      _audioSource = GetComponent<AudioSource>();
      _audioSource.loop = true;
      _audioSource.spatialBlend = 1;
      _audioSource.rolloffMode = AudioRolloffMode.Linear;

      m_ActualThrottle = m_ThrottleInput;
      m_Volume = _audioSource.volume;
      m_CurrentBattery = _powerDurationMinutes;
   }

   void Update()
   {
      if(m_IsEnabled)
      {
         // Move thrust to desired throttle
         m_ActualThrottle = Mathf.Lerp(m_ActualThrottle, m_ThrottleInput, m_ThrustChangeSpeed * Time.deltaTime);

         // Mass can change
         m_MaxThrust = _thrustToWeight * _rigidbody.mass * 400f;

         // Discharge the battery
         m_CurrentBattery -= m_ActualThrottle * m_1_60th * Time.deltaTime;
         if(m_CurrentBattery <= 0)
         {
            m_CurrentBattery = 0;
            _audioSource.volume = 0;
            m_IsEnabled = false;
         }
         else
         {
            // Sound volume
            if(m_ActualThrottle == 0f)
            {
               _audioSource.volume = 0;
            }
            else
            {
               _audioSource.volume = m_Volume;
            }

            // Sound pitch
            _audioSource.pitch = m_ActualThrottle * m_MaxPitch;

            // Make each fan turn
            foreach(GameObject part in m_Fans)
            {
               part.transform.Rotate(m_RotationAxis, m_ActualThrottle * m_RPM * Time.deltaTime);
            }
         }
      }
      else
      {
         _audioSource.volume = 0;
      }
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Change the throttle according to the percent throttle wanted.
   /// Throttle change speed is controlled by the input manager, not in code.
   /// </summary>
   /// <param name="percentChangeWanted">A value from -1 to 1</param>
   public void AdjustThrottle(float percentChangeWanted)
   {
      // Avoid input error
      m_ThrottleInput = Mathf.Clamp(percentChangeWanted, -1f, 1f);
   }

   /// <summary>
   /// Start the engine, and assign a rigidbody on which the engine must operate.
   /// </summary>
   /// <param name="rigidbody">The body the engine must propel.</param>
   public void StartMotor(Rigidbody rigidbody)
   {
      if(m_CurrentBattery >= 0)
      {
         m_IsEnabled = true;
         _rigidbody = rigidbody;
      }
   }

   #endregion
}