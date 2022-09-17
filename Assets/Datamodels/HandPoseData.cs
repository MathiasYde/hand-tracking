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

    public static float PositionalDistance(HandPoseData handData1, HandPoseData handData2) {
        Vector3 point1 = handData1.offset + handData1.head;
        Vector3 point2 = handData2.offset + handData2.head;

        return Vector3.Distance(point1, point2);;
    }
}