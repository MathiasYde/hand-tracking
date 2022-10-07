using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class RecordingActionsManager : MonoBehaviour {
    [SerializeField] private PlayerRuntimeSet playerRuntimeSet;

    [SerializeField]
    private GenericDictionary<SteamVR_Input_Sources, Hand> sources;

    [SerializeField] private GameObject shieldPrefab;

    private void Start() {
        sources = playerRuntimeSet.Get("player").GetComponent<HandUpdater>().sources;
    }

    public void AttachShield(HandTrackRecording recording) {
        AttachGameObject(recording.source, shieldPrefab);
    }

    public void AttachGameObject(SteamVR_Input_Sources source, GameObject prefab) {
        Hand hand = playerRuntimeSet.Get("player").GetComponent<HandUpdater>().sources[source];
        
        if (hand.currentAttachedObject != null) {
            hand.DetachObject(hand.currentAttachedObject);
        }

        GameObject instance = Instantiate(prefab, hand.transform.position, hand.transform.rotation);
        hand.AttachObject(instance, GrabTypes.Grip, Hand.defaultAttachmentFlags);
    }
}
