using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

[CreateAssetMenu(menuName ="Hand Track Recording")]
public class HandTrackRecording : ScriptableObject {
	public GenericDictionary<SteamVR_Input_Sources, List<HandPoseData>> handData = new GenericDictionary<SteamVR_Input_Sources, List<HandPoseData>>();

	public float positionalMaxDistance = 0.4f; // meters radius
	public float curlMaxDistance = 0.4f; 
	public int count;

	public UnityEvent onRecognize;
}
