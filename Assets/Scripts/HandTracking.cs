using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public static class HandTracking {
    private static Dictionary<SteamVR_Input_Sources, HandPoseData> handData = new Dictionary<SteamVR_Input_Sources, HandPoseData>();

    public static IEnumerator Record() {
        throw new 
    }

    public static void UpdateHand(SteamVR_Input_Sources hand, HandPoseData pose) {
        handData[hand] = pose;
    }

    public static HandPoseData getHandData(SteamVR_Input_Sources hand) {
        return handData[hand];
    }
}
