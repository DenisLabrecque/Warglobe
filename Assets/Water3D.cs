using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Water3D : MonoBehaviour
{
   Mesh _mesh = null;
   Vector3[] _vertices;
   Vector3[] _normals;

   // Start is called before the first frame update
   void Start()
   {
      _mesh = GetComponent<MeshFilter>().sharedMesh;
   }

   void Update()
   {
      _vertices = _mesh.vertices;

      for (var i = 0; i < _vertices.Length; i++)
      {
         _vertices[i] += _mesh.normals[i] * Mathf.Sin(Time.time);
      }

      _mesh.vertices = _vertices;
   }

   /// <summary>
   /// Find the value of Perlin noise in 3D space.
   /// </summary>
   /// <param name="position">The position for which to resolve the noise.</param>
   /// <returns>The Perlin value.</returns>
   public static float Perlin3D(Vector3 position)
   {
      // Get all permutations of noise for x, y, z
      float ab = Mathf.PerlinNoise(position.x, position.y);
      float ac = Mathf.PerlinNoise(position.x, position.z);
      float ba = Mathf.PerlinNoise(position.y, position.x);
      float bc = Mathf.PerlinNoise(position.y, position.z);
      float ca = Mathf.PerlinNoise(position.z, position.x);
      float cb = Mathf.PerlinNoise(position.z, position.y);

      // Find the average
      return (ab + ac + ba + bc + ca + cb) / 6f;
   }
}
