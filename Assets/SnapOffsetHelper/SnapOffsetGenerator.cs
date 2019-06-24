using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Udonba.SnapOffsetHelper
{
    /// <summary>
    /// Generate prefab file for OVRGrabber.SnapOffset field.
    /// </summary>
    [RequireComponent(typeof(OVRGrabbable))]
    public class SnapOffsetGenerator : MonoBehaviour
    {
#if UNITY_EDITOR
        private const char UnitySeparatorChar = '/';
        private const string PrefabExtension = ".prefab";
        private const string PrefabDirectoryName = "Prefabs";
        public const string Prefix = "[SnapOffsetHelper] ";

        private OVRGrabbable grabbable = null;
        protected OVRGrabbable Grabbable
        {
            get
            {
                if (grabbable == null)
                    grabbable = this.GetComponent<OVRGrabbable>();
                return grabbable;
            }
        }

        private Transform gripTransform = null;
        public Transform GripTransform
        {
            get { return gripTransform; }
            set { gripTransform = value; }
        }

        private int selectedIndex = 0;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }

        private string[] prefabDirectories = null;
        public string[] PrefabDirectories
        {
            get { return prefabDirectories; }
        }


        /// <summary>
        /// Get snapOffset values
        /// </summary>
        /// <returns>Offset position and rotation</returns>
        public SnapOffsetParam GetSnapOffsetParams()
        {
            var ret = new SnapOffsetParam
            {
                Name = string.Format("SnapOffset({0})", Grabbable.gameObject.name),
                Position = GripTransform.InverseTransformPoint(Grabbable.transform.position),
                Rotation = Quaternion.Inverse(GripTransform.rotation) * Grabbable.transform.rotation
            };

            return ret;
        }

        /// <summary>
        /// Update 'Prefabs' directories list.
        /// </summary>
        public void UpdatePrefabDirectories()
        {
            this.prefabDirectories = GetPrefabDirectories();
        }

        /// <summary>
        /// Get 'Prefabs' directories list. (for display pop-up menu)
        /// </summary>
        /// <returns></returns>
        public string[] GetDisplayDirectories()
        {
            string[] directories = new string[prefabDirectories.Length];
            Array.Copy(prefabDirectories, directories, prefabDirectories.Length);

            var info = Directory.GetParent(Application.dataPath.ReplaceSeparator(ReplaceOrder.UnityToSystem));
            for (int i = 0; i < directories.Length; i++)
            {
                directories[i] = directories[i].Replace(info.FullName, string.Empty)
                                               .ReplaceSeparator(ReplaceOrder.SystemToUnity);
                if (directories[i].StartsWith(UnitySeparatorChar.ToString()))
                    directories[i] = directories[i].Substring(1, directories[i].Length - 1);
            }

            return directories;
        }

        /// <summary>
        /// Get or create 'Prefabs' directories list.
        /// </summary>
        /// <returns></returns>
        private static string[] GetPrefabDirectories()
        {
            string ret = Application.dataPath + UnitySeparatorChar + PrefabDirectoryName;

            // Find 'Prefabs' directory in assets
            var assetsPath = Application.dataPath.ReplaceSeparator(ReplaceOrder.UnityToSystem);
            var searchPattern = "*" + PrefabDirectoryName;
            var directories = Directory.GetDirectories(assetsPath, searchPattern, SearchOption.AllDirectories);
            if (0 < directories.Length)
            {
                // Get highest hierarchy 'Prefabs' directory
                Array.Sort<string>(directories, CompareByHierarchy);
            }
            else
            {
                var defaultPath = Application.dataPath + UnitySeparatorChar + PrefabDirectoryName;

                // Create 'Prefabs' directory
                var path = defaultPath.ReplaceSeparator(ReplaceOrder.UnityToSystem);
                var info = Directory.CreateDirectory(path);

                directories = new string[] { defaultPath };
            }
            return directories;
        }

        /// <summary>
        /// Create and get SnapOffset instance root GameObject.
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public GameObject CreateSnapOffsetObject(SnapOffsetParam param)
        {
            var obj = new GameObject(param.Name);
            obj.transform.position = param.Position;
            obj.transform.rotation = param.Rotation;

            return obj;
        }

        /// <summary>
        /// Get save prefab path.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetSavePath(string fileName)
        {
            string ret = prefabDirectories[selectedIndex] + Path.DirectorySeparatorChar + fileName;
            if (!ret.EndsWith(PrefabExtension))
                ret += PrefabExtension;

            return ret.ReplaceSeparator(ReplaceOrder.SystemToUnity);
        }
        
        /// <summary>
        /// Sort by hierarchy ascending order.
        /// </summary>
        private static int CompareByHierarchy(string path1, string path2)
        {
            int count1 = path1.Replace(Path.DirectorySeparatorChar, '\0').Length;
            int count2 = path2.Replace(Path.DirectorySeparatorChar, '\0').Length;

            if (path1 == null && path2 == null)
                return 0;
            if (path1 == null)
                return -1;
            if (path2 == null)
                return 1;

            return count1 - count2;
        }
#endif
    }

    /// <summary>
    /// Name, Position and Rotation
    /// </summary>
    public class SnapOffsetParam
    {
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private Vector3 position = new Vector3(0, 0, 0);
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }
        private Quaternion rotation = Quaternion.identity;
        public Quaternion Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }
    }

}