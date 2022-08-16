using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestRecording : MonoBehaviour {

    private IEnumerator RequestRecord() {
        yield return HandTracking.Record();
    }
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartCoroutine(RequestRecord());
        }
    }
}
