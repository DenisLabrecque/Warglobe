using UnityEngine;

namespace TurretDemo
{
   public class TurretTester : MonoBehaviour
   {
      public Turret[] turret;
      public Vector3 targetPos;
      public Transform targetTransform;

      [Space]
      public bool turretsIdle = false;

      private void Update()
      {
         // Toggle turret idle.
         if (Input.GetKeyDown(KeyCode.E))
            turretsIdle = !turretsIdle;

         // When a transform is assigned, pass that to the turret. If not,
         // just pass in whatever this is looking at.
         targetPos = transform.TransformPoint(Vector3.forward * 200.0f);
         foreach (Turret tur in turret)
         {
            if (targetTransform == null)
               tur.SetAimpoint(targetPos);
            else
               tur.SetAimpoint(targetTransform.position);

            tur.SetIdle(turretsIdle);
         }
      }

      private void OnDrawGizmos()
      {
         Gizmos.DrawWireSphere(targetPos, 1.0f);
      }

   }
}
