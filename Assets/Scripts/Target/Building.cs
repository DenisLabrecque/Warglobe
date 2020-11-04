using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warglobe
{
   /// <summary>
   /// Define an object that acts as a prop.
   /// </summary>
   public class Building : Target
   {

      void Awake()
      {
         base.Awake();

         GetComponent<Rigidbody>().isKinematic = true;
      }
   }
}