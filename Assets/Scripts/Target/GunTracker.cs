using UnityEngine;
using UnityEngine.UI;
using Warglobe;

public class GunTracker : MonoBehaviour
{
   [SerializeField] Sprite _ciwsTracker;
   [SerializeField] Sprite _gunTracker;
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

         switch(turret.Function)
         {
            case Function.Cannon:
               _image.sprite = _gunTracker;
               break;
            case Function.Ciws:
               _image.sprite = _ciwsTracker;
               break;
         }
      }
      else
         Debug.LogError("Turret was assigned as null to the tracker");
   }
}
