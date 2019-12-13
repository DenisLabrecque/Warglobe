using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Objective : MonoBehaviour
{
   [SerializeField] protected List<Objective> m_SubObjectives = new List<Objective>();
   [SerializeField] protected UnityEvent m_Accomplished = new UnityEvent();
   [SerializeField] public string name = "Testing";

   private void Start()
   {
      // Subscribe to the objective manager
      ObjectiveManager.Add(this);
   }

   public bool IsAccomplished {
      get {
         return IsObjectiveAccomplished();
      }
   }

   /// <summary>
   /// Whether all the objectives associated with this objective are complete. Returns true when there are no such sub-objectives.
   /// </summary>
   public bool SubObjectivesAccomplished {
      get {
         if (m_SubObjectives.Count > 0)
         {
            bool isAccomplished = true;
            foreach (Objective objective in m_SubObjectives)
            {
               if (objective.SubObjectivesAccomplished)
                  continue;
               else
               {
                  isAccomplished = false;
                  break;
               }
            }
            return isAccomplished;
         }
         else
         {
            return true;
         }
      }
   }

   /// <summary>
   /// To be implemented in the subclass.
   /// </summary>
   public abstract bool IsObjectiveAccomplished();
}