using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows accessing the camera from every class within the scene. There must only ever be one camera within the whole scene.
/// </summary>
public class SingleCamera : MonoBehaviour {
      
   public static Camera Camera1 { get; private set; }
   public static Camera Camera2 { get; private set; }

   void Awake()
   {
      Camera1 = FindObjectOfType<Camera>();
      Camera2 = Instantiate(Camera1);
      Camera2.GetComponent<AudioListener>().enabled = false; // Prevent having two audio listeners
      SetCameraLayout(1);
   }

   /// <summary>
   /// Set the game cameras according to the number of players.
   /// </summary>
   public static void SetCameraLayout(int numberCameras)
   {
      Mathf.Clamp(numberCameras, 1, 2);
      switch (numberCameras)
      {
         case 1:
            Camera1.rect = new Rect(0, 0, 1, 1);
            Camera2.enabled = false;
            break;
         case 2:
            Camera1.rect = new Rect(0, 0, 0.5f, 1);
            Camera2.enabled = true;
            Camera2.rect = new Rect(0.5f, 0, 0.5f, 1);
            break;
            //case 3:
            //   break;
            //case 4:
            //   break;
      }
   }
}
