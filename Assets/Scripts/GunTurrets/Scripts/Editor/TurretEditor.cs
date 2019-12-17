using UnityEngine;
using UnityEditor;

namespace Turrets
{
   [CustomEditor(typeof(Turret))]
   [CanEditMultipleObjects]
   public class TurretEditor : Editor
   {
      private const float ArcSize = 10.0f;

      public override void OnInspectorGUI()
      {
         Turret turret = (Turret)target;

         DrawDefaultInspector();

         EditorGUILayout.BeginHorizontal();

         if (GUILayout.Button(new GUIContent("Auto-Populate Transforms", "Automatically search and populate the \"Turret Base\" and \"Turret Barrels\" object references.\n\nRequires a child GameObject called \"Base\" and for that GameObject to have a child named \"Barrels\".")))
         {
            turret.AutoPopulateBaseAndBarrels();
         }

         if (GUILayout.Button(new GUIContent("Clear Transforms", "Sets the \"Turret Base\" and \"Turret Barrels\" references to None.")))
         {
            turret.ClearTransforms();
         }

         EditorGUILayout.EndHorizontal();
      }

      private void OnSceneGUI()
      {
         Turret turret = (Turret)target;
         Transform transform = turret.transform;

         // Don't show turret arcs when playing, because they won't be correct.
         if (turret.m_ShowArcs && !Application.isPlaying)
         {
            if (turret.m_Barrels != null)
            {
               // Traverse
               Handles.color = new Color(1.0f, 0.5f, 0.5f, 0.1f);
               if (turret.m_LimitTraverse)
               {
                  Handles.DrawSolidArc(turret.m_Barrels.position, turret.m_Barrels.up, turret.m_Barrels.forward, turret.m_RightTraverse, ArcSize);
                  Handles.DrawSolidArc(turret.m_Barrels.position, turret.m_Barrels.up, turret.m_Barrels.forward, -turret.m_LeftTraverse, ArcSize);
               }
               else
               {
                  Handles.DrawSolidArc(turret.m_Barrels.position, turret.m_Barrels.up, turret.m_Barrels.forward, 360.0f, ArcSize);
               }

               // Elevation
               Handles.color = new Color(0.5f, 1.0f, 0.5f, 0.1f);
               Handles.DrawSolidArc(turret.m_Barrels.position, turret.m_Barrels.right, turret.m_Barrels.forward, -turret.m_MaxElevation, ArcSize);

               // Depression
               Handles.color = new Color(0.5f, 0.5f, 1.0f, 0.1f);
               Handles.DrawSolidArc(turret.m_Barrels.position, turret.m_Barrels.right, turret.m_Barrels.forward, turret.m_MaxDepression, ArcSize);
            }
            else
            {
               Handles.color = new Color(1.0f, 0.5f, 0.5f, 0.1f);
               Handles.DrawSolidArc(transform.position, transform.up, transform.forward, turret.m_LeftTraverse, ArcSize);
               Handles.DrawSolidArc(transform.position, transform.up, transform.forward, -turret.m_LeftTraverse, ArcSize);

               Handles.color = new Color(0.5f, 1.0f, 0.5f, 0.1f);
               Handles.DrawSolidArc(transform.position, transform.right, transform.forward, -turret.m_MaxElevation, ArcSize);

               Handles.color = new Color(0.5f, 0.5f, 1.0f, 0.1f);
               Handles.DrawSolidArc(transform.position, transform.right, transform.forward, turret.m_MaxDepression, ArcSize);
            }
         }
      }
   }
}