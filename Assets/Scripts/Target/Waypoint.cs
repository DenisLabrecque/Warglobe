using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A 3D point in space that represents a transit route, a landing point, or a point of interest.
/// It can contain an/many actions (such as land, refuel, attack, loiter, defend, etc.)
/// </summary>
public class Waypoint : MonoBehaviour
{
   public enum Type { Transit, City, Landmark }

   [SerializeField] Type m_WaypointType = Type.Transit;
   [SerializeField][Range(10,100)] int m_Accuracy = 20;


   #region Properties

   /// <summary>
   /// This waypoint's altitude above sea level.
   /// </summary>
   public int Altitude { get; private set; }

   #endregion


   void Awake()
   {
      Altitude = (int)Planet.Singleton.AltitudeAboveSea(gameObject);
   }
}
