using System;
using System.Collections;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace UnityEngine.UI
{
   // Button that's meant to work with mouse or touch-based devices.
   /// <summary>
   /// Button with sounds and hover colours only.
   /// See 'Button' and 'Selectable' at
   /// https://bitbucket.org/Unity-Technologies/ui/src/fd5d3578da8c/UnityEngine.UI/UI/Core/?at=4.6
   /// 
   /// Denis Labrecque
   /// December 2018
   /// </summary>
   [AddComponentMenu("UI/Button2", 30)]
   [RequireComponent(typeof(AudioSource))]
   public class Button2 : MonoBehaviour, IPointerClickHandler, ISubmitHandler, IPointerExitHandler, IPointerEnterHandler
   {
      [Serializable]
      public class ButtonClickedEvent : UnityEvent { }

      // Event delegates triggered on click.
      [FormerlySerializedAs("onClick")]
      [SerializeField]
      private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

      [SerializeField] ButtonStyle m_ButtonStyle;
      AudioSource m_Audio;
      TextMeshProUGUI m_Text;
      Color m_NormalColour;

      protected Button2()
      { }

      public ButtonClickedEvent onClick {
         get { return m_OnClick; }
         set { m_OnClick = value; }
      }

      void Awake()
      {
         m_Audio = GetComponent<AudioSource>();
         m_Audio.playOnAwake = false;
         m_Audio.volume = 1f;

         m_Text = GetComponentInChildren<TextMeshProUGUI>();
         m_NormalColour = m_Text.color;
      }

      private void Hover()
      {
         m_Audio.clip = m_ButtonStyle.HoverAudio;
         m_Audio.Play();
         //m_Text.CrossFadeColor(m_ButtonStyle.HoverColour, m_ButtonStyle.EffectDuration, true, false);
         m_Text.color = m_ButtonStyle.HoverColour;
      }

      private void Press()
      {
         m_OnClick.Invoke();

         m_Text.color = m_ButtonStyle.ClickColour;
         m_Audio.clip = m_ButtonStyle.ClickAudio;
         m_Audio.Play();
      }

      private void Leave()
      {
         //m_Text.CrossFadeColor(m_NormalColour, m_ButtonStyle.EffectDuration, true, false);
         m_Text.color = m_NormalColour;
      }

      // Trigger all registered callbacks.
      public virtual void OnPointerClick(PointerEventData eventData)
      {
         if(eventData.button != PointerEventData.InputButton.Left)
            return;

         Press();
      }

      public virtual void OnSubmit(BaseEventData eventData)
      {
         Press();

         //StartCoroutine(OnFinishSubmit());
      }

      public void OnPointerExit(PointerEventData eventData)
      {
         Leave();
      }

      public void OnPointerEnter(PointerEventData eventData)
      {
         Hover();
      }
   }
}
