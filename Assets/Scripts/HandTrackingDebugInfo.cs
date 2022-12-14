using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandTrackingDebugInfo : MonoBehaviour {
    [SerializeField] private Transform head;

    void GizmosDrawHandPoseDataSphere(HandPoseData handPose)
    {

    }

    void OnDrawGizmos() {
        GenericDictionary<SteamVR_Input_Sources, HandPoseData> currentHandPoses = HandTracking.GetCurrentHandPoses();

        foreach ((SteamVR_Input_Sources source, HandPoseData handPoseData) in currentHandPoses)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(head.position, handPoseData.offset);
        }

        List<HandTrackRecording> recordings = HandTracking.GetRecordings();

        foreach (HandTrackRecording recording in recordings) {
            if (recording == null) { continue; }

            foreach (KeyValuePair<SteamVR_Input_Sources, List<HandPoseData>> pair2 in recording.handData) {
                SteamVR_Input_Sources source = pair2.Key;
                List<HandPoseData> handPoseDatas = pair2.Value;

                for (int i = 0; i < handPoseDatas.Count - 1; i++) {
                    HandPoseData current = handPoseDatas[i];
                    HandPoseData next = handPoseDatas[i+1];

                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(
                        head.TransformDirection(current.offset) + head.position,
                        head.TransformDirection(next.offset) + head.position
                    );
                }
                
                try {
                    HandPoseData currentRecognizedHandData = handPoseDatas[recording.recognitionProgress];
                    HandPoseData currentSource = currentHandPoses[source];

                    Gizmos.color = Color.magenta;
                    Vector3 position1 = head.TransformDirection(currentRecognizedHandData.offset) + currentSource.head;
                    Vector3 position2 = head.TransformDirection(currentSource.offset) + currentSource.head;
                    Gizmos.DrawLine(position1, position2);
                } catch { }


                for (int i = 0; i < handPoseDatas.Count; i++) {
                    HandPoseData handPoseData = handPoseDatas[i];

                    if (i > recording.recognitionProgress)  Gizmos.color = Color.red;
                    if (i == recording.recognitionProgress) Gizmos.color = Color.yellow;
                    if (i < recording.recognitionProgress)  Gizmos.color = Color.green;

                    //Vector3 position = handPoseData.offset + head.position;
                    Vector3 position = head.TransformDirection(handPoseData.offset) + head.position;
                    Gizmos.DrawWireSphere(position, recording.positionalMaxDistance);
                }
            }
        }
    }
}
