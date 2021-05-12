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
   [SerializeField][Range(0,1)] float _density = 0.4f;
   private float _densityInverse;
   [SerializeField] public Vector3 _dimensions = new Vector3(10f, 4f, 3f);

   private Rigidbody _rigidbody;
   private List<FlotationPoint> _corners;
   private List<float> _flotationForces;
   private float _totalVolume;
   private float _quadrantVolume; // One fourth of the total submersible volume
   private const float WaterDrag = 2f;

   /// <summary>
   /// The average of submerged percentages of all flotation points.
   /// Useful for setting drag as an overall value (not done here so that this does not affect flight).
   /// </summary>
   public float PercentSubmerged {
      get {
         float totalPercent = 0;
         foreach(FlotationPoint point in _corners)
            totalPercent += point.SubmergedPercent;
         return totalPercent * 0.25f;
      }
   }

   public float PercentNotSubmerged => 1f - PercentSubmerged;

   internal void Sink()
   {
      foreach(FlotationPoint corner in _corners)
         corner.Sink(1f);
   }

   /// <summary>
   /// Manages a flotation area.
   /// </summary>
   class FlotationPoint
   {
      Vector3 _position;
      FlotationArea _flotationArea;
      float _actualFloatFactor = 1f; // A percent of the total flotation force wanted; used for sinking the FlotationArea
      float? _force = null;
      float? _lastForce = null;

      /// <summary>
      /// Get the depth of this flotation point.
      /// </summary>
      public float Depth => Planet.Singleton.AltitudeAboveSea(SubmergedPosition);

      /// <summary>
      /// The quadrant of submerged volume is estimated from the submerged height of its flotation point corner
      /// </summary>
      public float SubmergedVolume {
         get {
            return _flotationArea._quadrantVolume * SubmergedPercent;
         }
      }

      public float SubmergedPercent {
         get {
            return Utility.Percent(Depth, -_flotationArea._dimensions.y, PercentMode.Clamp0To1);
         }
      }

      /// <summary>
      /// Get the position of this flotation point as calculated from the offset of the flotation area.
      /// </summary>
      public Vector3 SubmergedPosition {
         get {
            Transform transform = _flotationArea.gameObject.transform;
            return transform.position +
                  (transform.right * _position.x) +
                  (transform.forward * _position.z) +
                  (transform.up * _position.y);
         }
      }

      /// <summary>
      /// Find the direction in which flotation works directly against gravity.
      /// </summary>
      public Vector3 Upwards => -(Gravity.GravityCenter - SubmergedPosition);

      // Constructor
      public FlotationPoint(Vector3 location, FlotationArea flotationArea)
      {
         _position = location;
         _flotationArea = flotationArea;
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
            float flotationForce = SubmergedVolume * _flotationArea._densityInverse * _actualFloatFactor;
            _flotationArea._rigidbody.AddForceAtPosition(Upwards * flotationForce, SubmergedPosition);
            _flotationArea._rigidbody.drag = WaterDrag;
            _flotationArea._rigidbody.angularDrag = WaterDrag;

            // Show debugging
            float maxForce = _flotationArea._quadrantVolume * _flotationArea._densityInverse;
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
         _actualFloatFactor -= sinkPercent;
         _actualFloatFactor = Mathf.Clamp01(_actualFloatFactor);
      }
   }

   // Start is called before the first frame update
   void Start()
   {
      _rigidbody = GetComponentInParent<Rigidbody>();
      _totalVolume = _dimensions.x * _dimensions.y * _dimensions.z;
      _quadrantVolume = _totalVolume * 0.25f;
      _densityInverse = 1 - _density;

      // Add the four flotation points at the corners of the flotation area dimensions
      _corners = new List<FlotationPoint>()
      {
         // Front left
         new FlotationPoint(new Vector3(_dimensions.x / 2f, -_dimensions.y / 2f, -_dimensions.z / 2f), this),
         // Front right
         new FlotationPoint(new Vector3(_dimensions.x / 2f, -_dimensions.y / 2f, _dimensions.z / 2f), this),
         // Back left
         new FlotationPoint(new Vector3(-_dimensions.x / 2f, -_dimensions.y / 2f, _dimensions.z / 2f), this),
         // Back right
         new FlotationPoint(new Vector3(-_dimensions.x / 2f, -_dimensions.y / 2f, -_dimensions.z / 2f), this)
      };
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
      foreach (FlotationPoint point in _corners)
      {
         point.CalculateAndApplyFlotation();
      }
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
      Gizmos.DrawWireCube(Vector3.zero, _dimensions);

      // Draw rays representing the upwards forces of flotation
      if (_corners != null) // Maybe happens when the script is deactivated
      {
         for (int i = 0; i < _corners.Count; i++)
         {
            Gizmos.DrawRay(Vector3.zero, Vector3.up); // .DrawRay(Vector3.zero, transform.position, Color.red);
         }
      }

      Gizmos.matrix = oldMatrix;
   }
}
