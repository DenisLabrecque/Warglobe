using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Point a building towards the earth and add colliders to each building part.
/// </summary>
[ExecuteAlways]
public class PointTowardsEarth : MonoBehaviour
{
   // Start is called before the first frame update
   void Update()
   {
      Vector3 targetDir = Vector3.Cross(Vector3.zero, transform.position);

      // Move our position a step closer to the target.
      transform.LookAt(Vector3.zero, Vector3.forward);
      transform.Rotate(Vector3.left, 90);

      //// Create a list of all the mesh parts
      //List<GameObject> meshes = new List<GameObject>();
      //meshes = GetComponentsInChildren<GameObject>().ToList();

      //// Go through each game object and add the mesh collider
      //foreach(GameObject go in meshes)
      //{
      //   if(go.GetComponent<Collider>() == null)
      //   {

      //      MeshCollider meshCollider = go.AddComponent<MeshCollider>();
      //      meshCollider.sharedMesh = go.GetComponent<Mesh>();
      //      meshCollider.convex = true;
      //   }
      //}

      // Stop execution
      enabled = false;
   }
}
