using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warglobe
{
   /// <summary>
   /// Place where bullets are instantiated.
   /// </summary>
   public class GunMuzzle : MonoBehaviour
   {
      [SerializeField] GameObject _shellPrefab;

      Target _target; // Parent target shooter

      private void Awake()
      {
         _target = GetComponentInParent<Target>();
         if (_target == null)
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
         GameObject bullet = Instantiate(_shellPrefab, gameObject.transform.position, gameObject.transform.rotation);
         bullet.GetComponent<Bullet>().IgnoreHitsOn(_target); // The shooter shouldn't get shot by his own bullets in general
      }
   }
}