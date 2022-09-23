using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[System.Serializable]
public struct HandPoseData {
    public SteamVR_Input_Sources source;
    
    public float thumbCurl;
    public float indexCurl;
    public float middleCurl;
    public float ringCurl;
    public float pinkyCurl;

    public Vector3 offset; // relative to head
    public Vector3 head; // position in global space

    public static float CurlDistance(HandPoseData handData1, HandPoseData handData2) {
        float distance = 0.0f;

        distance += handData2.thumbCurl - handData1.thumbCurl;
        distance += handData2.indexCurl - handData1.indexCurl;
        distance += handData2.middleCurl - handData1.middleCurl;
        distance += handData2.ringCurl - handData1.ringCurl;
        distance += handData2.pinkyCurl - handData1.pinkyCurl;

        return distance;
    }

    public static float PositionalDistance(HandPoseData current, HandPoseData recording) {
        return Vector3.Distance(current.offset, recording.offset);
    }
}