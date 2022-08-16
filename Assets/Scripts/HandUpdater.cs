using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandUpdater : MonoBehaviour {
    [SerializeField] private Valve.VR.InteractionSystem.Hand leftHand;
    [SerializeField] private Valve.VR.InteractionSystem.Hand rightHand;

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
            HandTracking.UpdateHand(SteamVR_Input_Sources.RightHand, right);
        }

        if (leftHand.skeleton) {
            HandPoseData left = new HandPoseData();
            left.thumbCurl = leftHand.skeleton.thumbCurl;
            left.indexCurl = leftHand.skeleton.indexCurl;
            left.middleCurl = leftHand.skeleton.middleCurl;
            left.ringCurl = leftHand.skeleton.ringCurl;
            left.pinkyCurl = leftHand.skeleton.pinkyCurl;
            HandTracking.UpdateHand(SteamVR_Input_Sources.LeftHand, left);
        }
    }

    private class ReadOnlyFieldAttribute : Attribute
    {
    }
}
