using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HandTracking : MonoBehaviour {
    [SerializeField] private Valve.VR.InteractionSystem.Hand leftHand;
    [SerializeField] private Valve.VR.InteractionSystem.Hand rightHand;
    [SerializeField] private JointOrder jointOrder;
    private void Start() {
        leftHand.ShowSkeleton(true);
        rightHand.ShowSkeleton(true);
    }

    private void Update() {
        Debug.Log($"r.thumb:{rightHand.skeleton.thumbCurl}");
        Debug.Log($"r.index:{rightHand.skeleton.indexCurl}");
        Debug.Log($"r.middle:{rightHand.skeleton.middleCurl}");
        Debug.Log($"r.ring:{rightHand.skeleton.ringCurl}");
        Debug.Log($"r.pinky:{rightHand.skeleton.pinkyCurl}");
    }

}
