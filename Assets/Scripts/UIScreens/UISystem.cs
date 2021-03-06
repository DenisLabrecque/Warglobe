﻿using System.Collections;
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
      private static CameraEmplacement m_Emplacement;

      public UIScreen CurrentScreen { get { return m_CurrentScreen; } }
      public UIScreen PreviousScreen { get { return m_PreviousScreen; } }

      private void Awake()
      {
         m_Components = GetComponentsInChildren<Component>(true).ToList();
         m_Screens = GetComponentsInChildren<UIScreen>(true).ToList();
         m_Emplacement = GetComponentInChildren<CameraEmplacement>();
      }

      private void Start()
      {
         InitializeScreens();
      }

      /// <summary>
      /// Enable all screens in case an artist disabled one. Then close all screens, and open only the start screen.
      /// </summary>
      private void InitializeScreens()
      {
         foreach (Component component in m_Components)
         {
            if (component is UIScreen)
            {
               component.gameObject.SetActive(true);
            }
         }
         foreach (UIScreen screen in m_Screens)
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

      public static void AttachCamera(Camera camera)
      {
         m_Emplacement.Attach(camera);
      }

      IEnumerator WaitToLoadScene(int sceneIndex)
      {
         yield return null;
      }
   }
}