using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Udonba.SnapHelper
{
    public class SnapOffsetGenerator : MonoBehaviour
    {
        private OVRGrabber grabber = null;
        protected OVRGrabber Grabber
        {
            get
            {
                if (grabber == null)
                    grabber = this.GetComponent<OVRGrabber>();
                return grabber;
            }
        }

        [SerializeField]
        private Transform snapTransform = null;

        //[SerializeField, Tooltip("Show right-hand where place to grab.")]
        //private bool virtualHandVisible = true;

    }
}