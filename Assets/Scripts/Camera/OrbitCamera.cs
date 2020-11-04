using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Warglobe;

/// <summary>
/// Defines a camera that rotates around an object (at the starting distance) according to how the user drags the middle mouse button
/// 
/// Denis Labrecque
/// November 2018
/// </summary>
public class OrbitCamera : CameraEmplacement {

   #region Member Variables

   [SerializeField][Range(0.1f,1f)] float m_MaxRotationSpeed = 0.8f; // Degrees per second
   [SerializeField] float m_ZoomIncrement = 100f;
   [SerializeField] float m_MaxZoomDistance;

   GameObject m_RotatePt; // Game object this camera rotates around; this is always the parent of this camera emplacement
   bool m_MouseReleased = true;
   Vector3 m_MouseStartPoint;
   float m_ZoomStartDistance;

   #endregion


   #region Unity Methods

   void Awake()
   {
      m_RotatePt = gameObject.transform.parent.gameObject;

      // We always start zoomed out, so get the distance at start
      m_ZoomStartDistance = Vector3.Distance(transform.position, m_RotatePt.transform.position);
   }

   void Update()
   {
      // Rotate the camera emplacement
      if(_camera != null && m_MouseReleased == true && UserInput.MiddleMouseButton == true)
      {
         m_MouseStartPoint = Input.mousePosition;
         m_MouseReleased = false;
      }
      else if(UserInput.MiddleMouseButton == true)
      {
         // Rotate horizontally
         transform.RotateAround(m_RotatePt.transform.position, m_RotatePt.transform.up, m_MaxRotationSpeed * (m_MouseStartPoint.x - Input.mousePosition.x) * Time.deltaTime);
         // Rotate vertically
         transform.RotateAround(m_RotatePt.transform.position, m_RotatePt.transform.right, m_MaxRotationSpeed * (m_MouseStartPoint.y - Input.mousePosition.y) * Time.deltaTime);
      }
      else
      {
         m_MouseReleased = true;
      }


      // Zoom in/out
      //if(UserInput.ScrollWheel != 0)
      //{
      //   float scroll = UserInput.ScrollWheel;
      //   Mathf.Clamp(scroll, -1f, 1f);
         
      //   // Zoom in
      //   if(scroll > 0)
      //   {
      //      float zoomAmount = transform.position.z + (scroll * m_ZoomStartDistance);
      //      transform.position = new Vector3(transform.position.x, transform.position.y, zoomAmount);
      //   }

      //   // Zoom out
      //   else
      //   {
      //      float zoomAmount = transform.position.z + (scroll * m_ZoomStartDistance);
      //      transform.position = new Vector3(transform.position.x, transform.position.y, zoomAmount);
      //   }
      //}
   }

   #endregion
}
