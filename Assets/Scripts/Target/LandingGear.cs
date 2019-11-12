using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Archetype for airplane landing gear prefabs. Contain instructions for how to handle collisions, spinning, turning, and retracting gear
/// according to airplane inputs.
/// Taken from similar code in SimpleWings ControlSurface component.
/// 
/// Denis Labrecque
/// November 2018
/// </summary>
public abstract class LandingGear : MonoBehaviour {

   #region Member Variables

   [Tooltip("Landing gear retraction angle into the airplane")]
   [SerializeField] float m_RetractionAngle = 90f;

   [Tooltip("Retraction speed")]
   [SerializeField] protected float m_RetractionSpeed = 90f;
   
   protected Quaternion m_LocalRotation = Quaternion.identity;

   protected float m_TargetRetraction = 0f; // Target
   protected float m_ActualRetraction = 0f; // Actual

   #endregion


   #region Properties

   /// <summary>
   /// Degree to which the wheel is inside the airplane, with 0 being completely extended outwards.
   /// </summary>
   public float RetractionAngle { get { return m_ActualRetraction; } }

   /// <summary>
   /// Whether the wheel is ready for landing.
   /// </summary>
   public bool IsExtended { get { return (m_ActualRetraction == 0) ? true : false; } }

   #endregion


   #region Unity Methods

   private void Start()
   {
      // Dirty hack so that the rotation can be reset before applying the retraction.
      m_LocalRotation = transform.localRotation;
   }

   private void FixedUpdate()
   {
      // Retract the wheel
      m_ActualRetraction = Mathf.MoveTowards(m_ActualRetraction, m_TargetRetraction, m_RetractionSpeed * Time.deltaTime);

      // Hacky way to do this!
      transform.localRotation = m_LocalRotation;
      transform.Rotate(Vector3.right, m_ActualRetraction, Space.Self);
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Main method to tuck landing gear in.
   /// </summary>
   public void Retract()
   {
      m_TargetRetraction = m_RetractionAngle;
   }

   /// <summary>
   /// Main method to prepare gear for landing.
   /// </summary>
   public void Extend()
   {
      m_TargetRetraction = 0;
   }

   #endregion
}
