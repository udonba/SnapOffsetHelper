using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace Udonba.SnapHelper
{
    [CustomEditor(typeof(SnapOffsetGenerator))]
    public class SnapOffsetGeneratorEditor : Editor
    {
        private SnapOffsetGenerator target = null;


        private string[] displayDirectories = null;
        private int selectedIndex = 0;
        private bool needUpdateDirectories = true;

        private GUIContent label_SaveDirectory = new GUIContent("Save Prefab to");

        [InitializeOnLoadMethod]
        private void OnLoad()
        {
            Debug.Log("OnLoad");
            EditorApplication.projectChanged -= EditorApplication_projectChanged;
            EditorApplication.projectChanged += EditorApplication_projectChanged;
        }

        private void EditorApplication_projectChanged()
        {
            Debug.Log("EditorApplication.projectChanged");

            needUpdateDirectories = true;
        }



        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorApplication.projectChanged -= EditorApplication_projectChanged;
            EditorApplication.projectChanged += EditorApplication_projectChanged;

            target = base.target as SnapOffsetGenerator;

            // 'Prefabs'フォルダリストの初期化(無ければ作る)
            if (needUpdateDirectories)
                UpdateDirectories();

            if (target.PrefabDirectories == null || target.PrefabDirectories.Length == 0
                || this.displayDirectories == null || this.displayDirectories.Length == 0)
                needUpdateDirectories = true;

            this.selectedIndex 
                = EditorGUILayout.Popup(label_SaveDirectory, selectedIndex, this.displayDirectories);
            //Debug.Log("this.selectedIndex = " + this.selectedIndex);
            target.SelectedIndex = this.selectedIndex;
            //Debug.Log("target.selectedIndex = " + target.SelectedIndex);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Generate SnapOffset Prefab", GUILayout.MaxWidth(400)))
            {
                Debug.LogFormat("Index : {0}", target.SelectedIndex);
                Debug.LogFormat("Path : {0}", target.PrefabDirectories[target.SelectedIndex]);
                if (target.GripTransform == null)
                {
                    Debug.LogWarning("Need attach 'Grip Transform' field to generate SnapOffset.");
                }
                else
                {
                    GeneratePrefab();
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

        }

        private void UpdateDirectories()
        {
            if (!needUpdateDirectories)
                return;

            target.UpdatePrefabDirectories();
            this.displayDirectories = target.GetDisplayDirectories();

            needUpdateDirectories = false;
        }

        private void GeneratePrefab()
        {
            var param = target.GetSnapOffsetParams();
            var obj = target.CreateSnapOffsetObject(param);
            var path = target.GetSavePath(param.Name);

            Debug.LogFormat("Generating prefab... ({0}, {1})", obj.name, path);
            try
            {
                // User can undo
                PrefabUtility.SaveAsPrefabAssetAndConnect(obj, path, InteractionMode.UserAction);

                Debug.LogFormat("[SnapOffsetGenerator] Generated '{0}'. \nPath=\"{1}\"", param.Name, path);
            }
            catch (System.Exception ex)
            {
                Debug.LogFormat("[SnapOffsetGenerator] Failed to generate prefab. \nPath=\"{1}\"\n{2}", 
                    param.Name, path, ex);
            }
        }
    }
}