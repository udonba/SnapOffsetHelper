using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udonba.SnapHelper
{
    [RequireComponent(typeof(OVRGrabbable))]
    public class SnapOffsetGenerator : MonoBehaviour
    {
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

        [SerializeField, Tooltip("Grabber.gripTransform, or offset transform (inside grabbable object)")]
        private Transform gripTransform = null;
        private Transform prev = null;
        
        [SerializeField, Tooltip("Show right-hand where place to grab.")]
        private bool log = true;

        /// <summary>
        /// Get snapOffset values
        /// </summary>
        /// <returns>Offset position and rotation</returns>
        private SnapOffsetParams GetSnapOffsetParams()
        {
            var ret = new SnapOffsetParams
            {
                Position = gripTransform.InverseTransformPoint(Grabbable.transform.position),
                Rotation = Quaternion.Inverse(gripTransform.rotation) * Grabbable.transform.rotation
            };

            Debug.LogFormat("Position : ({0:F4}, {1:F4}, {2:F4})",
                ret.Position.x, ret.Position.y, ret.Position.z);
            Debug.LogFormat("Rotation : ({0:F4}, {1:F4}, {2:F4})",
                ret.Rotation.eulerAngles.x, ret.Rotation.eulerAngles.y, ret.Rotation.eulerAngles.z);
            
            return ret;
        }

        private void OnDrawGizmos()
        {
            if (!log)
                return;
            if (gripTransform == null)
                return;

            var param = GetSnapOffsetParams();

            prev = gripTransform;
        }

        private string GetPrefaObjectName(OVRGrabbable target)
        {
            return string.Format("SnapOffset({0})", target.gameObject.name);
        }
     
    }

    /// <summary>
    /// Position and Rotation
    /// </summary>
    public class SnapOffsetParams
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public SnapOffsetParams()
        {
            Position = new Vector3(0, 0, 0);
            Rotation = Quaternion.identity;
        }
        public SnapOffsetParams(Vector3 pos, Quaternion rot)
        {
            Position = pos;
            Rotation = rot;
        }
    }

}