using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public static class HandTracking {
    private static GenericDictionary<SteamVR_Input_Sources, HandPoseData> currentHandPoses = new GenericDictionary<SteamVR_Input_Sources, HandPoseData>();
    public static GenericDictionary<SteamVR_Input_Sources, HandPoseData> GetCurrentHandPoses() => currentHandPoses;
    
    public static RecordingStoppingCriteria stopCriteria;
    public static RecordingMethod recordingMethod;
    
    private static HandTrackRecording currentRecording;
    public static HandTrackRecording GetCurrentRecording() => currentRecording;
    
    private static List<HandTrackRecording> recordings = new List<HandTrackRecording>();
    public static List<HandTrackRecording> GetRecordings() => recordings;

    private static Timer recognitionCooldown = new Timer(1.0f);
    private static bool recognitionDisabled = false;
    
    public static void RemoveRecordingFromRecognitionSet(HandTrackRecording recording) {
        VRDebugConsole.Log("Removed recording to recognition set");
        recordings.Remove(recording);
    }

    public static void AddRecordingFromRecognitionSet(HandTrackRecording recording) {
        VRDebugConsole.Log("Added recording to recognition set");
        recordings.Add(recording);
    }

    public static IEnumerator Record(HandTrackRecording recording, bool extendRecording = false)  {
        if (HandTracking.currentRecording != null) {
            Debug.LogError("Tried to start recording while already recording");
            yield return null;
        }

        if (!extendRecording) recording.Reset();
        HandTracking.currentRecording = recording;
        
        stopCriteria.StartRecording();
        recordingMethod.StartRecording();
        
        while (!stopCriteria.ShouldStop()) {
            yield return null;
        }

        stopCriteria.StopRecording();
        recordingMethod.StopRecording();

        AddRecordingFromRecognitionSet(HandTracking.currentRecording);
        HandTracking.currentRecording = null;
    }
    public static void ResetHandGestureRecognitionProgress() {
        foreach (HandTrackRecording recording in recordings) {
            recording.recognitionProgress = 0;
        }
    }

    public static void RecognizeHandGestures() {
        foreach (HandTrackRecording recording in recordings) {
            bool disqualified = false;
            
            // test if disqualified
            foreach ((SteamVR_Input_Sources source, List<HandPoseData> recordingHandPoses) in recording.handData) {
                // prevent index error
                if (recording.recognitionProgress < recordingHandPoses.Count) { continue; }
                
                HandPoseData currentHandPose = currentHandPoses[source];
                HandPoseData recordingHandPose = recordingHandPoses[recording.recognitionProgress];
                
                // disqualify by positional distance
                float positionalDistance = HandPoseData.PositionalDistance(currentHandPose, recordingHandPose);
                if (recording.positionalMaxDistance.Enabled &&
                    (positionalDistance > recording.positionalMaxDistance)) {
                    disqualified = true;
                    goto break_loop;
                }
                
                // disqualify by curl distance
                float curlDistance = HandPoseData.CurlDistance(currentHandPose, recordingHandPose);
                if (recording.curlMaxDistance.Enabled &&
                    (curlDistance > recording.curlMaxDistance)) {
                    disqualified = true;
                    goto break_loop;
                }
            }

            break_loop:
            if (disqualified) { continue; }

            recording.recognitionProgress += 1;
            if (recording.recognitionProgress >= recording.count) {
                recording.onRecognize?.Invoke(recording);
                HandTracking.recognitionDisabled = true;
                HandTracking.recognitionCooldown.Reset();
                
                ResetHandGestureRecognitionProgress();
            }
        }
    }

    public static void CaptureHandData() {
        CaptureHandData(HandTracking.currentRecording);
    }

    public static void CaptureHandData(HandTrackRecording recording) {
        if (recording == null) {
            Debug.LogWarning("Tried to capture hand data, but no recording is present");
            return;
        }

        foreach (KeyValuePair<SteamVR_Input_Sources, HandPoseData> pair in currentHandPoses) {
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
        if (!recognitionDisabled) {
            RecognizeHandGestures();
        }
        

        if (currentRecording) {
            stopCriteria?.UpdateRecording(currentHandPoses);
            recordingMethod?.UpdateRecording();
        }
    }

    public static void Start() {
        // enable recognition when timer ends
        recognitionCooldown.onTimerEnd += () => { recognitionDisabled = false; };
    }

    public static void UpdateHand(SteamVR_Input_Sources source, HandPoseData pose) {
        currentHandPoses[source] = pose;
    }
}
