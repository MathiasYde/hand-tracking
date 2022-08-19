using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestRecording : MonoBehaviour {
    [SerializeField] private RecordingStoppingCriteria stopCriteria;
    [SerializeField] private RecordingMethod recordingMethod;
    [SerializeField] private HandTrackRecording recording;

    private IEnumerator RequestRecord() {
        Debug.Log("Starting recording");
        yield return HandTracking.Record(recording);
        Debug.Log("Stopped recording");
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            HandTracking.stopCriteria = stopCriteria;
            HandTracking.recordingMethod = recordingMethod;
            StartCoroutine(RequestRecord());
        }
    }
}
