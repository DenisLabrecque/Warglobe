using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generally control how long a panel is visible.
/// Set is done to true to go to the next panel.
/// 
/// Denis Labrecque
/// December 2018
/// </summary>
public abstract class LPanelController : MonoBehaviour
{
   protected bool m_IsDone = false;

   public bool IsDone { get { return m_IsDone; } }

   public void Hide()
   {
      m_IsDone = true;
   }

   public void Show()
   {
      m_IsDone = false;
   }
}
