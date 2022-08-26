using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public static class HandTracking {
    private static GenericDictionary<SteamVR_Input_Sources, HandPoseData> handData = new GenericDictionary<SteamVR_Input_Sources, HandPoseData>();

    public static RecordingStoppingCriteria stopCriteria;
    public static RecordingMethod recordingMethod;
    private static HandTrackRecording recording;

    public static IEnumerator Record(HandTrackRecording recording)  {
        if (HandTracking.recording != null) {
            Debug.LogError("Tried to start recording while already recording");
            yield return null;
        }

        HandTracking.recording = recording;
        
        stopCriteria.StartRecording();
        recordingMethod.StartRecording();
        while (!stopCriteria.ShouldStop()) {
            stopCriteria.UpdateRecording(handData);
            recordingMethod.UpdateRecording();
            yield return null;
        }
        stopCriteria.StopRecording();
        recordingMethod.StopRecording();

        HandTracking.recording = null;
    }

    public static void CaptureHandData() {
        Debug.Assert(recording != null, "Tried to capture hand data, but it's not recording");

        foreach (KeyValuePair<SteamVR_Input_Sources, HandPoseData> pair in handData) {
            SteamVR_Input_Sources source = pair.Key;
            HandPoseData handData = pair.Value;

            // create new entry for dictionary if null
            if (!recording.handData.ContainsKey(source)) {
                recording.handData[source] = new List<HandPoseData>();
            }

            recording.handData[source].Add(handData);
        }
    }

    public static void Update() {
        stopCriteria.UpdateRecording(handData);
    }

    public static void UpdateHand(SteamVR_Input_Sources hand, HandPoseData pose) {
        handData[hand] = pose;
    }

    public static HandPoseData GetHandData(SteamVR_Input_Sources hand) {
        return handData[hand];
    }
}
