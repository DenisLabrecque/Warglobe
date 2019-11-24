using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UIController : MonoBehaviour {

   enum UIPanel { MainMenu, Credits, Levels, Minimap, AircraftHUD, ShipHUD, VehicleHUD }

   #region Properties

   #endregion


   #region Member Variables

   [SerializeField] static List<CanvasGroup> m_CanvasGroups;

   #endregion


   #region Unity Methods

   void Start()
   {
      m_CanvasGroups = GetComponentsInChildren<CanvasGroup>().ToList();
   }

   void Update()
   {
      if(UserInput.Player1Vehicle is Airplane)
      {
         ShowPanel((int)UIPanel.AircraftHUD);
      }
   }

   #endregion


   #region Public Methods

   /// <summary>
   /// Show a menu and hide all others.
   /// </summary>
   /// <param name="panelIndex">The index of the menu to show. Canvas group indices are available as constants from this class.</param>
   public static void ShowPanel(int panelIndex)
   {
      // Hide every menu
      foreach(CanvasGroup menu in m_CanvasGroups)
      {
         menu.alpha = 0;
         menu.interactable = false;
      }

      // Show the panel we want
      m_CanvasGroups[panelIndex].alpha = 1;
      m_CanvasGroups[panelIndex].interactable = true;
   }

   #endregion
}
