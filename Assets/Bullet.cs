using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Something fired from a GunMuzzle.
/// Plays a sound and gets forced out on Start(), once the prefab is instantiated and enabled.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
   [Tooltip("Initial shot speed")]
   [SerializeField] float m_Impulse = 1000f;
   [SerializeField] float m_Lifespan = 20f; // How long the bullet should live before being destroyed

   Rigidbody m_Rigidbody;

   private void Start()
   {
      m_Rigidbody = GetComponent<Rigidbody>();
      m_Rigidbody.AddRelativeForce(new Vector3(0, 0, 1) * m_Impulse, ForceMode.Impulse);

      Destroy(gameObject, m_Lifespan);
   }

   private void FixedUpdate()
   {
      
   }
}
