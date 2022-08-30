using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandTrackingDebugInfo : MonoBehaviour {

    [SerializeField] private Transform head;
    void OnDrawGizmos() {
        HandTrackRecording recording = HandTracking.GetCurrentRecording();
        if (recording != null) {
            foreach (KeyValuePair<SteamVR_Input_Sources, List<HandPoseData>> pair in recording.handData)
            {
                SteamVR_Input_Sources source = pair.Key;
                List<HandPoseData> handposes = pair.Value;
                foreach (HandPoseData handpose in handposes)
                {
                    Vector3 center = handpose.offset;
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(center, 0.1f);
                }

            }
        }
    }
}
