using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

public class RecordingRecognitionActions : MonoBehaviour {
    [SerializeField]
    private GenericDictionary<SteamVR_Input_Sources, Hand> sources =
        new GenericDictionary<SteamVR_Input_Sources, Hand>();

    [SerializeField] private Transform head;

    public GameObject shield;

    private void Awake() {
        sources ??= GetComponent<HandUpdater>().sources;
    }

    public void AttachShield(HandTrackRecording recording) {
        AttachGameobject(recording.source, shield);
    }

    public void DetachObject(HandTrackRecording recording) {
        if (!sources.ContainsKey(recording.source)) { return; }
        Hand hand = sources[recording.source];
        hand.DetachObject(hand.currentAttachedObject);
    }

    public void AttachGameobject(SteamVR_Input_Sources source, GameObject gameObject) {
        Hand hand = sources[source];
        hand.AttachObject(gameObject, GrabTypes.Trigger);
    }
}
