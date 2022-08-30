using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestRecording : MonoBehaviour {
    [SerializeField] private RecordingStoppingCriteria stopCriteria;
    [SerializeField] private RecordingMethod recordingMethod;
    [SerializeField] private HandTrackRecording recording;

    public void LogFinishRecording() {
        VRDebugConsole.Log("Finished recording");
    }

    private IEnumerator RequestRecord() {
        VRDebugConsole.Log("Starting recording");
        yield return HandTracking.Record(recording); // maybe this needs to be wrapped in StartCoroutine
        VRDebugConsole.Log("Stopped recording");
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            HandTracking.stopCriteria = stopCriteria;
            HandTracking.recordingMethod = recordingMethod;
            StartCoroutine(RequestRecord());
        }
    }
}
