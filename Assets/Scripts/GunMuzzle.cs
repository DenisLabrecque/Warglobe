using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Place where bullets are instantiated.
/// </summary>
public class GunMuzzle : MonoBehaviour
{
   [SerializeField] GameObject m_ShellPrefab;

   Target m_Target; // Parent target shooter

   private void Awake()
   {
      m_Target = GetComponentInParent<Target>();
      if (m_Target == null)
         Debug.Log("A gun muzzle should be the child of some target.");
   }

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
      bullet.GetComponent<Bullet>().IgnoreHitsOn(m_Target); // The shooter shouldn't get shot by his own bullets in general
   }
}
