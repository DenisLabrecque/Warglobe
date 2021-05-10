using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowTurret : CameraEmplacement
{
   #region Unity Methods

   [SerializeField] float m_MaxRotationSpeed = 0.8f; // Degrees per second
   [SerializeField] int m_MaxHorizontalArc = 100; // Degrees left or right from center

   float m_HorizontalCenter;
   float m_VerticalCenter;

   void Update()
   {
      m_HorizontalCenter = Screen.width * 0.5f;
      m_VerticalCenter = Screen.height * 0.5f;

      // Rotate horizontally

      //transform.Rotate(
      //   m_MaxRotationSpeed * (m_VerticalCenter - Input.mousePosition.y) * Time.deltaTime, // Vertical (x)
      //   m_MaxRotationSpeed * (m_HorizontalCenter - Input.mousePosition.x) * Time.deltaTime, // Horizontal (y)
      //   0,
      //   Space.Self);

      transform.Rotate(
         0,
         m_MaxRotationSpeed * -(m_HorizontalCenter - Input.mousePosition.x) * Time.deltaTime, // Horizontal (y)
         0,
         Space.Self);

//      Debug.Log(transform.localRotation.x);
   }

   #endregion
}
