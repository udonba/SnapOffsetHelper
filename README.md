# SnapOffsetHelper
Class and editor-extension to more easily generate snapOffset.

## Usage
1. Add "SnapOffssetGenerator" to OVRGrabbable object.
![image01](https://github.com/udonba/SnapOffsetHelper/blob/images/usage_001.png)

2. Select directory to save prefab file. ("Prefabs" only)
![image02](https://github.com/udonba/SnapOffsetHelper/blob/images/usage_002.png)

3. Attach GripTransform.   
GripTransform is location of the hand when OVRGrabber grabs it.
![image03](https://github.com/udonba/SnapOffsetHelper/blob/images/usage_003.png)

4. Push "Generate SnapOffset Prefab" button.  
When success it, automatically focus to prefab.
![image04](https://github.com/udonba/SnapOffsetHelper/blob/images/usage_004.png)

5. Attach prefab to OVRGrabbable.snapOffset field.  
And check "Snap Position" and "Snap Rotation".
![image05](https://github.com/udonba/SnapOffsetHelper/blob/images/usage_005.png)

## Environment
- Unity v2018.4.0f1
- Oculus Utilities v1.38.0, OVRPlugin v1.38.0, SDK v1.38.0.
