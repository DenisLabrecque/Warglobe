using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transform the sun lamp so it rotates at a certain speed.
/// </summary>
public class SunOrbit : MonoBehaviour {

   [SerializeField] float m_DaySeconds = 260f; // How long the orbit should last in seconds

   float m_DegreesPerSecond; // Speed the planet must attain to make a complete revolution in the specified time

   #region Unity Methods

   void Start()
   {
      // Precompute how many degrees must be traversed in one second
      m_DegreesPerSecond = CalculateDegreesPerSecond(m_DaySeconds);
   }

   void Update()
   {
      // Rotate the sun around the planet (a sun's position does not matter)
      transform.Rotate(transform.position, m_DegreesPerSecond * Time.deltaTime);
   }

   #endregion

   #region Private Methods

   /// <summary>
   /// Find how many degrees per second an orbit must traverse in order to make a rotation in the sent time.
   /// </summary>
   /// <param name="daySeconds"></param>
   private float CalculateDegreesPerSecond(float daySeconds)
   {
      return daySeconds / 360;
   }

   #endregion
}
