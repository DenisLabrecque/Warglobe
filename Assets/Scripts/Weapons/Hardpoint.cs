using UnityEngine;

namespace Warglobe
{
   public enum ProjectileWeight { Light = 10, Heavy = 20, ExtremelyHeavy = 300 }

   public class Hardpoint : MonoBehaviour
   {
      #region Member Variables

      [Header("Hardpoint Settings")]

      [Tooltip("Whether an extra battery can be externally plugged in this hardpoint")]
      [SerializeField] bool m_IsChargePoint = false;

      [Tooltip("Maximum weight rating of a weapon attached to this hardpoint")]
      [SerializeField] ProjectileWeight _capacity = ProjectileWeight.Light;
      private bool _hasFired = false;

      #endregion


      #region Properties

      /// <summary>
      /// Whether a projectile is ready to shoot at this hardpoint.
      /// </summary>
      public bool IsArmed {
         get {
            if (Projectile != null && _hasFired == false)
               return true;
            else
               return false;
         }
      }

      /// <summary>
      /// The projectile parented to this hardpoint.
      /// </summary>
      public Projectile Projectile { get; private set; } = null;

      #endregion


      #region Unity Methods

      void Awake()
      {
         Projectile = GetComponentInChildren<Projectile>();

         if (Projectile != null && !IsCompatible(Projectile)) { }
         //throw new System.Exception("Projectile incompatible with hardpoint: a projectile was assigned to hardpoint " + gameObject + " of insufficient capacity");
      }

      #endregion

      /// <summary>
      /// Check whether a projectile is light enough for the hardpoint to carry, or whether the hardpoint has the necessary connections
      /// for the wanted system (eg. battery).
      /// </summary>
      /// <param name="projectile">The projectile with a certain weight</param>
      /// <returns></returns>
      public bool IsCompatible(Projectile projectile)
      {
         if ((int)projectile.Weight <= (int)_capacity)
            return true;
         else
            return false;
      }

      /// <summary>
      /// Launch a projectile at a target from this hardpoint.
      /// </summary>
      public void Launch()
      {
         if (Projectile != null)
         {
            _hasFired = true;
            Projectile.Fire();
         }
      }

      /// <summary>
      /// Launch this projectile without allowing it to explode.
      /// </summary>
      public void Jettison()
      {
         _hasFired = true;
      }
   }
}