using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAimingSystem : MonoBehaviour
{
   [SerializeField] float _aimDistance = 500f;

   Camera _camera;
   Vector2 _mouse;
   Vector3 _oldPoint;

   public Vector3 AimPoint { get; set; }

   private void Start()
   {
      _camera = SingleCamera.Camera1;
   }

   void FixedUpdate()
   {
      // Get the mouse position from Event.
      // Note that the y position from Event is inverted.
      _mouse.x = Input.mousePosition.x;
      _mouse.y = Input.mousePosition.y;
      //Vector3 newPosition = new Vector3(mouse.x, mouse.y)

      //point = m_Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, m_Camera.farClipPlane));
      Vector3 point = _camera.ScreenToWorldPoint(new Vector3(_mouse.x, _mouse.y, _aimDistance));
      Vector3 lerped = Vector3.Lerp(_oldPoint, point, 1f);

      _oldPoint = AimPoint;
      AimPoint = point;
   }
}
