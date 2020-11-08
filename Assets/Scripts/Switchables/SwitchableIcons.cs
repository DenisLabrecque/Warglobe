using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Warglobe
{
   [CreateAssetMenu (menuName ="Denis/Switchable Icons")]
   public class SwitchableIcons : ScriptableObject
   {
      [SerializeField] Sprite _gunIcon;
      [SerializeField] Sprite _ciwsIcon;
      [SerializeField] Sprite _missile1Icon;
      [SerializeField] Sprite _radarIcon;
      [SerializeField] Sprite _lightIcon;



      private static SwitchableIcons _singleton = null;
      private static SwitchableIcons Singleton {
         get {
            if (_singleton == null)
            {
               Debug.Log("Singleton initialized");
               _singleton = Resources.FindObjectsOfTypeAll<SwitchableIcons>().FirstOrDefault();
            }
            return _singleton;
         }
      }

      private void OnEnable()
      {
         Debug.Log("Icons enabled");
      }



      public static Sprite IconFromFunction(Function function)
      {
         switch(function)
         {
            case Function.Cannon:
               return Singleton._gunIcon;
            case Function.Ciws:
               return Singleton._ciwsIcon;
            case Function.Missile:
               return Singleton._missile1Icon;
            case Function.Radar:
               return Singleton._radarIcon;
            case Function.Light:
               return Singleton._lightIcon;
            default:
               return Singleton._radarIcon;
         }
      }
   }
}