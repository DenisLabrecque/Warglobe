using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Processes player commands globally throughout the game.
/// 
/// Denis Labrecque
/// October
/// </summary>
public class UserInput : MonoBehaviour {

   #region Key Binding Constants

   const string MINIMAP = "Map"; // Toggle minimap view
   const string CAMERA = "Camera"; // Cycle through camera views
   const string WEAPON = "Weapon"; // Cycle through weapons; -1 last weapon, 1 next weapon
   const string FIRE = "FireWeapon"; // Fire the selected weapon
   const string GUN = "FireGun"; // Fire the mounted gun
   const string RADAR = "Radar"; // Turn radar on/off
   const string GEAR = "Gear"; // Move landing gear up/down
   const string BRAKE = "Break"; // Apply resistance
   const string PITCH = "Vertical"; // Point up or down on an airplane, press gas on a vehicle; -1 (less) to 1 (more)
   const string PITCH2 = "Vertical2";
   const string ROLL = "Horizontal"; // Bank on an airplane, turn on a vehicle; -1 (left) to 1 (right)
   const string ROLL2 = "Horizontal2";
   const string YAW = "Sideways"; // Turn on an airplane; -1 (left) to 1 (right)
   const string THROTTLE = "Throttle"; // Throttle up on any vehicle -1 (less) to 1 (more)
   const string ROTATE_PLANET = "RotatePlanet"; // Go around the planet
   const string ZOOM = "Mouse ScrollWheel";
   const string PAUSE = "Pause";
   const string CHANGE_VEHICLE = "ChangeVehicle";
   const string NEXT_TARGET = "NextTarget";
   const string ENTER = "AcceptSelection";

   #endregion


   #region Member Variables

   [SerializeField] Vehicle m_Player1Vehicle;
   [SerializeField] Vehicle m_Player2Vehicle;
   [SerializeField] UIPanelController m_UIController;

   private bool m_MinimapView = false;
   private static bool m_IsGamePaused = false;
   private static UIPanelController ms_UIController;
   private static List<Vehicle> m_PlayableVehicles;
   private static int m_CurrentVehicleIndex;

   #endregion


   #region Properties

   /// <summary>
   /// Get the vehicle the user is currently controlling.
   /// </summary>
   public static Vehicle Player1Vehicle { get; private set; }
   public static Vehicle Player2Vehicle { get; private set; }

   /// <summary>
   /// Toggle the minimap view
   /// </summary>
   public static bool ToggleMinimap {
      get {
         return Input.GetButtonDown(MINIMAP) ? true : false;
      }
   }

   /// <summary>
   /// Cycle through cameras (-1, 0, or 1)
   /// </summary>
   public static int ChangeCameraView {
      get {
         if(Input.GetButtonDown(CAMERA))
         {
            return (int)Input.GetAxisRaw(CAMERA); // -1 or 1
         }
         else
         {
            return 0;
         }
      }
   }

   /// <summary>
   /// Cycle through vehicles (-1, 0, or 1)
   /// </summary>
   public static int ChangePlayableVehicle {
      get {
         if(Input.GetButtonDown(CHANGE_VEHICLE))
         {
            return (int)Input.GetAxisRaw(CHANGE_VEHICLE); // -1 or 1
         }
         else
         {
            return 0;
         }
      }
   }

   /// <summary>
   /// Cycle through weapons (-1, 0, or 1)
   /// </summary>
   public static int ChangeWeapon {
      get {
         if(Input.GetButtonDown(WEAPON))
         {
            return (int)Input.GetAxisRaw(WEAPON); // -1 or 1
         }
         else
         {
            return 0;
         }
      }
   }

   /// <summary>
   /// Cycle through targets (one-way)
   /// </summary>
   public static bool NextTarget {
      get {
         return Input.GetButtonDown(NEXT_TARGET) ? true : false;
      }
   }

   /// <summary>
   /// Accept selection (enter)
   /// </summary>
   public static bool Enter {
      get {
         return Input.GetButtonDown(ENTER) ? true : false;
      }
   }

   /// <summary>
   /// Fire the selected weapon
   /// </summary>
   public static bool Fire {
      get {
         return Input.GetButtonDown(FIRE) ? true : false;
      }
   }

   /// <summary>
   /// Fire the default embarked gun
   /// </summary>
   public static bool Gun {
      get {
         return (Input.GetAxis(GUN) == 1) ? true : false;
      }
   }

   /// <summary>
   /// Turn radar on/off
   /// </summary>
   public static bool Radar {
      get {
         return Input.GetButtonDown(RADAR) ? true : false;
      }
   }

   /// <summary>
   /// Move the gear up or down
   /// </summary>
   public static bool Gear {
      get {
         return Input.GetButtonDown(GEAR) ? true : false;
      }
   }

   /// <summary>
   /// Apply brakes
   /// </summary>
   public static bool Brake {
      get {
         return Input.GetButtonDown(BRAKE) ? true : false;
      }
   }

