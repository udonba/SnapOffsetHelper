# SnapOffsetHelper
Class and editor-extension to more easily generate snapOffset.

## Usage
1. Add "SnapOffssetGenerator" to OVRGrabbable object.
![image01.png](https://imgur.com/kk9iykF "Add component SnapOffsetGenerator")

2. Select directory to save prefab file. ("Prefabs" only)
![image02.png](https://imgur.com/XTDqdOp "Select directory")

3. Attach GripTransform. GripTransform is location of the hand when OVRGrabber grabs it.
![image03.png](https://imgur.com/tGUZnKx "Attach grip transform")

4. Push "Generate SnapOffset Prefab" button.
When success it, automatically focus to prefab.
![image04.png](https://imgur.com/mFoDH9C "Generate prefab")

5. Attach prefab to OVRGrabbable.snapOffset field.
And check "Snap Position" and "Snap Rotation".
![image05.png](https://imgur.com/undefined "Attach snapOffset")

## Environment
- Unity v2018.4.0f1
- Oculus Utilities v1.38.0, OVRPlugin v1.38.0, SDK v1.38.0.
