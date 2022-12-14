using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class HandUpdater : MonoBehaviour {
    [SerializeField] private Transform head;

    [SerializeField]
    private GenericDictionary<SteamVR_Input_Sources, Color> sourceColorMap =
        new GenericDictionary<SteamVR_Input_Sources, Color>();
    
    public GenericDictionary<SteamVR_Input_Sources, Hand> sources =
        new GenericDictionary<SteamVR_Input_Sources, Hand>();

#if UNITY_EDITOR
    private GenericDictionary<SteamVR_Input_Sources, HandPoseData> handData = new GenericDictionary<SteamVR_Input_Sources, HandPoseData>();
#endif
    
    
    private void Start() {
        // log warning if some hands are not initialized in sources dictionary
        foreach ((SteamVR_Input_Sources source, Hand hand) in sources) {
            if (hand == null) {
                Debug.LogWarning($"{hand} not initialized, from source: {source}", this);
            }
        }
        
        HandTracking.Start();
    }
    
    private void Update() {
        // update HandTracking about each hand
        try {
            foreach ((SteamVR_Input_Sources source, Hand hand) in sources) {
                HandPoseData handPoseData = new HandPoseData();
                handPoseData = PopulateHandPoseData(source, handPoseData, hand);
                HandTracking.UpdateHand(source, handPoseData);
            }
        } catch {}
        
        HandTracking.Update();
    }

    // Populate HandPoseData struct for a hand
    private HandPoseData PopulateHandPoseData_Hand(HandPoseData handPoseData, Hand hand) {
        try {
            handPoseData.thumbCurl = hand.skeleton.thumbCurl;
            handPoseData.indexCurl = hand.skeleton.indexCurl;
            handPoseData.middleCurl = hand.skeleton.middleCurl;
            handPoseData.ringCurl = hand.skeleton.ringCurl;
            handPoseData.pinkyCurl = hand.skeleton.pinkyCurl;
        } catch {}
        
        handPoseData.head = head.position;
        
        Transform wrist = hand.skeleton.GetBone(1);
        
        handPoseData.offset = (head.position - wrist.position) * -1;
        handPoseData.offset = head.InverseTransformDirection(handPoseData.offset);

        return handPoseData;
    }
    
    private HandPoseData PopulateHandPoseData(SteamVR_Input_Sources source, HandPoseData handPoseData, Hand hand) {
        handPoseData.source = source;

        switch (source) {
            case SteamVR_Input_Sources.RightHand:
                handPoseData = PopulateHandPoseData_Hand(handPoseData, hand);
                break;
            case SteamVR_Input_Sources.LeftHand:
                handPoseData = PopulateHandPoseData_Hand(handPoseData, hand);
                break;
            default:
                Debug.LogWarning("Tried to populate hand pose data for non-existing source");
                break;
        }

        return handPoseData;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        // draw lines between head position to each hand
        foreach ((SteamVR_Input_Sources source, HandPoseData hand) in handData) {
            Gizmos.color = sourceColorMap.Get(source, Color.black);
            Gizmos.DrawRay(head.position, hand.offset);
        }
    }
#endif
}
