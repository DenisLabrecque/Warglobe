using UnityEngine;

/// <summary>
/// Attach to a target that is an objective in the game.
/// </summary>
public class ObjectiveKillTarget : Objective
{
   [SerializeField] Target m_Target;

   private void Awake()
   {
      // Automatically assign the target component if this was not done manually
      Target target = GetComponent<Target>();
      if (target)
         m_Target = target;
   }

   protected override bool IsObjectiveAccomplished()
   {
      if(m_Target.IsDead && SubObjectivesAccomplished)
      {
         m_IsAccomplished = true;
         m_Accomplished.Invoke();
         return true;
      }
      else
      {
         return false;
      }
   }
}
