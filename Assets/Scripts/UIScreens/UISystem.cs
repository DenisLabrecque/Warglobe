using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

namespace UIScreens
{
   public class UISystem : MonoBehaviour
   {
      [Header("Main Properties")]
      [SerializeField] UIScreen m_StartScreen;

      [Header("System Events")]
      [SerializeField] UnityEvent m_OnSwitchedScreen = new UnityEvent();

      private List<Component> m_Components = new List<Component>();
      private List<UIScreen> m_Screens = new List<UIScreen>();
      private UIScreen m_CurrentScreen;
      private UIScreen m_PreviousScreen;

      public UIScreen CurrentScreen { get { return m_CurrentScreen; } }
      public UIScreen PreviousScreen { get { return m_PreviousScreen; } }

      private void Awake()
      {
         m_Components = GetComponentsInChildren<Component>(true).ToList();
         m_Screens = GetComponentsInChildren<UIScreen>(true).ToList();
      }

      private void Start()
      {
         InitializeScreens();
      }

      private void InitializeScreens()
      {
         foreach(Component screen in m_Components)
         {
            screen.gameObject.SetActive(true);
         }
         foreach(UIScreen screen in m_Screens)
         {
            screen.Close();
         }

         m_StartScreen.Open();
      }

      public void SwitchScreens(UIScreen openScreen)
      {
         if(openScreen)
         {
            if(m_CurrentScreen)
            {
               m_CurrentScreen.Close();
               m_PreviousScreen = m_CurrentScreen;
            }

            m_CurrentScreen = openScreen;
            m_CurrentScreen.gameObject.SetActive(true);
            m_CurrentScreen.Open();

            if(m_OnSwitchedScreen != null)
            {
               m_OnSwitchedScreen.Invoke();
            }
         }
      }

      public void LoadScene(int sceneIndex)
      {
         StartCoroutine(WaitToLoadScene(sceneIndex));
      }

      IEnumerator WaitToLoadScene(int sceneIndex)
      {
         yield return null;
      }
   }
}