using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCamera : CameraEmplacement
{
   [Header("View Settings")]
   [SerializeField] float m_MaxNearDistance = 20f;
   [SerializeField] float m_MaxFarDistance = 300f;
   [SerializeField] float m_MaxHeight = 75f;

   private Target m_Ship;

   private void Awake()
   {
      m_Ship = GetComponentInParent<Target>();
   }

   private void Start()
   {
      // Place the camera at the back
      Vector3 emplacement = new Vector3(
                 0,
                 m_MaxHeight,
                 m_MaxFarDistance);
      transform.localPosition = emplacement;
      //transform.localRotation = m_Ship.transform.rotation;
   }

   private void LateUpdate()
   {
      //gameObject.transform.LookAt(m_Ship.transform.position);
   }
}
