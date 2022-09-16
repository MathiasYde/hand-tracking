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

    private static Timer recognitionCooldown = new Timer(1.0f);
    private static bool recognitionDisabled = false;

    public static HandTrackRecording GetCurrentRecording() => recording;

    public static GenericDictionary<SteamVR_Input_Sources, HandPoseData> GetCurrentHandData() => currentHandData;

    public static void RemoveRecordingFromRecognitionSet(HandTrackRecording recording)
    {
        VRDebugConsole.Log("Removed recording to recognition set");
        recordings.Remove(recording);
    }

    public static void AddRecordingFromRecognitionSet(HandTrackRecording recording)
    {
        VRDebugConsole.Log("Added recording to recognition set");
        recordings.Add(recording);
    }

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

        AddRecordingFromRecognitionSet(HandTracking.recording);
        HandTracking.recording = null;
    }
    public static void ResetHandGestureRecognitionProgress() {
        foreach (HandTrackRecording recording in recordings) {
            recording.recognitionProgress = 0;
        }
    }

    public static void RecognizeHandGestures(GenericDictionary<SteamVR_Input_Sources, HandPoseData> currentHandData) {
        foreach (HandTrackRecording recording in recordings) {
            Debug.Log($"Attempting to recognize recording ${recording.name}");
            bool disqualified = false;

            foreach (KeyValuePair<SteamVR_Input_Sources, List<HandPoseData>> pair in recording.handData)
            {
                SteamVR_Input_Sources source = pair.Key;
                List<HandPoseData> handposes = pair.Value;
                HandPoseData currentHandPose = currentHandData[source];

                foreach (HandPoseData handPose in handposes) {
                    float positionalDistance = HandPoseData.PositionalDistance(handPose, currentHandPose);
                    float curlDistance = HandPoseData.CurlDistance(handPose, currentHandPose);

                    if (
                        recording.positionalMaxDistance.Enabled &&
                        (positionalDistance > recording.positionalMaxDistance)) {
                        disqualified = true;
                        goto break_loop;
                    }

                    if (
                        recording.curlMaxDistance.Enabled &&
                        (curlDistance > recording.curlMaxDistance))
                    {
                        disqualified = true;
                        goto break_loop;
                    }


                }
            }


            break_loop:
            if (disqualified) { continue; }

            recording.recognitionProgress += 1;

            // test if recording is executed
            if (recording.recognitionProgress >= recording.count - 1)
            {
                recording.onRecognize?.Invoke();
                HandTracking.recognitionDisabled = true;
                recognitionCooldown.Reset();

                // reset recordings progress
                ResetHandGestureRecognitionProgress();
            }
        }
    }

    public static void CaptureHandData()
    {
        CaptureHandData(HandTracking.recording);
    }

    public static void CaptureHandData(HandTrackRecording recording) {
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
        recognitionCooldown.Update(Time.deltaTime);

        if (recording != null) {
            stopCriteria?.UpdateRecording(currentHandData);
            recordingMethod?.UpdateRecording();
        }
        if (recognitionDisabled == false) {
            RecognizeHandGestures(currentHandData);
        }
    }

    public static void Start()
    {
        recognitionCooldown.onTimerEnd += () => { recognitionDisabled = false; };
    }

    public static void UpdateHand(SteamVR_Input_Sources hand, HandPoseData pose) {
        currentHandData[hand] = pose;
    }

    public static HandPoseData GetHandData(SteamVR_Input_Sources hand) {
        return currentHandData[hand];
    }
}
