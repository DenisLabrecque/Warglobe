using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Something fired from a GunMuzzle.
/// Plays a sound and gets forced out on Start(), once the prefab is instantiated and enabled.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class Bullet : MonoBehaviour
{
   public const float MAX_UNDERWATER_DIST = -20f; // How far under water a bullet can go without exploding

   [Tooltip("Initial shot speed")]
   [SerializeField] float m_Impulse = 1000f;
   [SerializeField] float m_Damage = 1000f;
   [SerializeField] float m_Lifespan = 20f; // How long the bullet should live before being destroyed

   Target m_Target;
   Rigidbody m_Rigidbody;
   AudioSource m_Audio; // For the explosion sound
   bool m_HasCollided = false;

   private void Awake()
   {
      m_Rigidbody = GetComponent<Rigidbody>();
      m_Audio = GetComponent<AudioSource>();
   }

   private void Start()
   {
      m_Rigidbody.AddRelativeForce(new Vector3(0, 0, 1) * m_Impulse, ForceMode.Impulse);
      PointForwards();
      Destroy(gameObject, m_Lifespan);
   }

   private void FixedUpdate()
   {
      Gravity.Gravitate(m_Rigidbody);
      ExplodeIfUnderwater();
   }

   private void LateUpdate()
   {
      PointForwards();
   }


   /// <summary>
   /// Set the originator of the shot. Hits to this target are ignored.
   /// </summary>
   /// <param name="originator">The shooter of this bullet.</param>
   public void IgnoreHitsOn(Target originator)
   {
      m_Target = originator;
   }


   /// <summary>
   /// Turn the bullet to point forwards along the Y axis according to its velocity.
   /// </summary>
   private void PointForwards()
   {
      // Rotate bullet to face where it is heading
      gameObject.transform.rotation = Quaternion.LookRotation(m_Rigidbody.velocity, new Vector3(0, 1));
   }


   /// <summary>
   /// When the bullet is a certain distance underwater, it must explode.
   /// </summary>
   private void ExplodeIfUnderwater()
   {
      float altitude = Planet.Singleton.AltitudeAboveSea(m_Rigidbody.transform.position);
      if (altitude < MAX_UNDERWATER_DIST)
      {
         Explode();
      }
   }


   /// <summary>
   /// Detect when the bullet hits something.
   /// </summary>
   /// <param name="collision"></param>
   void OnCollisionEnter(Collision collision)
   {
      if (m_HasCollided == false)
      {
         // Play a sound if the colliding objects had a big impact.
         if (collision.relativeVelocity.magnitude > 2)
         {
            Target targetHit = collision.gameObject.GetComponent<Target>();

            // Hit oneself
            if(m_Target != null && targetHit == m_Target)
            {
               // Oops, let's ignore hits to the originator
            }
            // Hit terrain or something
            else if (targetHit == null)
            {
               Explode();
            }
            // Hit a legitimate target
            else
            {
               // Debug-draw all contact points and normals
               foreach (ContactPoint contact in collision.contacts)
               {
                  Debug.Log("HIT! " + collision.gameObject);
                  Debug.DrawRay(contact.point, contact.normal, Color.white);

                  m_HasCollided = true;
                  Explode(targetHit);
               }
            }
         }
      }
   }


   /// <summary>
   /// Explode this as a bomb, and damage the target it hit.
   /// </summary>
   /// <param name="target">The target to damage</param>
   public void Explode(Target target)
   {
      target.Damage(m_Damage);
      Explode();
   }

   /// <summary>
   /// The bullet explodes; does not damage the target, but should be called to complete the explosion.
   /// Instantiates an AudioSource where the bullet hit.
   /// </summary>
   public void Explode()
   {
      GameObject empty = new GameObject();
      AudioSource audio = empty.AddComponent<AudioSource>();
      audio = m_Audio;
      Instantiate(empty, gameObject.transform.position, gameObject.transform.rotation, null);
      audio.Play();
      //AudioSource audio = Instantiate(m_Audio, gameObject.transform.position, gameObject.transform.rotation, null);
      //audio.Play();
      Destroy(gameObject);
   }
}
