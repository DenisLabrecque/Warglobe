using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handle aircraft input for landing gear automatically.
/// Specific types of landing gear can be implemented (eg. tricycle, tricycle with arrestor hook, taildragger, etc.)
/// 
/// Denis Labrecque
/// November 2018
/// </summary>
public class AirplaneUndercarriage : MonoBehaviour
{

   #region Member Variables

   [SerializeField] List<LandingGear> m_LandingGears;

   #endregion


   #region Properties

   /// <summary>
   /// Whether or not each landing gear is extended, ready for landing.
   /// </summary>
   public bool IsExtended {
      get {
         bool undercarriageIsExtended = true;
         foreach(LandingGear gear in m_LandingGears)
         {
            if(gear.IsExtended == false)
            {
               undercarriageIsExtended = false;
               break;
            }
         }
         return undercarriageIsExtended;
      }
   }

   #endregion


   #region Unity Methods

   // Check for missing editor assignments
   void Start()
   {
      foreach(LandingGear gear in m_LandingGears)
      {
         if(gear == null)
         {
            Debug.LogError("A landing gear was not assigned to this airplane undercarriage! Please fill in the array.");
         }
      }
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Retract all landing gears.
   /// </summary>
   public void Retract()
   {
      foreach(LandingGear gear in m_LandingGears)
      {
         gear.Retract();
//         Debug.Log("Retract called");
      }
   }

   /// <summary>
   /// Extend all landing gears for landing.
   /// </summary>
   public void Extend()
   {
      foreach(LandingGear gear in m_LandingGears)
      {
         gear.Extend();
//         Debug.Log("Extend called");
      }
   }

   /// <summary>
   /// Steer left with -1 and right with +1
   /// </summary>
   /// <param name="direction">Left or right direction to turn towards</param>
   public void Steer(float direction)
   {
      foreach(LandingGear gear in m_LandingGears)
      {
         if(gear is NoseWheel)
         {
            //NoseWheel noseWheel = (NoseWheel)gear;
            //noseWheel.Steer(direction);
            //Debug.Log("Steering nose " + direction);
         }
      }
   }

   #endregion
}

