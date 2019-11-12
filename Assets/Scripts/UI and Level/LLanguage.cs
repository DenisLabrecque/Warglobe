using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Level handler for language selection.
/// Presents three choices for language selection if a language has not yet been selected.
/// 
/// Denis Labrecque
/// December 2018
/// </summary>
public class LLanguage : LPanelController
{
   [SerializeField] Button2 b_English;
   [SerializeField] Button2 b_Francais;
   [SerializeField] Button2 b_Magyar;

   void Awake()
   {
      b_English.onClick.AddListener(SetEnglish);
      b_Francais.onClick.AddListener(SetFrancais);
      b_Magyar.onClick.AddListener(SetMagyar);

      if(PlayerPrefs.HasKey(Preferences.LANGUAGE))
      {
         Multilang.Initialize((Language)PlayerPrefs.GetInt(Preferences.LANGUAGE));
         Debug.Log("Current language " + Multilang.Language);

         m_IsDone = true;
      }
   }

   #region Click Events

   private void SetEnglish()
   {
      Multilang.Initialize(Language.English);
      TranslationString.UpdateAllTextStrings();
      Debug.Log("Language set is " + Multilang.Language);

      PlayerPrefs.SetInt(Preferences.LANGUAGE, (int)Language.English);
      m_IsDone = true;
   }

   private void SetFrancais()
   {
      Multilang.Initialize(Language.Francais);
      TranslationString.UpdateAllTextStrings();
      Debug.Log("Language set is " + Multilang.Language);

      PlayerPrefs.SetInt(Preferences.LANGUAGE, (int)Language.Francais);
      m_IsDone = true;
   }

   private void SetMagyar()
   {
      Multilang.Initialize(Language.Magyar);
      TranslationString.UpdateAllTextStrings();
      Debug.Log("Language set is " + Multilang.Language);

      PlayerPrefs.SetInt(Preferences.LANGUAGE, (int)Language.Magyar);
      m_IsDone = true;
   }

   #endregion
}
