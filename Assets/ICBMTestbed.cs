using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICBMTestbed : MonoBehaviour
{
   [SerializeField] float m_Counter = 2f;
   Hardpoint m_Hardpoint;

   private void Awake()
   {
      m_Hardpoint = GetComponentInChildren<Hardpoint>();
   }

   private void Update()
   {
      m_Counter -= Time.deltaTime;

      if(m_Counter <= 0 && m_Hardpoint.Projectile != null)
      {
         m_Hardpoint.Launch();
      }
   }
}
