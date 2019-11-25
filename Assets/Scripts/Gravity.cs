using DGL.Math;
using UnityEngine;

/// <summary>
/// Define the constants of gravitation.
/// Determine the force of gravity for an object's rigidbody (call the gravitate method as needed on fixed update).
/// 
/// Denis Labrecque
/// December 2018
/// February 2019
/// </summary>
public static class Gravity {

   public const float GRAVITY_PULL = 1.5f;
   public static Vector3 GRAVITY_CENTER = new Vector3(0,0,0);
   public const int GRAVITY_RADIUS = 160000;

   /// <summary>
   /// Call this method at every physics tick using FixedUpdate() for an object that must be gravitated towards earth.
   /// </summary>
   /// <param name="rigidbody">The one physics item to add gravity to.
   /// Because rigidbodies can contain many colliders, do not use colliders as objects to add force to,
   /// as each now collider is a force multiplier, exaggerating gravity's strength.</param>
   public static void Gravitate(Rigidbody rigidbody)
   {
      if(rigidbody.useGravity)
      {
         float gravityIntensity = Intensity(rigidbody.transform.position, rigidbody.mass);

         rigidbody.AddForce((rigidbody.position - GRAVITY_CENTER) * gravityIntensity * Time.deltaTime);

         Debug.DrawRay(rigidbody.position, GRAVITY_CENTER - rigidbody.position);
      }
   }

   /// <summary>
   /// Find the intensity of gravity at a certain position around the center of gravity.
   /// </summary>
   public static float Intensity(Vector3 position, float mass)
   {
      // Gravity = mass1 x mass2 / distance ^ 2
      // Here, the effect of gravity is fudged by cutting it off at a distance
      float intensity = Utility.Percent(
         Vector3.Distance(position, GRAVITY_CENTER),
         GRAVITY_RADIUS,
         PercentMode.Clamp0To1);

      // Force of gravity must be proportional to mass (so smaller objects fall at the same speed)
      intensity *= (mass * -GRAVITY_PULL);

      return intensity;
   }
}
