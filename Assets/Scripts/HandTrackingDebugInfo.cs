using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HandTrackingDebugInfo : MonoBehaviour {
    void OnDrawGizmos() {
        HandPoseData right = HandTracking.getHandData(Valve.VR.SteamVR_Input_Sources.RightHand);
        VRDebugConsole.Log($"r.thumb {right.thumbCurl}");
        VRDebugConsole.Log($"r.indx {right.indexCurl}");
        //VRDebugConsole.Log($"r.middel {right.middleCurl}");
        //VRDebugConsole.Log($"r.ring {right.ringCurl}");
        //VRDebugConsole.Log($"r.pinky {right.pinkyCurl}");
        HandPoseData left = HandTracking.getHandData(Valve.VR.SteamVR_Input_Sources.LeftHand);
        //VRDebugConsole.Log($"l.thumb {right.thumbCurl}");
        //VRDebugConsole.Log($"l.indx {right.indexCurl}");
        //VRDebugConsole.Log($"l.middel {right.middleCurl}");
        //VRDebugConsole.Log($"l.ring {right.ringCurl}");
        //VRDebugConsole.Log($"l.pinky {right.pinkyCurl}");
    }
}
