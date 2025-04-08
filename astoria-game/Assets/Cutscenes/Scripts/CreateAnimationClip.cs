using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class CreateAnimationClip : MonoBehaviour
{

    [SerializeField] private Transform trackedTransform; // The transform to track
    [SerializeField] private string animationClipName = "RecordedClip"; // Name of the animation clip

    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();

    private void FixedUpdate()
    {
        // Record the transform's position and rotation
        if (trackedTransform != null)
        {
            positions.Add(trackedTransform.position);
            rotations.Add(trackedTransform.rotation);
        }
    }

    private void OnDestroy()
    {
        AnimationClip newClip = new AnimationClip();
        newClip.name = animationClipName;

        // Create an Animation Curve for position and rotation
        AnimationCurve positionXCurve = new AnimationCurve();
        AnimationCurve positionYCurve = new AnimationCurve();
        AnimationCurve positionZCurve = new AnimationCurve();
        AnimationCurve rotationXCurve = new AnimationCurve();
        AnimationCurve rotationYCurve = new AnimationCurve();
        AnimationCurve rotationZCurve = new AnimationCurve();
        AnimationCurve rotationWCurve = new AnimationCurve();

        for (int i = 0; i < positions.Count; i++)
        {
            float time = i * Time.fixedDeltaTime;
            positionXCurve.AddKey(time, positions[i].x);
            positionYCurve.AddKey(time, positions[i].y);
            positionZCurve.AddKey(time, positions[i].z);
            rotationXCurve.AddKey(time, rotations[i].x);
            rotationYCurve.AddKey(time, rotations[i].y);
            rotationZCurve.AddKey(time, rotations[i].z);
            rotationWCurve.AddKey(time, rotations[i].w);
        }

        // Set the curves to the Animation Clip
        newClip.SetCurve("", typeof(Transform), "localPosition.x", positionXCurve);
        newClip.SetCurve("", typeof(Transform), "localPosition.y", positionYCurve);
        newClip.SetCurve("", typeof(Transform), "localPosition.z", positionZCurve);
        newClip.SetCurve("", typeof(Transform), "localRotation.x", rotationXCurve);
        newClip.SetCurve("", typeof(Transform), "localRotation.y", rotationYCurve);
        newClip.SetCurve("", typeof(Transform), "localRotation.z", rotationZCurve);
        newClip.SetCurve("", typeof(Transform), "localRotation.w", rotationWCurve);

        // Save the clip as an asset in the project
        string path = "Assets/Cutscenes/" + newClip.name + ".anim";
        UnityEditor.AssetDatabase.CreateAsset(newClip, path);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log("Animation clip saved at: " + path);
    }
}