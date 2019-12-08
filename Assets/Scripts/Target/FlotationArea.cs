using DGL.Math;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A rectangle to be put under anything that floats.
/// When it goes below sea level, it will counteract the force of gravity.
/// This implementation assumes four points, each of which controls the flotation of a quadrant of the flotation area's rectangular space.
/// </summary>
[ExecuteAlways]
public class FlotationArea : MonoBehaviour
{
   [SerializeField][Range(0,1)] float m_Density = 0.4f;
   private float m_DensityInverse;
   [SerializeField] public Vector3 m_Dimensions = new Vector3(10f, 4f, 3f);

   private Rigidbody m_Rigidbody;
   private List<FlotationPoint> m_Corners;
   private List<float> m_flotationForces;
   private float m_TotalVolume;
   private float m_QuadrantVolume; // One fourth of the total submersible volume
   private const float WATER_DRAG = 2f;

   /// <summary>
   /// The average of submerged percentages of all flotation points.
   /// Useful for setting drag as an overall value (not done here so that this does not affect flight).
   /// </summary>
   public float PercentSubmerged {
      get {
         float totalPercent = 0;
         foreach(FlotationPoint point in m_Corners)
         {
            totalPercent += point.SubmergedPercent;
         }
         return totalPercent * 0.25f;
      }
   }

   public float PercentNotSubmerged {
      get {
         return 1f - PercentSubmerged;
      }
   }

   internal void Sink()
   {
      foreach(FlotationPoint corner in m_Corners)
      {
         corner.Sink(1f);
      }
   }

   /// <summary>
   /// Manages a flotation area.
   /// </summary>
   class FlotationPoint
   {
      const float SINK_RATE = 0.1f;

      Vector3 m_position;
      FlotationArea m_flotationArea;
      float m_ActualFloatFactor = 1f; // A percent of the total flotation force wanted; used for sinking the FlotationArea
      float? m_force = null;
      float? m_lastForce = null;

      /// <summary>
      /// Get the depth of this flotation point.
      /// </summary>
      public float Depth {
         get {
            return Planet.Singleton.AltitudeAboveSea(SubmergedPosition);
         }
      }

      /// <summary>
      /// The quadrant of submerged volume is estimated from the submerged height of its flotation point corner
      /// </summary>
      public float SubmergedVolume {
         get {
            return m_flotationArea.m_QuadrantVolume * SubmergedPercent;
         }
      }

      public float SubmergedPercent {
         get {
            return Utility.Percent(Depth, -m_flotationArea.m_Dimensions.y, PercentMode.Clamp0To1);
         }
      }

      /// <summary>
      /// Get the position of this flotation point as calculated from the offset of the flotation area.
      /// </summary>
      public Vector3 SubmergedPosition {
         get {
            Transform transform = m_flotationArea.gameObject.transform;
            return transform.position +
                  (transform.right * m_position.x) +
                  (transform.forward * m_position.z) +
                  (transform.up * m_position.y);
         }
      }

      /// <summary>
      /// Find the direction in which flotation works directly against gravity.
      /// </summary>
      public Vector3 Upwards {
         get {
            return -(Gravity.GRAVITY_CENTER - SubmergedPosition);
         }
      }

      // Constructor
      public FlotationPoint(Vector3 location, FlotationArea flotationArea)
      {
         m_position = location;
         m_flotationArea = flotationArea;
      }

