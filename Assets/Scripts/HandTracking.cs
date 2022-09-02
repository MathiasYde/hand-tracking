using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public static class HandTracking {
    private static GenericDictionary<SteamVR_Input_Sources, HandPoseData> currentHandData = new GenericDictionary<SteamVR_Input_Sources, HandPoseData>();

    public static RecordingStoppingCriteria stopCriteria;
    public static RecordingMethod recordingMethod;
    private static HandTrackRecording recording;
    public static HandTrackRecordingSet recordingSet = new HandTrackRecordingSet();

    public static HandTrackRecording GetCurrentRecording() => recording;

    public static IEnumerator Record(HandTrackRecording recording)  {
        if (HandTracking.recording != null) {
            Debug.LogError("Tried to start recording while already recording");
            yield return null;
        }

        recording.Reset();
        HandTracking.recording = recording;
        
        stopCriteria.StartRecording();
        recordingMethod.StartRecording();
        
        while (!stopCriteria.ShouldStop()) {
            yield return null;
        }

        stopCriteria.StopRecording();
        recordingMethod.StopRecording();

        AddRecordingToRecognize(HandTracking.recording);
        HandTracking.recording = null;
    }
    public static void AddRecordingToRecognize(HandTrackRecording recording) {
        recordingSet.recordings.Add(new Tuple<HandTrackRecording, int>(recording, 0));
    }
    public static void ResetHandGestureRecognitionProgress() {
        foreach (Tuple<HandTrackRecording, int> pair in recordingSet.recordings) {
            HandTrackRecording recording = pair.Item1;
            int progress = pair.Item2;
            progress = 0;
        }
    }

    public static void RecognizeHandGestures(GenericDictionary<SteamVR_Input_Sources, HandPoseData> handData) {
        foreach (Tuple<HandTrackRecording, int> pair1 in recordingSet.recordings) {
            HandTrackRecording recording = pair1.Item1;
            int progress = pair1.Item2;

            bool disqualified = false;
            foreach (KeyValuePair<SteamVR_Input_Sources, HandPoseData> pair2 in handData) {
                SteamVR_Input_Sources source = pair2.Key;
                HandPoseData data = pair2.Value;

                if (recording.positionalMaxDistance.Enabled && HandPoseData.PositionalDistance(handData[source], data) > recording.positionalMaxDistance) { disqualified = true; }
                if (recording.curlMaxDistance.Enabled && HandPoseData.CurlDistance(handData[source], data) > recording.curlMaxDistance) { disqualified = true; }

            }

            // skip if this pose is disqualified
            if (disqualified) { continue; }

            progress += 1;

            // test if recording is executed
            if (progress >= recording.count) {
                recording.onRecognize?.Invoke();

                // reset recordings progress
                ResetHandGestureRecognitionProgress();
            }
        }
    }

    public static void CaptureHandData() {
        if (recording == null) {
            Debug.LogWarning("Tried to capture hand data, but no recording is present");
            return;
        }

        foreach (KeyValuePair<SteamVR_Input_Sources, HandPoseData> pair in currentHandData) {
            SteamVR_Input_Sources source = pair.Key;
            HandPoseData data = pair.Value;

            // skip this source if not present in recording
            if (!recording.handData.ContainsKey(source)) {
                continue;
            }

            recording.handData[source].Add(data);
            recording.count += 1;
        }
    }

    public static void Update() {
        if (recording != null) // dont update if not recording
        {
        stopCriteria?.UpdateRecording(currentHandData);
        recordingMethod?.UpdateRecording();
        }
        RecognizeHandGestures(currentHandData);
    }

    public static void UpdateHand(SteamVR_Input_Sources hand, HandPoseData pose) {
        currentHandData[hand] = pose;
    }

    public static HandPoseData GetHandData(SteamVR_Input_Sources hand) {
        return currentHandData[hand];
    }
}
