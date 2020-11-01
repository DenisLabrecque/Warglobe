using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just add this script to something you want to gravitate.
/// </summary>
public class Gravitate : MonoBehaviour
{
   Rigidbody _rigidbody;

   // Start is called before the first frame update
   void Start()
   {
      _rigidbody = GetComponentInChildren<Rigidbody>();
   }

   void FixedUpdate()
   {
      Gravity.Gravitate(_rigidbody);
   }
}
