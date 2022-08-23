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

    private void Start() {
        if (!leftHand) { Debug.LogError("Left hand not initialized", this); }
        if (!rightHand) { Debug.LogError("Right hand not initialized", this); }
    }

    private void PopulateHandPoseData(Hand hand, HandPoseData handData) {
        handData.thumbCurl = hand.skeleton.thumbCurl;
        handData.indexCurl = hand.skeleton.indexCurl;
        handData.middleCurl = hand.skeleton.middleCurl;
        handData.ringCurl = hand.skeleton.ringCurl;
        handData.pinkyCurl = hand.skeleton.pinkyCurl;
        handData.offset.x = head.position.x - hand.transform.position.x;
        handData.offset.y = head.position.y - hand.transform.position.y;
        handData.offset.z = head.position.z - hand.transform.position.z;
    }

    private void Update() {
        if (rightHand.skeleton) {
            HandPoseData right = new HandPoseData();
            PopulateHandPoseData(rightHand, right);
            HandTracking.UpdateHand(SteamVR_Input_Sources.RightHand, right);
        }

        if (leftHand.skeleton) {
            HandPoseData left = new HandPoseData();
            PopulateHandPoseData(leftHand, left);
            HandTracking.UpdateHand(SteamVR_Input_Sources.LeftHand, left);
        }
    }
}
