using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Disable all targets before the game starts.
/// </summary>
public class GameTypeAndMode : MonoBehaviour
{
   static List<Target> m_Targets = new List<Target>();

   private void Awake()
   {
      m_Targets = FindObjectsOfType<Target>().ToList();
   }

   private void Start()
   {
      EnableTargets(true);
   }

   /// <summary>
   /// Enable or disable all targets.
   /// </summary>
   public static void EnableTargets(bool doEnable)
   {
      // Activate or deactivate
      foreach(Target target in m_Targets)
      {
         target.gameObject.SetActive(doEnable);
      }

      //Vehicle player1 = UserInput.Player1Vehicle;
      //Vector3 origin = player1.transform.position;
      //Vector3 newPosition = new Vector3(origin.x + 500, origin.y, origin.z);

      //Vehicle player2 = Instantiate(player1, newPosition, player1.transform.rotation);

      //SingleCamera.SetCameraLayout(2);

      //player2.AttachCamera(SingleCamera.Camera2);
   }
}
