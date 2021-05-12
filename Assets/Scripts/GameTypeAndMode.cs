using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UIScreens;
using System.Data.SqlClient;
using System;

namespace Warglobe
{
   /// <summary>
   /// Disable all targets before the game starts.
   /// </summary>
   public class GameTypeAndMode : MonoBehaviour
   {
      [SerializeField] GameObject m_Friendly;
      [SerializeField] GameObject m_Enemy;

      [SerializeField] GameObject m_Spawn1;
      [SerializeField] GameObject m_Spawn2;

      static List<Target> m_Targets = new List<Target>();

      private void Awake()
      {
         m_Targets = FindObjectsOfType<Target>().ToList();

         if (PlayerPrefs.HasKey(Preferences.LANGUAGE))
         {
            Multilang.Initialize((Language)PlayerPrefs.GetInt(Preferences.LANGUAGE));
            Debug.Log("Current language " + Multilang.Language);
         }
         else
         {
            Multilang.Initialize(Language.English);
         }
      }

      private void Start()
      {
         EnableTargets(false);

         SqlConnection connection = new SqlConnection("Server=localhost\\SQLEXPRESS; Database=warglobe; Integrated Security=True;");
         try
         {
            Debug.Log("Connecting...");
            connection.Open();
            Debug.Log("Connected successfully");
         }
         catch(Exception e)
         {
            throw e;
         }

         using (SqlCommand command = new SqlCommand("SELECT * from Players", connection))
         {
            var reader = command.ExecuteReader();
            while (reader.Read())
               Debug.Log((int)reader[0] + " " + (string)reader[1]);
               //Console.WriteLine((int)reader[0] + " " + (string)reader[1]);
            reader.Close();
         }

         connection.Close();
      }

      /// <summary>
      /// Enable or disable all targets.
      /// </summary>
      private static void EnableTargets(bool doEnable)
      {
         // Activate or deactivate
         foreach (Target target in m_Targets)
         {
            target.gameObject.SetActive(doEnable);
         }

         // Attach the camera to the vehicle
         if (doEnable)
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

         //NetworkManager.Instance.InstantiateShip(position: m_Spawn2.transform.position);
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
         PlayerPrefs.SetInt(Preferences.LANGUAGE, (int)language);
      }
   }
}