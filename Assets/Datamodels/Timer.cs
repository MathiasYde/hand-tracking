using System;
using UnityEngine;

public class Timer {
    private float timer;
    private float time;

    public event Action onTimerEnd;

    public bool isFinished { get { return timer >= time; } }
    public float remainingTime { get { return time - timer; } }
    public Timer(float time) {
        this.time = time;
    }
    public void Update(float deltaTime) {
        timer += deltaTime;
        if (isFinished) {
            onTimerEnd?.Invoke();
        }
    }

    public void Reset() {
        timer = 0.0f;
    }
}
