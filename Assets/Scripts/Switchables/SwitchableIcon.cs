using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Warglobe.Hud
{
   /// <summary>
   /// HUD element for displaying information about a weapon like the keyboard shortcut and whether it's on.
   /// </summary>
   public class SwitchableIcon : MonoBehaviour
   {
      [SerializeField] public ISwitchable _element; // The gun, radar, light, etc.

      [SerializeField] Image _icon;
      [SerializeField] TextMeshProUGUI _name;
      [SerializeField] TextMeshProUGUI _keystroke;

      // Update is called once per frame
      void Update()
      {
         if (_element != null)
         {
            // TODO get the type of weapon and match with icon
            SetOnOrSelected();
            SetKeystroke();
            // TODO get how many projectiles are left
         }
      }

      /// <summary>
      /// Call this before next run. Awake and start are useless at the beginning of the game when there's no vehicle,
      /// or when the vehicle is changed.
      /// </summary>
      public void Initialize(ISwitchable switchable)
      {
         gameObject.SetActive(true);

         _element = switchable;
         _name.text = _element.Switchable.GroupName.ToUpperInvariant();
         _icon.sprite = _element.Switchable.Icon;
            
         // TODO decide the hud group where things go (using SwitchableHelper.cs)
      }

      /// <summary>
      /// Show transparent if the switchable is deselected or off.
      /// </summary>
      void SetOnOrSelected()
      {
         if (_element.IsOnOrSelected)
         {
            _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, 1f);
            _name.color = new Color(_name.color.r, _name.color.g, _name.color.b, 1f);
         }
         else
         {
            _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, 0.5f);
            _name.color = new Color(_name.color.r, _name.color.g, _name.color.b, 0.5f);
         }
      }

      void SetKeystroke()
      {
         _keystroke.text = _element.Switchable.Key;
      }
   }
}