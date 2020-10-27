using Mathematics;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This archetype can handle input to an airplane's controls.
/// Control surfaces must be assigned manually.
/// There should be one tricycle gear as child.
/// There should be one motor as a child.
/// </summary>
public abstract class Airplane : Vehicle {

   #region Member Variables

   [Header("Airplane control surfaces")]
   [SerializeField] ControlSurface m_LeftAileron;
   [SerializeField] ControlSurface m_RightAileron;
   [SerializeField] ControlSurface m_RElevator;
   [SerializeField] ControlSurface m_LElevator;
   [SerializeField] ControlSurface m_Tail;
   [SerializeField] ControlSurface m_AirBrake;
   [SerializeField] AirplaneUndercarriage m_Undercarriage;

   [Header("Airplane speed drag")]
   [SerializeField][Range(0,1)] float m_MinWingLift = 0.05f; // Wing lift percent minimum

   [Header("At start")]
   [SerializeField] bool m_PreGearIn = true;
   [SerializeField] bool m_RadarOn = false;

   PIDController AltitudePID; // Whether to increase or decrease pitch
   PIDController PitchAnglePID; // Whether the pitch angle has been exceeded
   PIDController RollPID; // For AI control
   PIDController SpeedPID;

   List<SimpleWing> m_Wings;

   Action m_CurrentAction = Action.MaintainAltitude;
   TurnAngle m_CurrentAggressiveness = TurnAngle.RateHalf;

   #endregion


   #region Enumerations

   public enum TurnAngle { Shallow = 4, RateHalf = 13, Rate1 = 27, Turn1G = 45, Turn2G = 60, Max = 90 }
   public enum Altitude { Minimum = 100, VeryLow = 200, Low = 800, General = 1200, High = 2000, VeryHigh = 3200 }
   public enum Action { MaintainAltitude }

   #endregion


   #region Properties

   public Action CurrentAction {
      get { return m_CurrentAction; }
   }

   public TurnAngle Aggressiveness {
      get { return m_CurrentAggressiveness; }
   }

   /// <summary>
   /// Altitude this airplane is above its base planet's sea
   /// </summary>
   public float AltitudeAboveSea {
      get {
         return Planet.Singleton.AltitudeAboveSea(gameObject);
      }
   }

   /// <summary>
   /// Climb angle
   /// </summary>
   public float PitchAngle {
      get {
         return Planet.Singleton.PitchAngle(gameObject);
      }
   }

   /// <summary>
   /// Whether the airplane is upright or upside-down
   /// </summary>
   public bool IsUpright {
      get {
         if(Mathf.Abs(RollAngle) <= 90f)
            return true;
         else
            return false;
      }
   }

   #endregion


   #region Unity Methods

   /// <summary>
   /// Assign hierarchical components
   /// </summary>
   void Awake()
   {
      base.Awake();

      // Assign the child landing gear
      m_Undercarriage = GetComponentInChildren<AirplaneUndercarriage>();
      m_Undercarriage.Retract();

      // Assign all wings
      m_Wings = GetComponentsInChildren<SimpleWing>().ToList();

      AltitudePID = new PIDController(0.008f, 0.001f, 0.01f, 1, -1);
      PitchAnglePID = new PIDController(0.005f, 0.01f, 0.03f, 1, -1);
      RollPID = new PIDController(0.0005f, 0.0003f, 0.0003f, 1, -1);
      SpeedPID = new PIDController(0.05f, 0.03f, 0.03f, 1, -1);
   }

   /// <summary>
   /// Detect missing components.
   /// </summary>
   void Start()
   {
      base.Start();

      if(m_RightAileron == null)
         Debug.LogError("An airplane must have a right aileron");
      else if(m_LeftAileron == null)
         Debug.LogError("An airplane must have a left aileron");
      else if(m_RElevator == null)
         Debug.LogError("An airplane's elevator1 must be set");
      else if(m_LElevator == null)
         Debug.LogError("An airplane's elevator2 must be set");
      else if(m_Undercarriage == null)
         Debug.LogError("An airplane's landing gear must be assigned");
      else if(m_Tail == null)
         Debug.LogError("An airplane's tail control surface must be assigned");
   }

   void Update()
   {
      // Steer airplane through user input
      if(UserInput.Player1Vehicle == this || UserInput.Player2Vehicle == this)
      {
         HumanControl();
      }
      // Steer airplane through AI
      else
      {
         AIControl();
      }
   }

   void FixedUpdate()
   {
      Gravity.Gravitate(_Rigidbody);
      AdjustWingLift();
      AdjustDrag();
   }

   #endregion


   #region Private Methods

