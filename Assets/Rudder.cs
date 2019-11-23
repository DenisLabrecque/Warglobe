using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudder : MonoBehaviour
{
   Rigidbody m_Rigidbody;
   Vehicle m_Ship;
   private float m_AngleOfAttack = 0;

   // Start is called before the first frame update
   void Awake()
   {
      GetParentItems();
   }

   public void GetParentItems()
   {
      m_Rigidbody = GetComponentInParent<Rigidbody>();
      m_Ship = GetComponentInParent<Vehicle>();
   }

   private void FixedUpdate()
   {
      Vector3 localVelocity = transform.InverseTransformDirection(m_Rigidbody.velocity);
      // Angle of attack is used as the look up for the lift and drag curves.
      m_AngleOfAttack = Vector3.Angle(new Vector3(1, 0), localVelocity);

      Debug.Log("AOA: " + m_AngleOfAttack);

      //m_Rigidbody.AddTorque(localVelocity * m_AngleOfAttack * 1000);
      //Vector3 liftDirection = Vector3.Cross(m_Rigidbody.velocity, transform.right).normalized;
      //m_Rigidbody.AddForceAtPosition(liftDirection * m_AngleOfAttack * m_Rigidbody.mass * m_Ship.ForwardSpeed, gameObject.transform.position, ForceMode.Force);

      m_Rigidbody.AddTorque(new Vector3(0, 1, 0) * m_AngleOfAttack * m_Rigidbody.mass * m_Ship.Throttle, ForceMode.Force);
   }
}
