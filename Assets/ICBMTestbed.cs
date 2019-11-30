using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICBMTestbed : MonoBehaviour
{
   [SerializeField] float m_Counter = 2f;
   Hardpoint m_Hardpoint;
   bool m_IsFired = false;

   private void Awake()
   {
      m_Hardpoint = GetComponentInChildren<Hardpoint>();
   }

   private void Update()
   {
      if (m_IsFired == false)
      {
         m_Counter -= Time.deltaTime;

         if (m_Counter <= 0)
         {
            Debug.Log("Testbed fired.");
            m_IsFired = true;
            m_Hardpoint.Launch();
         }
      }
   }
}
