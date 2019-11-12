using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointTowards : MonoBehaviour
{
   public Transform target;

   void Update()
   {
      // Rotate the camera every frame so it keeps looking at the target
      transform.LookAt(target);
   }
}
