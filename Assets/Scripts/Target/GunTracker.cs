using DGL.Math;
using UnityEngine;
using UnityEngine.UI;
using Warglobe;

public class GunTracker : MonoBehaviour
{
   Turret _turret;
   Image _image;

   const float SmoothingSpeed = 0.1f; // Amount of interpolation per second
   Vector3 _oldPoint = new Vector3();
   Vector3 _newPoint;


   private void Update()
   {
      _newPoint = SingleCamera.Camera1.WorldToScreenPoint(_turret.PointingAt); // This may be very jittery
      gameObject.transform.position = Vector3.Slerp(_oldPoint, _newPoint, Time.deltaTime * SmoothingSpeed); // Interpolate to smooth out
      _oldPoint = _newPoint;
   }

   internal void SetTurret(Turret turret)
   {
      if (turret != null)
      {
         _turret = turret;
         _image = GetComponent<Image>();
         _image.sprite = _turret.Switchable.Tracker;
      }
      else
         Debug.LogError("Turret was assigned as null to the tracker");
   }
}
