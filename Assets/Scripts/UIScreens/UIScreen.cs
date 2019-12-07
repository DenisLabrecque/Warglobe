using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UIScreens
{
   [RequireComponent(typeof(CanvasGroup))]
   public class UIScreen : MonoBehaviour
   {
      [Header("Main Properties")]
      [SerializeField] Selectable m_StartSelectable;

      [Header("Screen Events")]
      [SerializeField] UnityEvent m_OnScreenStart = new UnityEvent();
      [SerializeField] UnityEvent m_OnScreenClose = new UnityEvent();

      CanvasGroup m_CanvasGroup;

      private void Awake()
      {
         m_CanvasGroup = GetComponent<CanvasGroup>();
      }

      private void Start()
      {
         if(m_StartSelectable)
         {
            EventSystem.current.SetSelectedGameObject(m_StartSelectable.gameObject);
         }
      }

      public virtual void Open()
      {
         if(m_OnScreenStart != null)
         {
            m_OnScreenStart.Invoke();
         }

         m_CanvasGroup.interactable = true;
         m_CanvasGroup.blocksRaycasts = true;
         m_CanvasGroup.alpha = 1;
      }

      public virtual void Close()
      {
         if(m_OnScreenClose != null)
         {
            m_OnScreenClose.Invoke();
         }

         m_CanvasGroup.interactable = false;
         m_CanvasGroup.blocksRaycasts = false;
         m_CanvasGroup.alpha = 0;
      }
   }
}