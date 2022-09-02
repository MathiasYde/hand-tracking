using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HandTrackRecording))]
public class HandTrackRecordingEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        HandTrackRecording script = (HandTrackRecording)target;

        if (GUILayout.Button("Reset recording", GUILayout.Height(40))) {
            script.Reset();
        }

    }
}
