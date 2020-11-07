using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Warglobe
{
   /// <summary>
   /// Something fired from a GunMuzzle.
   /// Plays a sound and gets forced out on Start(), once the prefab is instantiated and enabled.
   /// </summary>
   [RequireComponent(typeof(Rigidbody))] // Gravitation and physics
   [RequireComponent(typeof(AudioSource))] // Explosion sound
   public class Bullet : MonoBehaviour
   {
      public const float MAX_UNDERWATER_DIST = -20f; // How far under water a bullet can go without exploding

      [Tooltip("Initial shot speed")]
      [SerializeField] float m_Impulse = 1000f;
      [SerializeField] float m_Damage = 1000f;
      [SerializeField] float m_DamageRadius = 50f;
      [SerializeField] float m_Lifespan = 20f; // How long the bullet should live before being destroyed

      [Tooltip("Sounds")]
      [SerializeField] AudioClip m_TargetHit;
      [SerializeField] AudioClip m_SurfaceExplosion;
      [SerializeField] AudioClip m_UnderwaterExplosion;

      Target m_Target;
      Rigidbody m_Rigidbody;
      AudioSource m_Audio; // For the explosion sound
      bool m_HasCollided = false;
      public enum ExplosionType { surface, underwater, targetHit }

      private void Awake()
      {
         m_Rigidbody = GetComponent<Rigidbody>();
         m_Audio = GetComponent<AudioSource>();
         m_Audio.playOnAwake = false;
         m_Audio.loop = false;
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
         //gameObject.transform.rotation = Quaternion.LookRotation(m_Rigidbody.velocity, new Vector3(0, 1));
      }


      /// <summary>
      /// When the bullet is a certain distance underwater, it must explode.
      /// </summary>
      private void ExplodeIfUnderwater()
      {
         float altitude = Planet.Singleton.AltitudeAboveSea(m_Rigidbody.transform.position);
         if (altitude < MAX_UNDERWATER_DIST)
         {
            Explode(ExplosionType.underwater);
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
               if (m_Target != null && targetHit == m_Target)
               {
                  // Oops, let's ignore hits to the originator
                  Debug.Log("Hit originator");
               }
               // Hit a surface
               else if (targetHit == null)
               {
                  //Debug.Log("Hit surface");
                  Explode(ExplosionType.surface);
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

      //private void OnTriggerEnter(Collider other)
      //{
      //   // Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
      //   Collider[] colliders = Physics.OverlapSphere(transform.position, m_DamageRadius);

      //   // Go through all the colliders
      //   for (int i = 0; i < colliders.Length; i++)
      //   {
      //      if(colliders[i].gameObject.tag == "land")
      //      {
      //         Explode(ExplosionType.surface);
      //         return;
      //      }
      //      else if(colliders[i].gameObject.tag == "water")
      //      {
      //         // Don't explode yet, wait for the FixedUpdate to catch this underwater
      //         return;
      //      }
      //      else
      //      {
      //         // Add an explosion force
      //         Rigidbody rigidbody = colliders[i].GetComponentInParent<Rigidbody>();

      //         if (rigidbody)
      //         {
      //            // Add an explosion force.
      //            rigidbody.AddExplosionForce(m_Damage, transform.position, m_DamageRadius);

      //            // Find the Target script associated with the rigidbody.
      //            Target target = rigidbody.GetComponent<Target>();

      //            // Damage the target
      //            if (target)
      //            {
      //               Explode(target);
      //            }
      //            // If there is no Target script attached to the gameobject, go on to the next collider.
      //            else
      //            {
      //               continue;
      //            }
      //         }
      //      }
      //   }
      //}

      private float CalculateDamage(Vector3 targetPosition)
      {
         // Create a vector from the shell to the target.
         Vector3 explosionToTarget = targetPosition - transform.position;

         // Calculate the distance from the shell to the target.
         float explosionDistance = explosionToTarget.magnitude;

         // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
         float relativeDistance = (m_DamageRadius - explosionDistance) / m_DamageRadius;

         // Calculate damage as this proportion of the maximum possible damage.
         float damage = relativeDistance * m_Damage;

         // Make sure that the minimum damage is always 0.
         damage = Mathf.Max(0f, damage);

         return damage;
      }

      /// <summary>
      /// Explode this as a bomb, and damage the target it hit.
      /// </summary>
      /// <param name="target">The target to damage</param>
      public void Explode(Target target)
      {
         // Calculate the amount of damage the target should take based on its distance from the shell.
         float damage = CalculateDamage(target.Rigidbody.position);

         // Deal this damage to the Target
         target.Damage(damage);
         float percentDamage = DGL.Math.Utility.Percent(damage, target.MaxHitpoints);
         Explode(ExplosionType.surface, percentDamage);
      }

      /// <summary>
      /// The bullet explodes; does not damage the target, but should be called to complete the explosion.
      /// Instantiates an AudioSource where the bullet hit.
      /// TODO this was colliding before start.
      /// </summary>
      public void Explode(ExplosionType type, float loudness = 1f)
      {
         if (!m_HasCollided)
         {
            // Play the explosion sound
            switch (type)
            {
               case ExplosionType.surface:
                  m_Audio.clip = m_SurfaceExplosion;
                  break;
               case ExplosionType.targetHit:
                  m_Audio.clip = m_TargetHit;
                  break;
               case ExplosionType.underwater:
                  m_Audio.clip = m_UnderwaterExplosion;
                  break;
            }
            m_Audio.volume = loudness;
            m_Audio.Play();
            //Debug.Log("Explosion! " + m_Audio.clip);

            // Stop the shell path particles, stop physics, and don't display the shell
            //gameObject.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            //gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;

            // Destroy the shell
            Destroy(gameObject, 3f);

            m_HasCollided = true;
         }
      }
   }
}