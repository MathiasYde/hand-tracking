using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackRecording : ScriptableObject {
	public List<HandPoseData> handPoses;
	public float accuracy = 0.4f; // meters, radius
}
