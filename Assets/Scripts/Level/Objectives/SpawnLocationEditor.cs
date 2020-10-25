//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(SpawnLocation))]
//public class SpawnLocationEditor : Editor
//{
//   public override void OnInspectorGUI()
//   {
//      DrawDefaultInspector();

//      EditorGUILayout.BeginHorizontal();

//      if (GUILayout.Button(new GUIContent("Auto-Populate Transforms", "Automatically search and populate the \"Turret Base\" and \"Turret Barrels\" object references.\n\nRequires a child GameObject called \"Base\" and for that GameObject to have a child named \"Barrels\".")))
//      {
//         //turret.AutoPopulateBaseAndBarrels();
//      }

//      if (GUILayout.Button(new GUIContent("Clear Transforms", "Sets the \"Turret Base\" and \"Turret Barrels\" references to None.")))
//      {
//         //turret.ClearTransforms();
//      }

//      EditorGUILayout.EndHorizontal();
//   }
//}
