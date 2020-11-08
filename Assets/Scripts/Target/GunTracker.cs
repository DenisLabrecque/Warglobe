using UnityEngine;
using UnityEngine.UI;
using Warglobe;

public class GunTracker : MonoBehaviour
{
   Turret _turret;
   Image _image;

   private void Update()
   {
      gameObject.transform.position = SingleCamera.Camera1.WorldToScreenPoint(_turret.PointingAt);
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
