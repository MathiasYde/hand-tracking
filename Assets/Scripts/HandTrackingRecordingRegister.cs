using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrackingRecordingRegister : MonoBehaviour {
    [SerializeField] private List<HandTrackRecording> recordings;

    private void Start()
    {
        foreach (HandTrackRecording recording in recordings)
        {
            HandTracking.AddRecordingFromRecognitionSet(recording);
        }
    }
}
