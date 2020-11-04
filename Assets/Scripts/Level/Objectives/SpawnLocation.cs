using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warglobe
{
   [ExecuteInEditMode]
   public class SpawnLocation : MonoBehaviour
   {
      [Header("Spawn this Vehicle")]
      [SerializeField] Vehicle m_Vehicle;
      [SerializeField] bool m_PointVertically = true;
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

         // Remove the vehicle if there was one when the prefab pointed to is null
         if (m_Vehicle == null)
         {
            DestroyVehicle();
         }
         // On the frame that a new vehicle is assigned to the serialized field
         else if (m_LastVehicle == null && m_Vehicle != null)
         {
            InstantiateVehicle();
         }
         // Remove the vehicle and replace it if there is a different prefab pointed to
         //else if (m_Vehicle != m_CurrentVehicle)
         //{
         //   DestroyVehicle();
         //   InstantiateVehicle();
         //}

         m_LastVehicle = m_CurrentVehicle;
      }

      private void InstantiateVehicle()
      {
         if (m_CurrentVehicle != null)
         {
            DestroyVehicle();
         }
         m_CurrentVehicle = Instantiate(m_Vehicle, m_Transform.position, new Quaternion(), m_Transform);
         if (m_PointVertically)
         {
            PointTowardsEarth.PointVertically(m_CurrentVehicle.gameObject);
         }
         Debug.Log("Vehicle " + m_CurrentVehicle + " instantiated");
      }

      private void DestroyVehicle()
      {
         if (m_CurrentVehicle != null)
         {
            Debug.Log("Vehicle " + m_CurrentVehicle + " destroyed");
            DestroyImmediate(m_CurrentVehicle.gameObject);
            m_CurrentVehicle = null;
         }
      }
   }
}