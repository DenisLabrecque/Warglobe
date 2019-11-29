using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudder : MonoBehaviour
{
   [Tooltip("How much steering force the rudder actually has")]
   [SerializeField][Range(0, 1)] float m_Effect = 0.1f;

   [Tooltip("Speed the rudder reaches full effect")]
   [SerializeField] [Range(0, 1)] float m_ShiftSpeed = 0.5f;

   Rigidbody m_Rigidbody;
   Vehicle m_Ship;
   private float m_AngleOfAttack = 0;
   private float m_PercentAOA = 0;
   private float m_ActualAOA = 0;

   // A speed that ships generally will never exceed
   public const float GENERAL_MAX_SPEED = 200f;

   // Start is called before the first frame update
   void Awake()
   {
      GetParentItems();
   }

   private void Update()
   {
      m_ActualAOA = Mathf.Lerp(m_ActualAOA, m_PercentAOA, m_ShiftSpeed * Time.deltaTime);
   }

   public void GetParentItems()
   {
      m_Rigidbody = GetComponentInParent<Rigidbody>();
      m_Ship = GetComponentInParent<Vehicle>();
   }

   private void FixedUpdate()
   {
      // Find the total direction the ship is going as compared to the rudder
      Vector3 localVelocity = transform.InverseTransformDirection(m_Rigidbody.velocity);
      m_AngleOfAttack = Vector3.Angle(new Vector3(0, 0, 1), localVelocity);
      // Vector3.Angle always returns a positive value, so add the sign back in
      m_AngleOfAttack *= Mathf.Sign(localVelocity.x);

      m_PercentAOA = DGL.Math.Utility.Percent(m_AngleOfAttack, 90f, DGL.Math.PercentMode.ClampNegative1To1);
      float percentSpeed = DGL.Math.Utility.Percent(m_Ship.ForwardSpeed, GENERAL_MAX_SPEED, DGL.Math.PercentMode.Clamp0To1);
      float massSquared = m_Rigidbody.mass * m_Rigidbody.mass;

      // Debug.Log("Force: " + (speedPercent * m_AngleOfAttack));

      m_Rigidbody.AddRelativeTorque(new Vector3(0, 1, 0) * m_Effect * m_ActualAOA * massSquared * percentSpeed * Time.deltaTime, ForceMode.Force); // The actual turning
      m_Rigidbody.AddRelativeTorque(new Vector3(0, 0, 1) * m_Effect * m_ActualAOA * m_Rigidbody.mass * percentSpeed * Time.deltaTime, ForceMode.Force); // Turning effect
   }
}
