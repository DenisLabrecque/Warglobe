using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Provide previously chosen player names to select from.
/// 
/// Denis Labrecque
/// December 2018
/// </summary>
public class LPlayer : LPanelController
{
   [SerializeField] Button2 b_Player1;
   [SerializeField] Button2 b_Player2;
   [SerializeField] Button2 b_Player3;

   void Start()
   {
      // Add listeners
      b_Player1.onClick.AddListener(Player1Click);
      b_Player2.onClick.AddListener(Player2Click);
      b_Player3.onClick.AddListener(Player3Click);

      // Get button 1 title
      string button1Name;
      if(PlayerPrefs.HasKey(Preferences.PLAYER1))
         button1Name = PlayerPrefs.GetString(Preferences.PLAYER1);
      else
         button1Name = Multilang.Text["h1_newPlayer"];

      // Get button 2 title
      string button2Name;
      if(PlayerPrefs.HasKey(Preferences.PLAYER2))
         button2Name = PlayerPrefs.GetString(Preferences.PLAYER2);
      else
         button2Name = Multilang.Text["h1_newPlayer"];

      // Get button 3 title
      string button3Name;
      if(PlayerPrefs.HasKey(Preferences.PLAYER3))
         button3Name = PlayerPrefs.GetString(Preferences.PLAYER3);
      else
         button3Name = Multilang.Text["h1_newPlayer"];

      b_Player1.GetComponentInChildren<TextMeshProUGUI>().text = button1Name;
      b_Player2.GetComponentInChildren<TextMeshProUGUI>().text = button2Name;
      b_Player3.GetComponentInChildren<TextMeshProUGUI>().text = button3Name;
   }


   public void Player1Click()
   {
      Preferences.m_CurrentPlayer = 1;
      m_IsDone = true;
      Debug.Log("Player 1 clicked; preferences current player set to " + Preferences.m_CurrentPlayer);
   }

   public void Player2Click()
   {
      Preferences.m_CurrentPlayer = 2;
      m_IsDone = true;
      Debug.Log("Player 2 clicked; preferences current player set to " + Preferences.m_CurrentPlayer);
   }

   public void Player3Click()
   {
      Preferences.m_CurrentPlayer = 3;
      m_IsDone = true;
      Debug.Log("Player 3 clicked; preferences current player set to " + Preferences.m_CurrentPlayer);
   }
}
