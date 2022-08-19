using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecordingMethod : ScriptableObject {
    public abstract void StartRecording();
    public abstract void StopRecording();
    public abstract void UpdateRecording();
}
