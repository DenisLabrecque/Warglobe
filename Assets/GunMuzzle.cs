using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place where bullets are instantiated.
/// </summary>
public class GunMuzzle : MonoBehaviour
{
   [SerializeField] GameObject m_ShellPrefab;
   //[SerializeField] float ReloadTime = 1f; // TODO

   /// <summary>
   /// Seconds in the future when the shot will be reloaded
   /// </summary>
   public float TimeTillReloaded {
      get {
         return 0f;
      }
   }

   public void Fire()
   {
      GameObject bullet = Instantiate(m_ShellPrefab, gameObject.transform.position, gameObject.transform.rotation);
   }
}
