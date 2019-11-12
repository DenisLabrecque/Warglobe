using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

/// <summary>
/// Set a string to the correct translation reference on awake for a TMPro text.
/// 
/// Denis Labrecque
/// December 2018
/// </summary>
[RequireComponent(typeof(TextMeshProUGUI))]
public class TranslationString : MonoBehaviour
{
   private static List<TranslationString> m_TranslationStrings = new List<TranslationString>();
   TextMeshProUGUI m_TextField;
   [SerializeField] string m_WantedString = "loading";

   void Start()
   {
      m_TextField = gameObject.GetComponent<TextMeshProUGUI>();

      UpdateString();
   }

   /// <summary>
   /// Update the string text according to the present Multilang language setting.
   /// </summary>
   public void UpdateString()
   {
      if(Multilang.IsInitialized)
      {
         m_TextField.text = Multilang.Text[m_WantedString];
      }
      else
      {
         Multilang.Initialize(Language.English); // Temporarily initialize
         Debug.Log("Multilang was temporarily initialized to English because no language was set before fetching string: " + m_WantedString);
      }
   }

   /// <summary>
   /// Find all translation strings in the scene and update them to match the current language.
   /// </summary>
   public static void UpdateAllTextStrings()
   {
      // List all root (parent) game objects
      List<GameObject> gameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().ToList();

      // Go through each parent in the scene
      foreach(GameObject gObject in gameObjects)
      {
         List<TranslationString> translationStrings = gObject.GetComponentsInChildren<TranslationString>().ToList();

         // Go through each translation string component and update the text to the current language's
         foreach(TranslationString tString in translationStrings)
         {
            tString.UpdateString();
         }
      }
   }
}
