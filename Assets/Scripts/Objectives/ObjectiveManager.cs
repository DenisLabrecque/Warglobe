using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
   [SerializeField] UnityEvent m_ObjectivesAccomplished;
   static List<Objective> m_AllObjectives = new List<Objective>();
   bool m_AllObjectivesAccomplished = false;

   public static void Add(Objective objective)
   {
      m_AllObjectives.Add(objective);
   }

   private void Start()
   {
      StartCoroutine("CheckAccomplished");
   }

   /// <summary>
   /// Coroutine checking whether the objectives are all accomplished at each second.
   /// </summary>
   IEnumerator CheckAccomplished()
   {
      for (; ; )
      {
         bool areObjectivesAccomplished = true;
         foreach(Objective objective in m_AllObjectives)
         {
            if (objective.IsAccomplished)
               continue;
            else
            {
               areObjectivesAccomplished = false;
               break;
            }
         }
         m_AllObjectivesAccomplished = areObjectivesAccomplished;

         if(m_AllObjectivesAccomplished)
         {
            m_ObjectivesAccomplished.Invoke();
            Debug.Log("ALL OBJECTIVES ACCOMPLISHED!");
         }
         else
         {
            Debug.Log("Checked... " + m_AllObjectives);
         }

         yield return new WaitForSeconds(1f);
      }
   }
}
