using UnityEngine;

[CreateAssetMenu(menuName = "Recording Methods/Time based recording method")]
public class TimeBasedRecordingMethod : RecordingMethod {
    [SerializeField] private float timeout;
    private Timer timer;
    public override void StartRecording() {
        timer = new Timer(timeout);
        timer.onTimerEnd += onTimerEnd;
    }

    public void onTimerEnd() {
        timer.Reset();
        HandTracking.CaptureHandData();
    }

    public override void StopRecording() {}

    public override void UpdateRecording() {
        Debug.Log($"Updating time based recording method");
        timer.Update(Time.deltaTime);
    }
}
