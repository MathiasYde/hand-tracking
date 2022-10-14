using System;
using UnityEngine;

public class Timer {
    private float timer;
    private float timeout;

    public event Action onTimerEnd;

    public bool isFinished { get { return timer >= timeout; } }
    public float remainingTime { get { return timeout - timer; } }
    public Timer(float time) {
        this.timeout = time;
    }
    
    public float GetTimeout() {
        return timeout;
    }
    
    public void SetTimeout(float newTimeout) {
        timeout = newTimeout;
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
