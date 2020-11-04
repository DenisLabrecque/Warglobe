using UnityEngine;

namespace Warglobe
{
   /// <summary>
   /// Attach to a target that is an objective in the game.
   /// </summary>
   public class ObjectiveKillTarget : Objective
   {
      [SerializeField] Target m_Target;

      private void Awake()
      {
         // Automatically assign the target component if this was not done manually
         if (m_Target == null)
         {
            Target target = GetComponent<Target>();
            if (target)
               m_Target = target;
         }
      }

      public override bool IsObjectiveAccomplished()
      {
         if (m_Target.IsDead)
         {
            m_Accomplished.Invoke();
            return true;
         }
         else
         {
            return false;
         }
      }
   }
}