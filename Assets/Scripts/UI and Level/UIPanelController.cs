using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Show and hide UI panels.
/// </summary>
public class UIPanelController : MonoBehaviour
{
   static List<CanvasGroup> m_Canvases = new List<CanvasGroup>();
   static List<LPanelController> m_PanelControllers = new List<LPanelController>();
   int m_PanelIndex = 0;

   void Awake()
   {
      m_Canvases = GetComponentsInChildren<CanvasGroup>().ToList();
      m_PanelControllers = GetComponentsInChildren<LPanelController>().ToList();
   }

   /// <summary>
   /// Go through all the canvas groups that are done and show the next one that needs input.
   /// </summary>
   void Update()
   {
      if(m_PanelControllers[m_PanelIndex].IsDone && (m_PanelIndex + 2) <= m_PanelControllers.Count)
      {
         m_PanelIndex++;
         //Debug.Log("Present panel is now " + m_PanelIndex + " panel count is " + m_PanelControllers.Count);

         ShowCanvasGroup(m_PanelIndex);
      }
   }

   /// <summary>
   /// Show a desired canvas.
   /// </summary>
   public void ShowCanvasGroup(int index)
   {
      HideAllCanvasses();

      // Show the canvas wanted
      m_Canvases[index].alpha = 1;
      m_Canvases[index].blocksRaycasts = true;
      m_Canvases[index].interactable = true;
      m_Canvases[index].ignoreParentGroups = true;
   }

   /// <summary>
   /// Hide all canvasses for this controller.
   /// </summary>
   public void HideAllCanvasses() {
      foreach(CanvasGroup canvas in m_Canvases)
      {
         canvas.alpha = 0;
         canvas.blocksRaycasts = false;
         canvas.interactable = false;
         canvas.ignoreParentGroups = true;
      }
   }
}

