using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Languages supported by the application. Changing the number of languages requires changing the number of translations for each phrase.
/// 
/// Denis Labrecque
/// December 2018
/// </summary>
public enum Language { English, Francais, Magyar }

/// <summary>
/// Manage all translation strings in the app.
/// First initialize with the language wanted; a dictionary of strings for that language will be created and can be used throughout the application.
/// To change languages, initialize again.
/// 
/// Denis Labrecque
/// December 17, 2018
/// </summary>
public static class Multilang
{
   private static List<Phrase> m_Translations; // Temporary initialization list
   private static Dictionary<string, string> m_CompressedTranslations = new Dictionary<string, string>(); // Compressed runtime list
   private static bool m_IsInitialized = false;
   private static Language ? m_CurrentLanguage = null;

   /// <summary>
   /// Main property to access a string in a specific language. Use only after initialization in a language.
   /// </summary>
   public static Dictionary<string, string> Text { get { return m_CompressedTranslations; } }

   /// <summary>
   /// Mark the language strings as done.
   /// </summary>
   public static bool IsInitialized { get { return m_IsInitialized; } }

   /// <summary>
   /// Which language the text strings are currently initialized in.
   /// Returns null if no language was set.
   /// </summary>
   public static Language ? Language { get { return m_CurrentLanguage; } }
   
   /// <summary>
   /// Initialize this static class with a dictionary of phrases for the wanted language; other language phrases are discarded.
   /// </summary>
   /// <param name="tongue">Language enumeration code</param>
   public static void Initialize(Language tongue)
   {
      // Insert translations here
      m_Translations = new List<Phrase>() {

         // Language names
         new Phrase("lang_english", new string [] {"English", "Anglais", "Angol"}),
         new Phrase("lang_français", new string [] {"French", "Français", "Francia"}),
         new Phrase("lang_magyar", new string [] {"Hungarian", "Hongrois", "Magyar"}),

         // Screen titles
         new Phrase("h1_play", new string[] { "Play", "Jouer", "Játszani"}),
         new Phrase("h1_players", new string[] { "Players", "Joueurs", "Játékosok"}),
         new Phrase("h1_setup", new string[] { "Setup", "Configuration", "Konfigurációs"}),
         new Phrase("h1_newPlayer", new string[] { "New Player", "Nouveau joueur", "Új játékos"}),
         new Phrase("h1_language", new string[] {"Language", "Langue", "Nyelv"}),
         new Phrase("credits", new string[] {"Credits", "Attribution", "Jóváírás" }),
         new Phrase("sounds", new string[] {"Sounds", "Sons", "Hangok"}),
         new Phrase("you_win", new string[] {"You Win!", "Vous gagnez!", "Nyersz!" }),

         // Button titles
         new Phrase("btn_newPlayer", new string[] {"New Player", "Nouveau joueur", "Új játékos"}),
         new Phrase("btn_callsign", new string[] {"Callsign", "Indicatif", "Hívójel"}),
         new Phrase("btn_country", new string[] {"Country", "Pays", "Ország" }),
         new Phrase("btn_settings", new string[] {"Settings", "Paramètres", "Beállítások"}),
         new Phrase("btn_submit", new string[] {"Submit", "Soumettre", "Belép"}),
         new Phrase("btn_back", new string[] {"Back", "Retour", "Vissza"}),

         // Text input titles
         new Phrase("txt_playerName", new string[] {"Name wanted", "Nom voulu", "Játékos neve"}),

         // Tidbits
         new Phrase("loading", new string[] {"Loading", "Chargement", "Betöltése" }),
         new Phrase("radar_on", new string[] {"on", "allumé", "a"}),
         new Phrase("radar_off", new string[] {"off", "fermé", "ki"}),
         new Phrase("no_weapon", new string[] {"none", "aucune", "nincs"}),
         new Phrase("you", new string[] {"You", "Vous", "Ön"}),
         new Phrase("projectile_short", new string[] {"PROJ", "PROJ", "LÖV"})
      };

      // Create single-language string dictionary
      m_CompressedTranslations.Clear(); // Remove any previous initialisation
      foreach(Phrase text in m_Translations)
      {
         m_CompressedTranslations.Add(text.Name, text.Text(tongue));
      }

      // Save memory
      m_Translations.Clear();

      // Mark process as done
      m_IsInitialized = true;
      m_CurrentLanguage = tongue;
   }

   /// <summary>
   /// A simple class to contain a number of different translations for the same string.
   /// </summary>
   private class Phrase
   {
      string m_Name;
      string [] m_Phrases;

      public string Name { get { return m_Name; } }

      public string Text(Language tongue)
      {
         return m_Phrases[(int)tongue];
      }

      /// <summary>
      /// Constructor
      /// </summary>
      /// <param name="phrases">A phrase for each language needed</param>
      public Phrase(string name, string [] phrases)
      {
         m_Name = name;

         m_Phrases = new string[Enum.GetNames(typeof(Language)).Length];
         m_Phrases = phrases;
      }
   }
}
