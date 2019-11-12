using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LOpenTheGame : MonoBehaviour
{
   static List<CanvasGroup> m_CanvasGroups = new List<CanvasGroup>();
   static List<LPanelController> m_PanelControllers = new List<LPanelController>();
   int m_PanelIndex = 0;

   void Awake()
   {
      m_CanvasGroups = GetComponentsInChildren<CanvasGroup>().ToList();
      m_PanelControllers = GetComponentsInChildren<LPanelController>().ToList();
      //PlayerPrefs.DeleteAll();

      ShowVisiblePanelOnly();
   }

   /// <summary>
   /// Go through all the canvas groups that are done and show the next one that needs input.
   /// </summary>
   void Update()
   {
      if(m_PanelControllers[m_PanelIndex].IsDone && (m_PanelIndex + 2) <= m_PanelControllers.Count)
      {
         m_PanelIndex++;
//         Debug.Log("Present panel is now [" + m_PanelIndex + "]; " + (m_PanelIndex+1) + " out of " + m_PanelControllers.Count);

         ShowCanvasGroup(m_PanelIndex);
      }
   }

   /// <summary>
   /// Show a desired canvas.
   /// </summary>
   public void ShowCanvasGroup(int index)
   {
      // Hide all other canvasses
      foreach(CanvasGroup canvas in m_CanvasGroups)
      {
         HideCanvasGroup(canvas);
      }

      // Show the canvas wanted
      ShowCanvasGroup(m_CanvasGroups[index]);
   }

   private void HideCanvasGroup(CanvasGroup canvasGroup)
   {
      canvasGroup.alpha = 0;
      canvasGroup.blocksRaycasts = false;
      canvasGroup.interactable = false;
      canvasGroup.ignoreParentGroups = true;
   }

   private void ShowCanvasGroup(CanvasGroup canvasGroup)
   {
      canvasGroup.alpha = 1;
      canvasGroup.blocksRaycasts = true;
      canvasGroup.interactable = true;
      canvasGroup.ignoreParentGroups = true;
   }

   /// <summary>
   /// Show only the panel with alpha of 1 so that the other panels don't block raycasts.
   /// </summary>
   private void ShowVisiblePanelOnly()
   {
      bool shown = false;
      foreach(CanvasGroup cg in m_CanvasGroups)
      {
         if(cg.alpha == 1 && shown == false)
         {
            shown = true;
         }
         else
         {
            HideCanvasGroup(cg);
         }
      }
   }
}
