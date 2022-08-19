using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[CreateAssetMenu(fileName = "Stop Criterias/StillMotionRecordingStoppingCriteria")]
public class StillMotionRecordingStoppingCriteria : RecordingStoppingCriteria {
    [SerializeField] private float maxFrames = 8; // frames
    [SerializeField] private float maxNoise; 

    [SerializeField] private int handDataBufferSize = 16;
    [SerializeField] private List<float> noiseBuffer;
    [SerializeField] private Dictionary<SteamVR_Input_Sources, HandPoseData> previousHandData;
    private float CalculateHandPoseDistance(HandPoseData hand1, HandPoseData hand2) {
        float noise = 0f;
        noise += hand1.thumbCurl - hand2.thumbCurl;
        noise += hand1.indexCurl - hand2.indexCurl;
        noise += hand1.middleCurl - hand2.middleCurl;
        noise += hand1.ringCurl - hand2.ringCurl;
        noise += hand1.pinkyCurl - hand2.pinkyCurl;
        noise += hand1.offset.x - hand2.offset.x;
        noise += hand1.offset.y - hand2.offset.y;
        noise += hand1.offset.z - hand2.offset.z;
        return noise;
    }

    public override bool ShouldStop() {
        float totalNoise = 0.0f;
        for (int i = 1; i <= maxFrames; i++) {
            totalNoise += noiseBuffer[noiseBuffer.Count - i];
        }

        return totalNoise < maxNoise;
    }

    public override void UpdateHands(Dictionary<SteamVR_Input_Sources, HandPoseData> handPoses) {
        float totalNoise = 0f;
        foreach (KeyValuePair<SteamVR_Input_Sources, HandPoseData> pair in handPoses) {
            SteamVR_Input_Sources source = pair.Key;
            HandPoseData currentData = pair.Value;
            HandPoseData previousData = previousHandData[source];

            float noise = CalculateHandPoseDistance(currentData, previousData);
            totalNoise += noise;
        }

        noiseBuffer.Add(totalNoise);
        // remove old data when count is bigger than buffer size
        if (noiseBuffer.Count > 16) {
            noiseBuffer.RemoveAt(0);
        }

        // save current data as previous for future frame
        foreach (KeyValuePair<SteamVR_Input_Sources, HandPoseData> pair in handPoses) {
            SteamVR_Input_Sources source = pair.Key;
            HandPoseData handPoseData = pair.Value;

            previousHandData[source] = handPoseData;
        }
    }

    public override void Start() {
        noiseBuffer = new List<float>();
        previousHandData = new Dictionary<SteamVR_Input_Sources, HandPoseData>();
    }
}
