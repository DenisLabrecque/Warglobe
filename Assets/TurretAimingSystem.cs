using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAimingSystem : MonoBehaviour
{
   [SerializeField] GameObject m_IndicatorBall;
   [SerializeField] float m_ProjectionDistance = 500f;

   Camera m_Camera;

   public Vector3 AimPoint { get; set; }

   private void Start()
   {
      m_Camera = SingleCamera.Camera1;
   }

   void OnGUI()
   {
      Vector3 point = new Vector3();
      Event currentEvent = Event.current;
      Vector2 mousePos = new Vector2();

      // Get the mouse position from Event.
      // Note that the y position from Event is inverted.
      mousePos.x = currentEvent.mousePosition.x;
      mousePos.y = m_Camera.pixelHeight - currentEvent.mousePosition.y;

      //point = m_Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, m_Camera.farClipPlane));
      point = m_Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, m_ProjectionDistance));

      m_IndicatorBall.transform.position = point;
      AimPoint = point;
   }
}
