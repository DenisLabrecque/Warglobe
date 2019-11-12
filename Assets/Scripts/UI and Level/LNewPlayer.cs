using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// When a player preference doesn't exist for player 1, 2, or 3, a new name must be registered for the player pref.
/// 
/// Denis Labrecque
/// December 2018
/// February 2019
/// </summary>
public class LNewPlayer : LPanelController
{
   [SerializeField] TMP_InputField inpt_WantedName;
   [SerializeField] Button2 b_Submit;

   void Awake()
   {
      inpt_WantedName.onSubmit.AddListener(SubmitName);
      b_Submit.onClick.AddListener(SubmitInput);
   }

   void Update()
   {
      // Check whether this panel is done
      switch(Preferences.m_CurrentPlayer)
      {
         case 1:
            if(PlayerPrefs.HasKey(Preferences.PLAYER1))
               m_IsDone = true;
            break;

         case 2:
            if(PlayerPrefs.HasKey(Preferences.PLAYER2))
               m_IsDone = true;
            break;

         case 3:
            if(PlayerPrefs.HasKey(Preferences.PLAYER3))
               m_IsDone = true;
            break;

         default:
            m_IsDone = false;
            break;
      }
   }

   /// <summary>
   /// Intput listener.
   /// </summary>
   private void SubmitInput()
   {
      if(Preferences.m_CurrentPlayer == 0 || Preferences.m_CurrentPlayer > 3)
         Debug.LogError("No player was selected, but player preferences have attempted to update according to a player index less or greater than the number of players that can be initialized. [Player index out of range].");

      SubmitName(inpt_WantedName.text);
   }

   /// <summary>
   /// Try to submit a string name, and set player preferences if this name is acceptable.
   /// </summary>
   /// <param name="submitString"></param>
   private void SubmitName(string submitString)
   {
      // Entered name isn't complete
      if(string.IsNullOrEmpty(submitString))
      {
         // Notify the user that we can't proceed with a null or empty name
      }
      // Entered name is complete
      else
      {
         switch(Preferences.m_CurrentPlayer)
         {
            case 1:
               PlayerPrefs.SetString(Preferences.PLAYER1, submitString);
               Debug.Log("Player 1 strored as " + submitString);
               break;

            case 2:
               PlayerPrefs.SetString(Preferences.PLAYER2, submitString);
               Debug.Log("Player 2 stored as " + submitString);
               break;

            case 3:
               PlayerPrefs.SetString(Preferences.PLAYER3, submitString);
               Debug.Log("Player 3 stored as " + submitString);
               break;
         }

         // Go to the next panel
         m_IsDone = true;
      }
   }
}
