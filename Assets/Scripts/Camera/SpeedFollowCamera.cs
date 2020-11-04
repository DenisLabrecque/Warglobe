using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warglobe;

/// <summary>
/// Define a camera emplacement that shows speed and angle of attack of a vehicle.
/// </summary>
public class SpeedFollowCamera : CameraEmplacement
{
   public const int MAX_SPEED = 600;
   public const float MAX_GFORCE = 9.2f;
   public const float MAX_YMOVE = 20f; // How much the camera can move upwards in 1 second

   [Header("Follow characteristics")]
   [SerializeField][Range(5,200)] float m_MaxForwardMovement = 100;
   [SerializeField][Range(1,30)] float m_MaxUpwardsMovement = 20;

   Vehicle m_Vehicle;
   Transform m_Target;
   float m_StartDistZ;
   float m_StartDistY;
   float m_PreviousY = 0;
   float m_PreviousZ = 0;

   void Awake()
   {
      m_Vehicle = GetComponentInParent<Vehicle>();
      m_Target = m_Vehicle.transform;
      m_StartDistZ = transform.localPosition.z;
      m_StartDistY = transform.localPosition.y;
   }

   void Start()
   {
      // Start at the right place depending on speed
      transform.localPosition = new Vector3(0, OffsetYAtGForce(m_Vehicle.VerticalSpeed), DistanceZAtSpeed(m_Vehicle.ForwardSpeed));

      //Multilang.Initialize(Language.Francais); // the heck was there doing here?
   }

   void LateUpdate()
   {
      transform.localPosition = new Vector3(0, OffsetYAtGForce(m_Vehicle.VerticalSpeed), DistanceZAtSpeed(m_Vehicle.ForwardSpeed));
   }

   /// <summary>
   /// The distance the camera should be from the target at the target's present speed.
   /// </summary>
   /// <returns>An appropriate camera distance</returns>
   private float DistanceZAtSpeed(float speed)
   {
      float wantedZ;
      float increment;

      // Move camera to percent of distance
      Mathf.Clamp(speed, -MAX_SPEED, MAX_SPEED);
      wantedZ = ((speed / MAX_SPEED) * (-m_MaxForwardMovement)) + m_StartDistZ;
      increment = Mathf.Clamp(wantedZ - m_PreviousZ, -MAX_YMOVE, MAX_YMOVE);
      m_PreviousZ = m_PreviousZ + increment;
      return m_PreviousZ;
   }

   /// <summary>
   /// How high or low the camera should be at a certain G force.
   /// </summary>
   /// <param name="gForce">The G force being felt (can be positive or negative)</param>
   /// <returns>An appropriate camera height to emphasize G force</returns>
   private float OffsetYAtGForce(float gForce)
   {
      float wantedY;
      float increment;

      // Percent of height that doesn't move too fast (avoiding jitter)
      Mathf.Clamp(gForce, -MAX_GFORCE, MAX_GFORCE);
      wantedY = (gForce / MAX_GFORCE) * m_MaxUpwardsMovement;
      increment = Mathf.Clamp(wantedY - m_PreviousY, -MAX_YMOVE, MAX_YMOVE) * Time.deltaTime;
      m_PreviousY = m_PreviousY + increment;
      return -m_PreviousY; // Now next y; negative changes upwards direction
   }
}
