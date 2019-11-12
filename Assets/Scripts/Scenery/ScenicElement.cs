using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Point a building towards the earth and add colliders to each building part.
/// </summary>
[ExecuteAlways]
public abstract class ScenicElement : MonoBehaviour
{
   [SerializeField] protected bool m_CustomRotation = false;

   protected void Awake()
   {
      if(m_CustomRotation == false)
      {
         PointStraightUp();
      }

      // Stop execution
      enabled = false;
   }

   /// <summary>
   /// Point an object towards 0,0,0
   /// </summary>
   protected void PointStraightUp()
   {
      Vector3 targetDir = Vector3.Cross(Vector3.zero, transform.position);

      // Move our position a step closer to the target.
      transform.LookAt(Vector3.zero, Vector3.forward);
      transform.Rotate(Vector3.left, 90);
   }
}
