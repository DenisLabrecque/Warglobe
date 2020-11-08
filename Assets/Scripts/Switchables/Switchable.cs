using UnityEngine;

namespace Warglobe.Assets
{
   [CreateAssetMenu(menuName ="Denis/Switchable")]
   public class Switchable : ScriptableObject
   {
      [SerializeField] protected string _groupName;
      [SerializeField] protected string _key;
      [SerializeField] protected Sprite _icon;

      public string GroupName { get => _groupName; }
      public string Key { get => _key; }
      public Sprite Icon { get => _icon; }
   }
}