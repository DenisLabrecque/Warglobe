using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UIScreens;

/// <summary>
/// Disable all targets before the game starts.
/// </summary>
public class GameTypeAndMode : MonoBehaviour
{
   static List<Target> m_Targets = new List<Target>();

   private void Awake()
   {
      m_Targets = FindObjectsOfType<Target>().ToList();

      if (PlayerPrefs.HasKey(Preferences.LANGUAGE))
      {
         Multilang.Initialize((Language)PlayerPrefs.GetInt(Preferences.LANGUAGE));
         Debug.Log("Current language " + Multilang.Language);
      }
   }

   private void Start()
   {
      EnableTargets(false);
   }

   /// <summary>
   /// Enable or disable all targets.
   /// </summary>
   private static void EnableTargets(bool doEnable)
   {
      // Activate or deactivate
      foreach(Target target in m_Targets)
      {
         target.gameObject.SetActive(doEnable);
      }

      // Attach the camera to the vehicle
      if(doEnable)
      {
         UserInput.Player1Vehicle.AttachCamera(SingleCamera.Camera1);
      }
      // Attach the camera to the UI
      else
      {
         UISystem.AttachCamera(SingleCamera.Camera1);
      }

      //Vehicle player1 = UserInput.Player1Vehicle;
      //Vector3 origin = player1.transform.position;
      //Vector3 newPosition = new Vector3(origin.x + 500, origin.y, origin.z);

      //Vehicle player2 = Instantiate(player1, newPosition, player1.transform.rotation);

      //SingleCamera.SetCameraLayout(2);

      //player2.AttachCamera(SingleCamera.Camera2);
   }

   public void EnableTargets()
   {
      EnableTargets(true);
   }

   public void DisableTargets()
   {
      EnableTargets(false);
   }

   public void SetLanguage(Language language)
   {
      Multilang.Initialize(language);
      TranslationString.UpdateAllTextStrings();
      Debug.Log("Language set is " + Multilang.Language);
      PlayerPrefs.SetInt(Preferences.LANGUAGE, (int)Language.English);
   }

   public void SetLanguageEnglish()
   {
      SetLanguage(Language.English);
   }

   public void SetLanguageFrench()
   {
      SetLanguage(Language.Francais);
   }

   public void SetLanguageHungarian()
   {
      SetLanguage(Language.Magyar);
   }
}
