using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

/// <summary>
/// Add this script to any DecalProjector GameObject to add roundels according to the Country of a Target.
/// </summary>
[ExecuteAlways]
public class Roundel : MonoBehaviour
{
   Country m_Country;
   DecalProjector m_DecalProjector;

   private void Start()
   {
      Target target = GetComponentInParent<Target>();
      m_Country = target.Country;
      m_DecalProjector = GetComponent<DecalProjector>();
      m_DecalProjector.material = m_Country.Roundel;
   }

   private void Update()
   {
      if(Application.isPlaying == false)
      {
         m_DecalProjector.material = m_Country.Roundel;
      }
   }
}
