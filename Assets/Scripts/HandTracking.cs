using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public static class HandTracking {
    private static GenericDictionary<SteamVR_Input_Sources, HandPoseData> handData = new GenericDictionary<SteamVR_Input_Sources, HandPoseData>();

    public static RecordingStoppingCriteria stopCriteria;
    public static RecordingMethod recordingMethod;
    private static HandTrackRecording recording;
    [SerializeField] private static List<Tuple<HandTrackRecording, int>> recognizeRecordings;

    public static IEnumerator Record(HandTrackRecording recording)  {
        if (HandTracking.recording != null) {
            Debug.LogError("Tried to start recording while already recording");
            yield return null;
        }

        HandTracking.recording = recording;
        
        stopCriteria.StartRecording();
        recordingMethod.StartRecording();
        while (!stopCriteria.ShouldStop()) {
            yield return null;
        }
        stopCriteria.StopRecording();
        recordingMethod.StopRecording();

        HandTracking.recording = null;
    }
    public static void AddRecordingToRecognize(HandTrackRecording recording) {
        recognizeRecordings.Add(new Tuple<HandTrackRecording, int>(recording, 0));
    }
    public static void ResetHandGestureRecognitionProgress() {
        foreach (Tuple<HandTrackRecording, int> pair1 in recognizeRecordings) {
            HandTrackRecording recording = pair1.Item1;
            int progress = pair1.Item2;
            progress = 0;
        }
    }

    public static void RecognizeHandGestures(GenericDictionary<SteamVR_Input_Sources, HandPoseData> handData) {
        foreach (Tuple<HandTrackRecording, int> pair1 in recognizeRecordings) {
            HandTrackRecording recording = pair1.Item1;
            int progress = pair1.Item2;


            float totalDistance = 0.0f;
            foreach (KeyValuePair<SteamVR_Input_Sources, HandPoseData> pair2 in handData) {
                SteamVR_Input_Sources source = pair2.Key;
                HandPoseData data = pair2.Value;

                float distance = HandPoseData.Distance(recording.handData[source][progress], data);
                totalDistance += distance;
            }

            progress += (totalDistance <= recording.maxDistance) ? 1 : 0;
            if (progress >= recording.count) {
                recording.onRecognize?.Invoke();
                // reset all other progress
                foreach
            }
        }
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
            recording.count += 1;
        }
    }

    public static void Update() {
        stopCriteria?.UpdateRecording(handData);
        recordingMethod?.UpdateRecording();
    }

    public static void UpdateHand(SteamVR_Input_Sources hand, HandPoseData pose) {
        handData[hand] = pose;
    }

    public static HandPoseData GetHandData(SteamVR_Input_Sources hand) {
        return handData[hand];
    }
}