   /// <summary>
   /// Return a value from -1 to 1 (throttle)
   /// </summary>
   public static float Throttle {
      get {
         return Input.GetAxis(THROTTLE);
      }
   }

   /// <summary>
   /// Return a value from -1 to 1 (pitch)
   /// </summary>
   public static float Pitch {
      get {
         return Input.GetAxis(PITCH);
      }
   }
   public static float Pitch2 {
      get {
         return Input.GetAxis(PITCH2);
      }
   }

   /// <summary>
   /// Return a value from -1 to 1 (yaw)
   /// </summary>
   public static float Yaw {
      get {
         return Input.GetAxis(YAW);
      }
   }

   /// <summary>
   /// Return a value from -1 to 1 (roll)
   /// </summary>
   public static float Roll {
      get {
         return Input.GetAxis(ROLL);
      }
   }
   public static float Roll2 {
      get {
         return Input.GetAxis(ROLL2);
      }
   }

   /// <summary>
   /// Spin around an object with the scrollwheel mouse button
   /// </summary>
   public static bool MiddleMouseButton {
      get {
         return Input.GetMouseButton(2);
      }
   }

   /// <summary>
   /// Zoom with the mouse scroll wheel
   /// </summary>
   public static float ScrollWheel {
      get {
         return Input.GetAxis(ZOOM);
      }
   }

   /// <summary>
   /// Return -1 for left and 1 for right
   /// </summary>
   public static Vector3 MouseDirection {
      get {
         Vector3 mouseDirection = Input.mousePosition;
         mouseDirection.x -= Screen.width / 2;
         mouseDirection.y -= Screen.height / 2;
         return mouseDirection;
      }
   }

   /// <summary>
   /// Whether the pause button is pressed
   /// </summary>
   public static bool Pause {
      get {
         return Input.GetButtonDown(PAUSE) ? true : false;
      }
   }

   #endregion


   #region Unity Methods

   void Awake()
   {
      // Find all playable vehicles in the game
      m_PlayableVehicles = FindObjectsOfType<Vehicle>().ToList();
      m_CurrentVehicleIndex = m_PlayableVehicles.IndexOf(m_Player1Vehicle);

      // Find the vehicle the player is playing with
      Player1Vehicle = m_Player1Vehicle;
      Player2Vehicle = m_Player2Vehicle;

      ms_UIController = m_UIController;
   }

   void Start()
   {
      Player1Vehicle.AttachCamera(SingleCamera.Camera1);
      //Player2Vehicle.AttachCamera(SingleCamera.Camera2);
   }

   /// <summary>
   /// Process user input at every frame
   /// </summary>
   void Update()
   {
      // Pause or unpause
      if(Pause && !m_IsGamePaused)
      {
         PauseGame();
      }
      else if(Pause && m_IsGamePaused)
      {
         UnpauseGame();
      }


      // Change between the minimap view and the vehicle view when the user hits the shortcut button
      else if(ToggleMinimap)
      {
         // Get in vehicle view
         if(m_MinimapView == true)
         {
            Player1Vehicle.AttachCamera(SingleCamera.Camera1);
            m_MinimapView = false;
         }
         // Get in minimap view
         else
         {
            FindObjectOfType<Planet>().GetComponentInChildren<CameraEmplacement>().Attach(SingleCamera.Camera1);
            m_MinimapView = true;
         }
      }

      // Cycle camera views
      else if(ChangeCameraView != 0)
      {
         Player1Vehicle.ChangeCamera(ChangeCameraView, SingleCamera.Camera1);
      }

      // Cycle playable vehicles
      else if(ChangePlayableVehicle != 0)
      {
         // Increment the current vehicle index
         m_CurrentVehicleIndex++;
         if(m_PlayableVehicles.Count <= m_CurrentVehicleIndex)
            m_CurrentVehicleIndex = 0;

         // Change the current player vehicle according to the current index
         Debug.Log("Changing vehicle to no " + m_CurrentVehicleIndex);
         Player1Vehicle = m_PlayableVehicles[m_CurrentVehicleIndex];
         Player1Vehicle.AttachCamera(SingleCamera.Camera1);
      }

      // Rotate the planet (if in planet view)
      else if(m_MinimapView == true)
      {
         //if(RotatePlanet == true)
            //SingleCamera.GameCam.GetComponentInParent<CameraEmplacement>().transform.RotateAround(Vector3.zero, Vector3.up, 30 * MouseDirection.x / Screen.width * Time.deltaTime);
      }
   }

   #endregion


   #region Public Methods

   public static void PauseGame()
   {
      ms_UIController.ShowCanvasGroup(0);
      Time.timeScale = 0;
      m_IsGamePaused = true;
   }

   public static void UnpauseGame()
   {
      ms_UIController.HideAllCanvasses();
      Time.timeScale = 1;
      m_IsGamePaused = false;
   }

   #endregion
}
