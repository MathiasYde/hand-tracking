using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandTrackingDebugInfo : MonoBehaviour {

    //public GenericDictionary<SteamVR_Input_Sources, Color> sourceColorMap = new GenericDictionary<SteamVR_Input_Sources, Color>();

    [SerializeField] private Transform head;
    void OnDrawGizmos() {
        List<HandTrackRecording> recordings = HandTracking.GetRecordings();
        if (recordings.Count == 0)
        {
            Debug.Log("No recordings to debug");
        }

        foreach (HandTrackRecording recording in recordings) {
            foreach (KeyValuePair<SteamVR_Input_Sources, List<HandPoseData>> pair2 in recording.handData) {
                SteamVR_Input_Sources source = pair2.Key;
                List<HandPoseData> handPoseDatas = pair2.Value;

                for (int i = 0; i < handPoseDatas.Count - 1; i++)
                {
                    HandPoseData current = handPoseDatas[i];
                    HandPoseData next = handPoseDatas[i+1];

                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(
                        current.offset + head.position,
                        next.offset + head.position
                    );
                }

                for (int i = 0; i < handPoseDatas.Count; i++) {
                    HandPoseData handPoseData = handPoseDatas[i];

                    if (i > recording.recognitionProgress)  Gizmos.color = Color.red;
                    if (i == recording.recognitionProgress) Gizmos.color = Color.yellow;
                    if (i < recording.recognitionProgress)  Gizmos.color = Color.green;

                    Vector3 position = handPoseData.offset + head.position;
                    Gizmos.DrawWireSphere(position, recording.positionalMaxDistance);
                }
            }
        }
        //HandTrackRecording recording = HandTracking.GetCurrentRecording();
        //if (recording != null) {
        //    foreach (KeyValuePair<SteamVR_Input_Sources, List<HandPoseData>> pair in recording.handData)
        //    {
        //        SteamVR_Input_Sources source = pair.Key;
        //        List<HandPoseData> handposes = pair.Value;
        //        foreach (HandPoseData handpose in handposes)
        //        {
        //            Vector3 center = handpose.offset + head.position;
        //            Gizmos.color = Color.blue;
        //            Gizmos.DrawWireSphere(center, recording.positionalMaxDistance);
        //        }

        //    }
        //}
    }
}
