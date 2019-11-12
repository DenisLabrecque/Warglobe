using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/// <summary>
/// HUD element that follows a target onscreen. The target symbol varies based on target type and acquiree characteristics. 
/// It is the vehicle's responsibility to ensure that invisible trackers are assigned new targets as needed.
/// </summary>
public class HUDTracker : MonoBehaviour
{
   #region Sprites

   [SerializeField] Sprite SQUARE_SOLID;
   [SerializeField] Sprite SQUARE_DASHED;
   [SerializeField] Sprite DIAMOND_SOLID;
   [SerializeField] Sprite DIAMOND_DASHED;
   [SerializeField] Sprite CIRCLE_SOLID;
   [SerializeField] Sprite CIRCLE_DASHED;
   [SerializeField] Sprite HEXAGON_SOLID;
   [SerializeField] Sprite HEXAGON_DASHED;

   #endregion


   #region Properties

   /// <summary>
   /// The target this tracker is following.
   /// Can return null if no target was assigned.
   /// </summary>
   public Target Target {
      get {
         return m_Target;
      }
   }

   /// <summary>
   /// Whether the target being tracked has been locked onto by a weapon.
   /// </summary>
   public bool IsLockedOn {
      get; set;
   }

   /// <summary>
   /// Whether the tracker should be seen or not
   /// </summary>
   public bool IsVisible { get; set; }

   #endregion


   #region Member Variables

   [Header("Text Info")]
   [SerializeField] TextMeshProUGUI m_TargetName;
   [SerializeField] TextMeshProUGUI m_TargetDistance;

   [Header("Target and Shape")]
   [SerializeField] Image m_TargetingShape;
   [SerializeField] Target m_Target = null;
   Waypoint m_Waypoint = null;

   [Header("Colours")]
   [SerializeField] Color FRIEND = Color.blue;
   [SerializeField] Color FOE = Color.green;
   [SerializeField] Color NEUTRAL = Color.white;
   [SerializeField] Color ALERT = Color.yellow;
   [SerializeField] Color WARNING = Color.red;

   #endregion


   #region Unity Methods

   void Update()
   {
      // Follow the target or waypoint onscreen
      Vector3 targetPoint;
      if(m_Target != null)
         targetPoint = SingleCamera.GameCam.WorldToScreenPoint(m_Target.transform.position);
      else
         targetPoint = SingleCamera.GameCam.WorldToScreenPoint(m_Waypoint.transform.position);

      if(targetPoint.z < 0f || IsVisible == false)
      {
         ApplyTargetingColour(Color.clear);
         IsLockedOn = false;
         return;
      }
      else
      {
         targetPoint.z = 1;
         gameObject.transform.position = targetPoint;
         gameObject.transform.eulerAngles = Vector3.forward * UserInput.CurrentVehicle.RollAngle;

         if(m_Target != null)
            ApplySymbology(m_Target);
         else
            ApplySymbology(m_Waypoint);
      }
   }

   #endregion


   #region Private Methods

   /// <summary>
   /// Make sure HUD symbols and colours match the target's description
   /// </summary>
   /// <param name="target">The target</param>
   private void ApplySymbology(Target target)
   {
      // Show target name and distance if the target is locked by a weapon
      ShowTargetText();

      // Apply correct shapes to the tracker according to target type
      ShowTargetBox(target);
      ApplyRelationshipColour();
   }

   /// <summary>
   /// Make sure HUD matches waypoint description
   /// </summary>
   /// <param name="waypoint">The waypoint</param>
   private void ApplySymbology(Waypoint waypoint)
   {
      // Show waypoint name and distance
      m_TargetName.text = waypoint.name;
      ApplyTargetingColour(Color.white);
   }

   /// <summary>
   /// Update the target's name and distance.
   /// </summary>
   private void ShowTargetText()
   {
      if(IsLockedOn)
      {
         // Print target name
         m_TargetName.text = m_Target.PopularName;

         // Print target distance
         float distance = Vector3.Distance(UserInput.CurrentVehicle.transform.position, m_Target.transform.position);
         m_TargetDistance.text = ((int)distance).ToString();
      }
      else
      {
         // Erase text
         m_TargetName.text = string.Empty;
         m_TargetDistance.text = string.Empty;
      }
   }

   /// <summary>
   /// Choose a correct sprite shape for the type of target being tracked.
   /// </summary>
   /// <param name="target">The target being tracked.</param>
   private void ShowTargetBox(Target target)
   {
      if(target is Airplane)
         m_TargetingShape.sprite = (IsLockedOn ? SQUARE_SOLID : SQUARE_DASHED);
      else if(target is Ship)
         m_TargetingShape.sprite = (IsLockedOn ? HEXAGON_SOLID : HEXAGON_DASHED);
      else if(target is Building)
         m_TargetingShape.sprite = (IsLockedOn ? DIAMOND_SOLID : DIAMOND_DASHED);
      else
         m_TargetingShape.sprite = (IsLockedOn ? CIRCLE_SOLID : CIRCLE_DASHED);
   }

   /// <summary>
   /// Automatically choose a correct targeting colour and apply it.
   /// </summary>
   protected void ApplyRelationshipColour()
   {
      // Find the correct colour according to target relationships by country
      switch(UserInput.CurrentVehicle.Country.Relationship(m_Target.Country))
      {
         case Country.Identification.Neutral:
            ApplyTargetingColour(NEUTRAL);
            break;

         case Country.Identification.Friend:
            ApplyTargetingColour(FRIEND);
            break;

         case Country.Identification.Foe:
            ApplyTargetingColour(FOE);
            break;

         default:
            ApplyTargetingColour(ALERT);
            break;
      }
   }

   /// <summary>
   /// Make the tracker a certain colour (eg. green for targets, blue for friendly, white for neutral)
   /// </summary>
   private void ApplyTargetingColour(Color colour)
   {
      m_TargetName.color = colour;
      m_TargetDistance.color = colour;
      m_TargetingShape.color = colour;
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Main method for starting a tracker on a target.
   /// </summary>
   /// <param name="target">The target to track</param>
   public void SetTarget(Target target)
   {
      // Set the tracker to the target
      m_Target = target;

      // Ensure a valid target was assigned
      if(m_Target == null)
      {
         Debug.LogError("A null should not initialize a target");
         Destroy(gameObject);
      }

      // Apply text and symbology
      ApplySymbology(target);
   }


   /// <summary>
   /// Main method for starting a tracker on a waypoint.
   /// </summary>
   /// <param name="waypoint">The waypoint to track</param>
   public void SetWaypoint(Waypoint waypoint)
   {
      // Set the tracker to the waypoint
      m_Waypoint = waypoint;
      IsVisible = true;

      // Apply text and symbols
      ApplySymbology(waypoint);
   }

   #endregion
}
