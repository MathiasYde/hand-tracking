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
    private static List<HandTrackRecording> recordings = new List<HandTrackRecording>();

    public static List<HandTrackRecording> GetRecordings() => recordings;

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
        recordings.Add(recording);
    }
    public static void ResetHandGestureRecognitionProgress() {
        foreach (HandTrackRecording recording in recordings) {
            recording.recognitionProgress = 0;
        }
    }

    public static void RecognizeHandGestures(GenericDictionary<SteamVR_Input_Sources, HandPoseData> handData) {
        foreach (HandTrackRecording recording in recordings) {

            bool disqualified = false;
            foreach (KeyValuePair<SteamVR_Input_Sources, HandPoseData> pair2 in handData) {
                SteamVR_Input_Sources source = pair2.Key;
                HandPoseData data = pair2.Value;

                float positionalDistance = HandPoseData.PositionalDistance(handData[source], data);
                float curlDistance = HandPoseData.CurlDistance(handData[source], data);

                if (
                    recording.positionalMaxDistance.Enabled && 
                    positionalDistance > recording.positionalMaxDistance) {
                    Debug.Log("Disqualified from positional distance");
                    disqualified = true;
                }

                if (
                    recording.curlMaxDistance.Enabled &&
                    curlDistance > recording.curlMaxDistance) {
                    Debug.Log("Disqualified from curl distance");
                    disqualified = true;
                }

            }

            // skip if this pose is disqualified
            if (disqualified) { continue; }

            recording.recognitionProgress += 1;

            // test if recording is executed
            if (recording.recognitionProgress >= recording.count) {
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
        if (recording != null) {
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
