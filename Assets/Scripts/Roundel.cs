using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Add this script to any DecalProjector GameObject to add roundels according to the Country of a Target.
/// </summary>
[ExecuteAlways]
public class Roundel : MonoBehaviour
{
   Target m_Target;
   Faction m_Country;
   DecalProjector m_DecalProjector;

   private void Start()
   {
      m_Target = GetComponentInParent<Target>();
      m_Country = Faction.m_CountryList[m_Target.m_CountryName];
      m_DecalProjector = GetComponent<DecalProjector>();
      m_DecalProjector.material = m_Country.Roundel;
   }

   private void Update()
   {
      if(Application.isPlaying == false)
      {
         //m_Country = Faction.m_CountryList[m_Target.m_CountryName];
         //Debug.Log("Material: " + m_DecalProjector.material);
         //Debug.Log("Target country: " + m_Country.name);
         m_DecalProjector.material = Faction.m_CountryList[Faction.CountryName.Gomer].Roundel;
         //Debug.Log("Material: " + m_DecalProjector.material);
      }
   }
}
