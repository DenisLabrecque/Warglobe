using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
   [SerializeField] UnityEvent m_ObjectivesAccomplished;
   static List<Objective> m_AllObjectives;
   bool m_AllObjectivesAccomplished = false;

   public static void Add(Objective objective)
   {
      if(m_AllObjectives == null)
      {
         m_AllObjectives = new List<Objective>();
      }
      m_AllObjectives.Add(objective);
   }

   private void Start()
   {
      InvokeRepeating("CheckAccomplished", 1f, 1f);
   }

   /// <summary>
   /// Coroutine checking whether the objectives are all accomplished at each second.
   /// </summary>
   bool CheckAccomplished()
   {
      if (m_AllObjectives == null)
      {
         return false;
      }
      else
      {
         bool areObjectivesAccomplished = true;
         foreach (Objective objective in m_AllObjectives)
         {
            if (objective.IsObjectiveAccomplished() == false)
            {
               areObjectivesAccomplished = false;
               break;
            }
         }
         m_AllObjectivesAccomplished = areObjectivesAccomplished;

         if (m_AllObjectivesAccomplished)
         {
            m_ObjectivesAccomplished.Invoke();
            return true;
         }
         else
         {
            return false;
         }
      }
   }
}