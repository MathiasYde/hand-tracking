using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public static class HandTracking {
    private static Dictionary<SteamVR_Input_Sources, HandPoseData> handData = new Dictionary<SteamVR_Input_Sources, HandPoseData>();

    private static bool isRecording = false;
    private static RecordingStoppingCriteria stopCriteria;

    public static IEnumerator Record(RecordingStoppingCriteria stopCriteria)  {
        if (isRecording) {
            Debug.LogError("Tried to start recording while already recording");
            yield return null;
        }
        HandTracking.stopCriteria = stopCriteria;
        
        stopCriteria.Start();
        while (!stopCriteria.ShouldStop()) {
            yield return new WaitForSeconds(1);    
        }
    }

    public static void Update() {
        stopCriteria.UpdateHands(handData);
    }

    public static void UpdateHand(SteamVR_Input_Sources hand, HandPoseData pose) {
        handData[hand] = pose;
    }

    public static HandPoseData GetHandData(SteamVR_Input_Sources hand) {
        return handData[hand];
    }
}
