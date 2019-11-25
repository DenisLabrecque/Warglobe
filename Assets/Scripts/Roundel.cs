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
      m_Country = m_Target.Country;
      m_DecalProjector = GetComponent<DecalProjector>();

      if (Application.isPlaying)
      {
         m_DecalProjector.material = m_Country.Roundel;
      }
      else
      {
         m_DecalProjector.material = Faction.RoundelNone;
      }
   }
}