   /// <summary>
   /// Reduce lift and drag as speed increases.
   /// Help prevent jitter.
   /// </summary>
   private void AdjustWingLift()
   {
      float supersonicPercentInverse = 1f - PercentSupersonic; // Inverse percent

      foreach(SimpleWing wing in m_Wings)
      {
         // Make sure the wings have minimum lift
         if(supersonicPercentInverse < m_MinWingLift)
            supersonicPercentInverse = m_MinWingLift;

         wing.Lift = supersonicPercentInverse;
         wing.Drag = supersonicPercentInverse;
      }
   }

   #endregion


   #region Public Methods

   private void AIControl()
   {
      // Throttle up the engines
      SpeedPID.SetPoint = 450f;
      SpeedPID.ProcessVariable = ForwardSpeed;
      _motor.PercentThrottle = ((float)SpeedPID.ControlVariable(Time.deltaTime));
      //m_Motor.AdjustThrottle(0.75f);

      // Roll

      // Roll
      RollPID.SetPoint = 0;
      RollPID.ProcessVariable = RollAngle;
//         Debug.Log("Roll angle : " + RollAngle);

      float rollDeflection = (float)RollPID.ControlVariable(Time.deltaTime);
      if(!IsUpright)
      {
         rollDeflection = -rollDeflection;
      }
      m_LeftAileron.DeflectionPercent = -rollDeflection;
      m_RightAileron.DeflectionPercent = rollDeflection;

      // Climb/dive
      AltitudePID.SetPoint = 200f; // Altitude
      //if(_sensorSystem.ActiveSensors != null && _sensorSystem.ActiveSensors.IsOn)
      //   AltitudePID.ProcessVariable = _sensorSystem.ActiveSensors.Altitude;
      //else
         AltitudePID.ProcessVariable = AltitudeAboveSea;

      float pitchDeflection;
      if(IsUpright)
         pitchDeflection = (float)AltitudePID.ControlVariable(Time.deltaTime);
      else
         pitchDeflection = 0;


      // Limit climb angle
      PitchAnglePID.SetPoint = (double)m_CurrentAggressiveness;
      PitchAnglePID.ProcessVariable = PitchAngle;

      float pitchLimiter = (float)PitchAnglePID.ControlVariable(Time.deltaTime);

      m_RElevator.DeflectionPercent = m_LElevator.DeflectionPercent = pitchDeflection * pitchLimiter;
   }

   private void HumanControl()
   {
      // Throttle up the engines
      if(UserInput.Throttle != 0)
         _motor.PercentThrottle = (UserInput.Throttle);
      //SpeedPID.SetPoint = 275f;
      //SpeedPID.ProcessVariable = ForwardSpeed;
      //Debug.Log("PID SPEED: " + (float)SpeedPID.ControlVariable(Time.deltaTime));
      //m_Motor.AdjustThrottle((float)SpeedPID.ControlVariable(Time.deltaTime));

      // Climb/dive
      if(UserInput.Player1Vehicle == this)
      {
         m_RElevator.DeflectionPercent = -UserInput.Pitch;
         m_LElevator.DeflectionPercent = -UserInput.Pitch;
      }
      else
      {
         m_RElevator.DeflectionPercent = -UserInput.Pitch2;
         m_LElevator.DeflectionPercent = -UserInput.Pitch2;
      }


      // Roll
      if (UserInput.Player1Vehicle == this)
      {
         m_LeftAileron.DeflectionPercent = -UserInput.Roll;
         m_RightAileron.DeflectionPercent = UserInput.Roll;
      }
      else
      {
         m_LeftAileron.DeflectionPercent = -UserInput.Roll2;
         m_RightAileron.DeflectionPercent = UserInput.Roll2;
      }

      // Yaw
      m_Tail.DeflectionPercent = UserInput.Yaw; // Tail
      // Nosewheel
      m_Undercarriage.Steer(UserInput.Yaw);

      // Air Brakes
      if(UserInput.Brake)
      {
         if(m_AirBrake.DeflectionPercent == 0)
            m_AirBrake.DeflectionPercent = 1f;
         else
            m_AirBrake.DeflectionPercent = 0f;
      }

      // Landing gear
      if(UserInput.Gear)
      {
         if(m_Undercarriage.IsExtended)
         {
            m_Undercarriage.Retract();
            Debug.Log("Retracting gear of " + gameObject);
         }
         else
         {
            m_Undercarriage.Extend();
            Debug.Log("Extending gear of " + gameObject);
         }
      }

      // Fire laser
      if(UserInput.Gun)
      {
         m_WeaponSystem.FireLaser();
      }

      // Fire projectiles
      if(UserInput.Fire)
      {
         m_WeaponSystem.FireProjectile();
      }

      // Next projectile
      if(UserInput.ChangeWeapon != 0)
      {
         m_WeaponSystem.NextProjectile(UserInput.ChangeWeapon);
      }

      // Next/previous target selection
      if(UserInput.NextTarget)
      {
         Debug.Log("Next target");

         WeaponSystem.SensorSystem.NextTarget(); // Next target is either +1 for increment or -1 for decrement
      }
   }

   #endregion
}