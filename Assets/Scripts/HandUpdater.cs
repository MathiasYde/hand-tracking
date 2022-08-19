using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandUpdater : MonoBehaviour {
    [SerializeField] private Valve.VR.InteractionSystem.Hand leftHand;
    [SerializeField] private Valve.VR.InteractionSystem.Hand rightHand;
    [SerializeField] private Transform head;

    private void Start() {
        if (!leftHand) { Debug.LogError("Left hand not initialized", this); }
        if (!rightHand) { Debug.LogError("Right hand not initialized", this); }
    }

    private void Update() {
        if (rightHand.skeleton) {
            HandPoseData right = new HandPoseData();
            right.thumbCurl = rightHand.skeleton.thumbCurl;
            right.indexCurl = rightHand.skeleton.indexCurl;
            right.middleCurl = rightHand.skeleton.middleCurl;
            right.ringCurl = rightHand.skeleton.ringCurl;
            right.pinkyCurl = rightHand.skeleton.pinkyCurl;
            right.offset.x = head.position.x - rightHand.transform.position.x;
            right.offset.y = head.position.y - rightHand.transform.position.y;
            right.offset.z = head.position.z - rightHand.transform.position.z;
            HandTracking.UpdateHand(SteamVR_Input_Sources.RightHand, right);
        }

        if (leftHand.skeleton) {
            HandPoseData left = new HandPoseData();
            left.thumbCurl = leftHand.skeleton.thumbCurl;
            left.indexCurl = leftHand.skeleton.indexCurl;
            left.middleCurl = leftHand.skeleton.middleCurl;
            left.ringCurl = leftHand.skeleton.ringCurl;
            left.pinkyCurl = leftHand.skeleton.pinkyCurl;
            left.offset.x = head.position.x - leftHand.transform.position.x;
            left.offset.y = head.position.y - leftHand.transform.position.y;
            left.offset.z = head.position.z - leftHand.transform.position.z;
            HandTracking.UpdateHand(SteamVR_Input_Sources.LeftHand, left);
        }
    }
}
