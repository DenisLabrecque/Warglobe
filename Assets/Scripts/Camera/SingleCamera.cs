using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows accessing the camera from every class within the scene. There must only ever be one camera within the whole scene.
/// </summary>
public class SingleCamera : MonoBehaviour {
      
   public static Camera GameCam { get; private set; }

   void Start()
   {
      GameCam = FindObjectOfType<Camera>();
   }
}
