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
        private const string PropPath_SelectedIndex = "selectedIndex";
        private const string PropPath_GripTransform = "gripTransform";
        private const string PropPath_CreateInstance = "createInstance";

        private new SnapOffsetGenerator target = null;
        private bool leaveInstanceRoot = true;
        private string[] displayDirectories = null;
        private bool needUpdateDirectories = true;

        private SerializedProperty prop_SelectedIndex;
        private SerializedProperty prop_GripTransform;
        private SerializedProperty prop_CreateInstance;

        private GUIContent label_SaveDirectory
            = new GUIContent("Generate to", "'Prefabs' directory, to Generate *.prefab file.");
        private GUIContent label_GripTransform
            = new GUIContent("Grip Transform", "Grabber's gripTransform, or offset transform. (inside grabbable object)");
        private GUIContent label_CreateInstance
            = new GUIContent("Create Instance", "Create instance object in scene.");
        private GUIContent label_GenerateButton
            = new GUIContent("Generate SnapOffset Prefab");

        private void OnEnable()
        {
            prop_SelectedIndex = serializedObject.FindProperty(PropPath_SelectedIndex);
            prop_GripTransform = serializedObject.FindProperty(PropPath_GripTransform);
            prop_CreateInstance = serializedObject.FindProperty(PropPath_CreateInstance);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            target = base.target as SnapOffsetGenerator;
            
            EditorApplication.projectChanged -= EditorApplication_projectChanged;
            EditorApplication.projectChanged += EditorApplication_projectChanged;

            serializedObject.Update();
                                                  
            UpdateDirectoriesIfNeed();
            if (target.PrefabDirectories == null || target.PrefabDirectories.Length == 0
                || this.displayDirectories == null || this.displayDirectories.Length == 0)
            {
                needUpdateDirectories = true;
            }

            prop_SelectedIndex.intValue 
                = EditorGUILayout.Popup(label_SaveDirectory, prop_SelectedIndex.intValue, this.displayDirectories);

            prop_GripTransform.objectReferenceValue
                = EditorGUILayout.ObjectField(label_GripTransform, prop_GripTransform.objectReferenceValue, typeof(Transform), true);

            prop_CreateInstance.boolValue
                = EditorGUILayout.Toggle(label_CreateInstance, prop_CreateInstance.boolValue);
            leaveInstanceRoot = prop_CreateInstance.boolValue;

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

            serializedObject.ApplyModifiedProperties();
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
            GameObject prefab = null;

            try
            {
                // User can undo
                prefab = PrefabUtility.SaveAsPrefabAssetAndConnect(obj, path, InteractionMode.UserAction);

                var fileName = Path.GetFileName(path);
                Debug.LogFormat(SnapOffsetGenerator.Prefix + "Success. Generated '{0}'. \nPath=\"{1}\"", fileName, path);
            }
            catch (System.Exception ex)
            {
                Debug.LogFormat(SnapOffsetGenerator.Prefix + "<Color=Red>Failed to generate prefab.</Color>\nPath=\"{0}\"\n{1}", path, ex);
            }

            if (prefab != null)
                EditorGUIUtility.PingObject(prefab);

            if (!leaveInstanceRoot && obj != null)
                DestroyImmediate(obj, false);
        }

        private void EditorApplication_projectChanged()
        {
            // To list-up new Directory
            needUpdateDirectories = true;
        }
    }
}