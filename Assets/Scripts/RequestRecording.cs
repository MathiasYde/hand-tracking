using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestRecording : MonoBehaviour {
    [SerializeField] private RecordingStoppingCriteria stopCriteria;
    [SerializeField] private RecordingMethod recordingMethod;
    [SerializeField] private HandTrackRecording recording;
    [SerializeField] private HandTrackRecordingSet recordingSet;

    public void LogOnRecognized()
    {
        VRDebugConsole.Log("Recognized recording");
    }

    public void LogFinishRecording() {
        VRDebugConsole.Log("Finished recording");
    }

    public void Start()
    {
        HandTracking.recordingSet = recordingSet;
    }

    private IEnumerator RequestRecord() {
        VRDebugConsole.Log("Starting recording");
        yield return HandTracking.Record(recording);
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
