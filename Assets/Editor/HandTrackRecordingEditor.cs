using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(HandTrackRecording))]
public class HandTrackRecordingEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        HandTrackRecording script = (HandTrackRecording)target;
        List<HandTrackRecording> recordings = HandTracking.GetRecordings();

        if (recordings.Contains(script)) {
            if (GUILayout.Button("Remove recording from recognition set", GUILayout.Height(40))) {
                HandTracking.RemoveRecordingFromRecognitionSet(script);
            }
        } else {
            if (GUILayout.Button("Add recording from recognition set", GUILayout.Height(40))) {
                HandTracking.AddRecordingFromRecognitionSet(script);
            }
        }

        if (GUILayout.Button("Reset recording", GUILayout.Height(40))) {
            script.Reset();
        }

        if (GUILayout.Button("Capture hand data", GUILayout.Height(40))) {
            HandTracking.CaptureHandData(script);
        }

        if (GUILayout.Button("Execute recognition events", GUILayout.Height(40))) {
            script.onRecognize?.Invoke(script);
        }

    }
}
