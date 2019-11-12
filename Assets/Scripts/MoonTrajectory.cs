using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonTrajectory : MonoBehaviour {

   [SerializeField] Transform m_Planet;
   [SerializeField] float m_MaxForce = 200f;
   [SerializeField] float m_MaxDistance = 80000f;
   [SerializeField] float m_MinDistance = 50000f;

   private Rigidbody m_Rigidbody;
   private float distance;
   private Vector3 direction;
   private float powerPercent;

   void Start()
   {
      m_Rigidbody = GetComponent<Rigidbody>();
      CalculateForces();
   }

   void FixedUpdate()
   {
      CalculateForces();
      ApplyForces();
   }

   private void ApplyForces()
   {
      // Move the moon
      m_Rigidbody.AddForce(-direction * m_Rigidbody.mass * powerPercent * m_MaxForce);
   }

   private void CalculateForces()
   {
      distance = Vector3.Distance(m_Planet.position, transform.position);
      direction = transform.position - m_Planet.transform.position;
      powerPercent = (distance - m_MinDistance) / (m_MaxDistance - m_MinDistance);
   }
}
