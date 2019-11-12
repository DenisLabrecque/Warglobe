using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// List all plants and de-activate them
/// </summary>
public class QualitySettingsControl : MonoBehaviour
{
   List<Plant> m_Plants = new List<Plant>();

   // deactivate all grass
   void Awake()
   {
      m_Plants = Resources.FindObjectsOfTypeAll<Plant>().ToList<Plant>();

      foreach(Plant plant in m_Plants)
      {
         plant.gameObject.SetActive(false);
      }
   }
}
