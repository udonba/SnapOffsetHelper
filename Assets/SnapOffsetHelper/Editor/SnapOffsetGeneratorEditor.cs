using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace Udonba.SnapOffsetHelper
{
    /// <summary>
    /// Editor extension for SnapOffsetGenerator
    /// </summary>
    [CustomEditor(typeof(SnapOffsetGenerator))]
    public class SnapOffsetGeneratorEditor : Editor
    {
        private new SnapOffsetGenerator target = null;
        private bool deleteInstanceRoot = true;
        private string[] displayDirectories = null;
        private bool needUpdateDirectories = true;

        private GUIContent label_GripTransform
            = new GUIContent("Grip Transform", "Grabber's gripTransform, or offset transform (inside grabbable object)");
        private GUIContent label_DeleteInstance
            = new GUIContent("Delete Instance", "Delete instance-root object after generate prefab.");
        private GUIContent label_SaveDirectory 
            = new GUIContent("Generate to", "'Prefabs' directory, to Generate *.prefab file.");
        private GUIContent label_GenerateButton
            = new GUIContent("Generate SnapOffset Prefab");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorApplication.projectChanged -= EditorApplication_projectChanged;
            EditorApplication.projectChanged += EditorApplication_projectChanged;

            target = base.target as SnapOffsetGenerator;
                                                  
            UpdateDirectoriesIfNeed();
            if (target.PrefabDirectories == null || target.PrefabDirectories.Length == 0
                || this.displayDirectories == null || this.displayDirectories.Length == 0)
            {
                needUpdateDirectories = true;
            }

            target.SelectedIndex
                = EditorGUILayout.Popup(label_SaveDirectory, target.SelectedIndex, this.displayDirectories);

            target.GripTransform
                = EditorGUILayout.ObjectField(label_GripTransform, target.GripTransform, typeof(Transform), true) as Transform;
            
            deleteInstanceRoot = EditorGUILayout.Toggle(label_DeleteInstance, this.deleteInstanceRoot);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(label_GenerateButton, GUILayout.MaxWidth(400)))
            {
                if (target.GripTransform == null)
                {
                    Debug.Log(SnapOffsetGenerator.Prefix + "<Color=Red>Need attach 'Grip Transform' field to generate SnapOffset.</Color>");
                    return;
                }

                GeneratePrefab();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

        }

        /// <summary>
        /// Update directories list if need.
        /// </summary>
        private void UpdateDirectoriesIfNeed()
        {
            if (!needUpdateDirectories)
                return;

            target.UpdatePrefabDirectories();
            this.displayDirectories = target.GetDisplayDirectories();

            needUpdateDirectories = false;
        }

        /// <summary>
        /// Generate SnapOffset prefab.
        /// </summary>
        private void GeneratePrefab()
        {
            var param = target.GetSnapOffsetParams();
            var obj = target.CreateSnapOffsetObject(param);
            var path = target.GetSavePath(param.Name);

            try
            {
                // User can undo
                PrefabUtility.SaveAsPrefabAssetAndConnect(obj, path, InteractionMode.UserAction);

                var fileName = Path.GetFileName(path);
                Debug.LogFormat(SnapOffsetGenerator.Prefix + "Success. Generated '{0}'. \nPath=\"{1}\"", fileName, path);
            }
            catch (System.Exception ex)
            {
                Debug.LogFormat(SnapOffsetGenerator.Prefix + "<Color=Red>Failed to generate prefab.</Color>\nPath=\"{0}\"\n{1}", path, ex);
            }

            if (deleteInstanceRoot && obj != null)
                DestroyImmediate(obj, false);
        }

        private void EditorApplication_projectChanged()
        {
            // To list-up new Directory
            needUpdateDirectories = true;
        }
    }
}