      /// <summary>
      /// Main function used to estimate the submerged volume of a flotation area quadrant and apply the equivalent push upwards according to Archimede's principle.
      /// To be used at every physics tick.
      /// </summary>
      public void CalculateAndApplyFlotation()
      {
         if(Depth < 0) // submerged
         {
            // Apply flotation force
            float flotationForce = SubmergedVolume * m_flotationArea.m_DensityInverse * m_ActualFloatFactor;
            m_flotationArea.m_Rigidbody.AddForceAtPosition(Upwards * flotationForce, SubmergedPosition);
            m_flotationArea.m_Rigidbody.drag = WATER_DRAG;
            m_flotationArea.m_Rigidbody.angularDrag = WATER_DRAG;

            // Show debugging
            float maxForce = m_flotationArea.m_QuadrantVolume * m_flotationArea.m_DensityInverse;
            float percentForce = Utility.Percent(flotationForce, maxForce, PercentMode.AnyPercent);
            Debug.DrawRay(SubmergedPosition, Upwards * percentForce * 0.01f, Color.blue);
         }
      }

      /// <summary>
      /// Sink this flotation area by a certain amount more.
      /// </summary>
      /// <param name="sinkPercent">The percent of sinking to apply.</param>
      public void Sink(float sinkPercent)
      {
         sinkPercent = Mathf.Clamp01(sinkPercent);
         m_ActualFloatFactor -= sinkPercent;
         m_ActualFloatFactor = Mathf.Clamp01(m_ActualFloatFactor);
      }
   }

   // Start is called before the first frame update
   void Start()
   {
      m_Rigidbody = GetComponentInParent<Rigidbody>();
      m_TotalVolume = m_Dimensions.x * m_Dimensions.y * m_Dimensions.z;
      m_QuadrantVolume = m_TotalVolume * 0.25f;
      m_DensityInverse = 1 - m_Density;

      // Add the four flotation points at the corners of the flotation area dimensions
      m_Corners = new List<FlotationPoint>()
      {
         // Front left
         new FlotationPoint(new Vector3(m_Dimensions.x / 2f, -m_Dimensions.y / 2f, -m_Dimensions.z / 2f), this),
         // Front right
         new FlotationPoint(new Vector3(m_Dimensions.x / 2f, -m_Dimensions.y / 2f, m_Dimensions.z / 2f), this),
         // Back left
         new FlotationPoint(new Vector3(-m_Dimensions.x / 2f, -m_Dimensions.y / 2f, m_Dimensions.z / 2f), this),
         // Back right
         new FlotationPoint(new Vector3(-m_Dimensions.x / 2f, -m_Dimensions.y / 2f, -m_Dimensions.z / 2f), this)
      };
   }

   // Update is called once per frame
   void Update()
   {

   }

   private void FixedUpdate()
   {
      Float();
   }

   /// <summary>
   /// Call at every physics tick to make the flotation area float in water.
   /// </summary>
   private void Float()
   {
      foreach (FlotationPoint point in m_Corners)
      {
         point.CalculateAndApplyFlotation();
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;

      //for (int i = 0; i < this.m_Corners.Count; i++)
      //{
      //   Vector3 position = SubmergedPosition(i);
      //   Vector3 upwards = Gravity.GRAVITY_CENTER - position;

      //   if (i == 0)
      //   {
      //      float depth = Planet.Singleton.AltitudeAboveSea(position);
      //      Debug.Log("Depth: " + depth);
      //   }

      //   Gizmos.DrawSphere(position, 1.5f);
      //   Debug.DrawRay(position, upwards * -1);
      //}
   }


   /// <summary>
   /// Draw gizmos when this object is selected.
   /// </summary>
   private void OnDrawGizmosSelected()
   {
      Matrix4x4 oldMatrix = Gizmos.matrix;
      Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);

      // Draw a rectangle representing the flotation area
      Gizmos.color = Color.white;
      Gizmos.DrawWireCube(Vector3.zero, m_Dimensions);

      // Draw rays representing the upwards forces of flotation
      if (m_Corners != null) // Maybe happens when the script is deactivated
      {
         for (int i = 0; i < m_Corners.Count; i++)
         {
            Gizmos.DrawRay(Vector3.zero, Vector3.up);//  .DrawRay(Vector3.zero, transform.position, Color.red);
         }
      }

      Gizmos.matrix = oldMatrix;
   }
}
