using UnityEngine;

[CreateAssetMenu(menuName = "Recording Methods/Time based recording method")]
public class TimeBasedRecordingMethod : RecordingMethod {
    [SerializeField] private float timeInterval = 1f;
    private float timer = 0f;
    public override void StartRecording() {
        timer = 0;
    }

    public override void StopRecording() {

    }

    public override void UpdateRecording() {
        timer += Time.deltaTime;
        if (timer >= timeInterval) {
            timer = 0;

            HandTracking.CaptureHandData();
        }
    }
}
