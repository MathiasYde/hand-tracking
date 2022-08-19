using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestRecording : MonoBehaviour {
    [SerializeField] private RecordingStoppingCriteria stopCriteria;

    private IEnumerator RequestRecord() {
        yield return HandTracking.Record(stopCriteria);
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(RequestRecord());
        }
    }
}
