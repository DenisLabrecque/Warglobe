using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Populator))]
public class PopulatorEditor : Editor
{
   public override void OnInspectorGUI()
   {
      DrawDefaultInspector();
      Populator _populator = target as Populator;
      List<Transform> _transforms = _populator.FindContaining("RadomeSmall");

      if(GUILayout.Button("Hide Boolean Meshes"))
      {
         _populator.FindBooleansRemoveRenderer();
      }

      if(GUILayout.Button("Reset Rotations to Zero"))
      {
         _populator.ResetRotations();
      }

      // Radomes
      foreach (Transform transform in _transforms)
      {
         GUILayout.Label(transform.name);
      }

      if(GUILayout.Button("Instantiate Radomes"))
      {
         _populator.Populate();
      }
   }
}
