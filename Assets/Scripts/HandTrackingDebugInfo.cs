using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HandTrackingDebugInfo : MonoBehaviour {
    void OnDrawGizmos() {
        HandPoseData right = HandTracking.GetHandData(Valve.VR.SteamVR_Input_Sources.RightHand);
        VRDebugConsole.Log($"r.thumb {right.thumbCurl}");
        VRDebugConsole.Log($"r.index {right.indexCurl}");
        //VRDebugConsole.Log($"r.middle {right.middleCurl}");
        //VRDebugConsole.Log($"r.ring {right.ringCurl}");
        //VRDebugConsole.Log($"r.pinky {right.pinkyCurl}");
        HandPoseData left = HandTracking.GetHandData(Valve.VR.SteamVR_Input_Sources.LeftHand);
        //VRDebugConsole.Log($"l.thumb {right.thumbCurl}");
        //VRDebugConsole.Log($"l.index {right.indexCurl}");
        //VRDebugConsole.Log($"l.middle {right.middleCurl}");
        //VRDebugConsole.Log($"l.ring {right.ringCurl}");
        //VRDebugConsole.Log($"l.pinky {right.pinkyCurl}");
    }
}
