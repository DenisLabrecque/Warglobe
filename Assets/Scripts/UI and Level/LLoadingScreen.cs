using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LLoadingScreen : LPanelController
{
   [SerializeField] Slider m_ProgressSlider;
   CanvasGroup m_ThisCanvas;
   float m_PreviousAlpha = 0;

   void Awake()
   {
      m_ThisCanvas = gameObject.GetComponent<CanvasGroup>();
   }

   void Update()
   {
      if(m_ThisCanvas.alpha > 0 && m_PreviousAlpha == 0)
      {
         Debug.Log("Loading the main scene");
         StartCoroutine(LoadAsync(1));
      }

      m_PreviousAlpha = m_ThisCanvas.alpha;
   }
   
   IEnumerator LoadAsync(int levelIndex)
   {
      AsyncOperation loadLevel = SceneManager.LoadSceneAsync(levelIndex);

      while(!loadLevel.isDone)
      {
         // Find our progress at the moment (progress stops at .9, so we must stretch that number)
         float progress = Mathf.Clamp01(loadLevel.progress / 0.9f);
         m_ProgressSlider.value = progress;

         yield return null;
      }
   }
}
