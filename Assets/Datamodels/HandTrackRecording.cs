using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

[CreateAssetMenu(menuName ="Hand Track Recording")]
public class HandTrackRecording : ScriptableObject {
	public GenericDictionary<SteamVR_Input_Sources, List<HandPoseData>> handData;
	public float maxDistance = 1f; // meters, radius
	public int count;

	public UnityEvent onRecognize;
}
