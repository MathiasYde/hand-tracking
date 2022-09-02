using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

[CreateAssetMenu(menuName ="Hand Track Recording")]
public class HandTrackRecording : ScriptableObject {
	public GenericDictionary<SteamVR_Input_Sources, List<HandPoseData>> handData = new GenericDictionary<SteamVR_Input_Sources, List<HandPoseData>>();

	public Optional<float> positionalMaxDistance = new Optional<float>(0.4f); // meters radius
	public Optional<float> curlMaxDistance = new Optional<float>(4.0f); 
	public int count;

	public UnityEvent onRecognize;

	public void Reset() {
		handData.Clear();
		count = 0;
    }
}
