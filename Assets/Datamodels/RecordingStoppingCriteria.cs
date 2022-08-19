using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecordingStoppingCriteria : ScriptableObject {
    public abstract bool ShouldStop();
    public abstract void UpdateRecording(Dictionary<Valve.VR.SteamVR_Input_Sources, HandPoseData> handPoses);
    public abstract void StartRecording();
    public abstract void StopRecording();
}
