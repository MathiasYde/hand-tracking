using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandUpdater : MonoBehaviour {
    [SerializeField] private Hand leftHand;
    [SerializeField] private Hand rightHand;
    [SerializeField] private Transform head;

    private HandPoseData currentLeftHand;
    private HandPoseData currentRightHand;

    private void Start() {
        if (!leftHand) { Debug.LogError("Left hand not initialized", this); }
        if (!rightHand) { Debug.LogError("Right hand not initialized", this); }
    }

    private HandPoseData PopulateHandPoseData(Hand hand, HandPoseData handData) {
        handData.thumbCurl = hand.skeleton.thumbCurl;
        handData.indexCurl = hand.skeleton.indexCurl;
        handData.middleCurl = hand.skeleton.middleCurl;
        handData.ringCurl = hand.skeleton.ringCurl;
        handData.pinkyCurl = hand.skeleton.pinkyCurl;

        handData.offset.x = (head.position.x - hand.skeleton.GetBone(1).position.x) * -1;
        handData.offset.y = (head.position.y - hand.skeleton.GetBone(1).position.y) * -1;
        handData.offset.z = (head.position.z - hand.skeleton.GetBone(1).position.z) * -1;
 

        //handData.offset.z = hand.transform.position.x - head.position.x;
        //handData.offset.y = hand.transform.position.y - head.position.y;
        //handData.offset.z = hand.transform.position.z - head.position.z;
        return handData;
    }

    private void Update() {
        if (rightHand.skeleton) {
            HandPoseData right = new HandPoseData();
            right = PopulateHandPoseData(rightHand, right);
            HandTracking.UpdateHand(SteamVR_Input_Sources.RightHand, right);
            currentRightHand = right;
        }

        if (leftHand.skeleton) {
            HandPoseData left = new HandPoseData();
            left = PopulateHandPoseData(leftHand, left);
            HandTracking.UpdateHand(SteamVR_Input_Sources.LeftHand, left);
            currentLeftHand = left;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(head.position, currentLeftHand.offset);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(head.position, currentRightHand.offset);
    }
}
