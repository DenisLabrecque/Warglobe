using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Warglobe;

public class LPauseMenu : LPanelController
{
   [SerializeField] Button2 b_Continue;
   [SerializeField] Button2 b_Reload;

   UIPanelController m_UIController;

   void Awake()
   {
      m_UIController = GetComponentInParent<UIPanelController>();

      b_Continue.onClick.AddListener(Continue);
      b_Reload.onClick.AddListener(Reload);
   }

   void Continue()
   {
      UserInput.UnpauseGame();
   }

   void Reload()
   {
      Debug.Log("Reload called");
      //SceneManager.LoadScene("MainHDRP", LoadSceneMode.Additive);
   }
}
