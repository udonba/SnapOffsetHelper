using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Udonba.SnapHelper
{
    [CustomEditor(typeof(SnapOffsetGenerator))]
    public class SnapOffsetGeneratorEditor : Editor
    {
        private SnapOffsetGenerator targetComponent = null;



        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            targetComponent = target as SnapOffsetGenerator;

            

            if (GUILayout.Button("Generate snapOffset prefab"))
            {
                Debug.LogFormat("SnapOffset prefab is generated.");
            }


        }
    }
}