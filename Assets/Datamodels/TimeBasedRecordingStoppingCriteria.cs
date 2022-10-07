using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[CreateAssetMenu(fileName = "Stop Criterias/TimeBasedRecordingStoppingCriteria")]
public class TimeBasedRecordingStoppingCriteria : RecordingStoppingCriteria {
    [SerializeField] private float timeout;
    private Timer timer;

    public void SetTimeout(float timeout) => this.timeout = timeout;
    public override bool ShouldStop() => timer.isFinished;

    public override void StartRecording() {
        timer = new Timer(timeout);
    }

    public override void StopRecording() {}

    public override void UpdateRecording(GenericDictionary<SteamVR_Input_Sources, HandPoseData> handPoses) {
        timer.Update(Time.deltaTime);
    }
}
