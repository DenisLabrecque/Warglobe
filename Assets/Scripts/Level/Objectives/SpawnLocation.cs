using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SpawnLocation : MonoBehaviour
{
   [Header("Spawn this Vehicle")]
   [SerializeField] Vehicle m_Vehicle;
   private Transform m_Transform;
   private Vehicle m_LastVehicle;
   private Vehicle m_CurrentVehicle;

   private void Awake()
   {
      m_Transform = gameObject.transform;
   }

   private void Update()
   {
      m_Transform = gameObject.transform;

      if(m_Vehicle == null)
      {
         DestroyVehicle();
      }
      // On the frame that a new vehicle is assigned to the serialized field
      else if (m_LastVehicle == null && m_Vehicle != null)
      {
         InstantiateVehicle();
      }

      m_LastVehicle = m_CurrentVehicle;
   }

   private void InstantiateVehicle()
   {
      if(m_CurrentVehicle != null)
      {
         DestroyVehicle();
      }
      m_CurrentVehicle = Instantiate(m_Vehicle, m_Transform.position, new Quaternion(), m_Transform);
      Debug.Log("Vehicle " + m_CurrentVehicle + " instantiated");
   }

   private void DestroyVehicle()
   {
      if (m_CurrentVehicle != null)
      {
         Debug.Log("Vehicle " + m_CurrentVehicle + " destroyed");
         DestroyImmediate(m_CurrentVehicle);
         m_CurrentVehicle = null;
      }
   }
}
