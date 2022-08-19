using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[CreateAssetMenu(menuName ="Hand Track Recording")]
public class HandTrackRecording : ScriptableObject {
	public Dictionary<SteamVR_Input_Sources, List<HandPoseData>> handData;
	public float accuracy = 0.4f; // meters, radius
}
