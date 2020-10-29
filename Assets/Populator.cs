using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Find empties in a mesh (by name) and populate prefabs.
/// </summary>
[ExecuteInEditMode]
public class Populator : MonoBehaviour
{
   [SerializeField] Transform _weapons;
   [SerializeField] Transform _sensors;
   [SerializeField] Transform _mesh;

   [SerializeField] GameObject _smallRadome;


   public void Populate()
   {
      // Error check
      if (_weapons == null)
         Debug.LogError("The weapon transform must be assigned");
      else if (_sensors == null)
         Debug.LogError("The sensor transform must be assigned");
      else if (_mesh == null)
         Debug.LogError("Mesh must be assigned");

      // Populate radomes
      foreach (Transform transform in SmallRadomeTransforms)
         Instantiate(_smallRadome, transform.position, transform.rotation, _sensors);
   }

   public void ResetRotations()
   {
      Transform[] children = _mesh.GetComponentsInChildren<Transform>();
      foreach(Transform child in children)
      {
         child.rotation = new Quaternion(0, 0, 0, 0);
      }
   }

   public List<Transform> SmallRadomeTransforms {
      get {
         return FindContaining(_mesh, "RadomeSmall", false);
      }
   }

   public void FindBooleansRemoveRenderer()
   {
      List<Transform> booleans = FindContaining(_mesh, "boolean");
      string bools = "";
      foreach (Transform boolean in booleans)
         bools += boolean.name + " ";
      Debug.Log("Found and unrendered " + bools);

      foreach (Transform boolean in booleans)
         boolean.GetComponent<MeshRenderer>().enabled = false;
   }

   void PopulateSmallRadomes()
   {
      string match = "RadomeSmall";
      List<Transform> empties = FindContaining(_mesh, match);

      // Delete previous instances
      foreach (Transform empty in empties)
         DeleteContaining(_mesh, match);

      // Instantiate the new instances
      //foreach (Transform empty in empties)
      //   Instantiate(_smallRadarPrefab, empty);
   }

   /// <summary>
   /// Find all children containing a certain string.
   /// </summary>
   /// <param name="name">String to match.</param>
   public List<Transform> FindContaining(Transform transform, string name, bool ignoreCase = true)
   {
      List<Transform> found = new List<Transform>();
      Transform[] children = _mesh.GetComponentsInChildren<Transform>();

      foreach (Transform child in children)
      {
         if (ignoreCase && child.name.ToLowerInvariant().Contains(name.ToLowerInvariant()))
         {
            found.Add(child);
         }
         else if (child.name.Contains(name))
            found.Add(child);
      }

      return found;
   }

   public List<Transform> FindContaining(string name, bool ignoreCase = true)
   {
      return FindContaining(_mesh, name, ignoreCase);
   }

   void DeleteContaining(Transform transform, string name, bool ignoreCase = true)
   {
      List<Transform> found = FindContaining(transform, name, ignoreCase);
      foreach (Transform find in found)
         DestroyImmediate(find.gameObject);
   }
}

[Serializable]
public class ItemLocation
{
   public GameObject _prefab;
   public Transform _position;

   public ItemLocation(Transform transform)
   {
      _position = transform;
      _prefab = null;
   }
}