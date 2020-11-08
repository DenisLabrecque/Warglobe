using System;
using UnityEngine;

namespace Warglobe.Assets
{
   /// <summary>
   /// Class for scriptable object enum definitions of icons for HUD display.
   /// </summary>
   [CreateAssetMenu(menuName ="Denis/Switchable")]
   public class Switchable : ScriptableObject
   {
      [SerializeField] protected Function _function;
      [SerializeField] protected string _key;
      [SerializeField] protected Sprite _icon;
      [SerializeField] protected Sprite _tracker;

      public Function Function => _function;

      /// <summary>
      /// Name for this particular switchable group.
      /// </summary>
      public string GroupName => Enum.GetName(typeof(Function), _function);

      /// <summary>
      /// Keystroke used for accessing this particular switchable element.
      /// </summary>
      public string Key => _key;

      /// <summary>
      /// Icon that represents this particular set of switchables in the player's settings.
      /// </summary>
      public Sprite Icon => _icon;

      /// <summary>
      /// HUD icon that shows where a gun is pointing. Can be empty for non-gun/missile items.
      /// </summary>
      public Sprite Tracker => _tracker;
   }
}