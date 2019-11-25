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

   [SerializeField] protected Vector3 m_ThrustDirection = new Vector3(0, 0, 1);

   [Tooltip("Unitless thrust at maximum throttle")]
   [SerializeField] protected float m_ThrustToWeightRatio = 1f;

   [Tooltip("Battery total charge when full (unitless)")]
   [SerializeField] float m_BatteryDurationMinutes = 11.5f;

   [Tooltip("Current throttle (modified in realtime)")]
   [Range(0,1)] [SerializeField] float m_ThrottleInput = 0;

   [Header("Visible Fan Properties")]

   [SerializeField] Vector3 m_RotationAxis = Vector3.up;
   [SerializeField][Range(1, 5000)] float m_RPM = 1000;

   [Tooltip("The mesh/meshes that are propellers/fans")]
   [SerializeField] List<GameObject> m_Fans;

   [Header("Audio")]
   [SerializeField] float m_MaxPitch = 2.2f;
   
   float m_Volume;
   protected float m_CurrentBattery;
   protected float m_MaxThrust;

   // Set at start
   protected Rigidbody m_Rigidbody = null;
   AudioSource m_AudioSource;

   #endregion


   #region Properties

   /// <summary>
   /// Percent throttle
   /// </summary>
   public float CurrentThrottle {
      get {
         return m_ThrottleInput;
      }
   }

   /// <summary>
   /// Maximum power at 100% throttle.
   /// </summary>
   public float PowerMassRatio {
      get {
         return m_ThrustToWeightRatio;
      }
   }

   /// <summary>
   /// Power output
   /// </summary>
   public float CurrentThrust {
      get {
         return m_MaxThrust * m_ThrottleInput;
      }
   }

   /// <summary>
   /// Battery charge percent
   /// </summary>
   public float BatteryPercent {
      get {
         return DGL.Math.Utility.Percent(m_CurrentBattery, m_BatteryDurationMinutes);
      }
   }

   /// <summary>
   /// Battery charge in ...minutes left.
   /// </summary>
   public int BatteryAmperage {
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
         m_Rigidbody = GetComponentInParent<Rigidbody>();

      m_AudioSource = GetComponent<AudioSource>();
      m_AudioSource.loop = true;
      m_AudioSource.spatialBlend = 1;
      m_AudioSource.rolloffMode = AudioRolloffMode.Logarithmic;

      m_Volume = m_AudioSource.volume;
      m_CurrentBattery = m_BatteryDurationMinutes;
   }

   void Update()
   {
      if(m_IsEnabled)
      {
         m_MaxThrust = m_ThrustToWeightRatio * m_Rigidbody.mass * 400f;

         // Discharge the battery
         m_CurrentBattery -= CurrentThrottle * 0.0166667f * Time.deltaTime;
         if(m_CurrentBattery <= 0)
         {
            m_CurrentBattery = 0;
            m_AudioSource.volume = 0;
            m_IsEnabled = false;
         }
         else
         {
            // Sound volume
            if(m_ThrottleInput == 0f)
            {
               m_AudioSource.volume = 0;
            }
            else
            {
               m_AudioSource.volume = m_Volume;
            }

            // Sound pitch
            m_AudioSource.pitch = m_ThrottleInput * m_MaxPitch;

            // Make each fan turn
            foreach(GameObject part in m_Fans)
            {
               part.transform.Rotate(m_RotationAxis, CurrentThrottle * m_RPM * Time.deltaTime);
            }
         }
      }
      else
      {
         m_AudioSource.volume = 0;
      }
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Change the throttle according to the percent throttle wanted.
   /// Throttle change speed is controlled by the input manager, not in code.
   /// </summary>
   /// <param name="percentChangeWanted">A value from 0 to 1?</param>
   public void AdjustThrottle(float percentChangeWanted)
   {
      // Avoid input error
      m_ThrottleInput = Mathf.Clamp(percentChangeWanted, 0, 1f);
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
         m_Rigidbody = rigidbody;
      }
   }

   #endregion
}