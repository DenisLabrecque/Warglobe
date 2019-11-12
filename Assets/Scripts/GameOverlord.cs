using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game controller.
/// Defines the single leader of a game, what type of game is being played, etc.
/// </summary>
public class GameOverlord : MonoBehaviour {

   #region Members

   private static GameOverlord m_Singleton = null;

   #endregion


   #region Properties

   public static GameOverlord Singleton {
      get {
         return m_Singleton;
      }
   }

   #endregion


   #region Unity Methods

   void Awake()
   {
      if(m_Singleton == null)
      {
         m_Singleton = this;
      }
   }

   void Start()
   {

   }

   #endregion


   #region Public Methods


   #endregion
}
