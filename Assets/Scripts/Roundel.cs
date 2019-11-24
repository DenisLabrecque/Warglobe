using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[ExecuteAlways]
public class Roundel : MonoBehaviour
{
   Country m_Country;
   DecalProjector m_DecalProjector;

   Target target;

   [ExecuteAlways]
   void Start()
   {
      target = GetComponentInParent<Target>();
      m_Country = target.Country;


      m_DecalProjector = GetComponent<DecalProjector>();
      m_DecalProjector.material = m_Country.Roundel;
   }

   [ExecuteAlways]
   private void Update()
   {
      Debug.Log("Target is: " + m_Country.Name);
      m_DecalProjector.material = m_Country.Roundel;
   }
}
