using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lifetime : MonoBehaviour {
    [SerializeField] private float timeout;
    [SerializeField] private UnityEvent onDeathEvent;

    private Timer timer;


    private void Awake() {
        timer = new Timer();
        onDeathEvent = new UnityEvent();
    }

    private void Start() {
        timer.SetTimeout(timeout);
        timer.onTimerEnd += () => {
            onDeathEvent?.Invoke();
            Destroy(this.gameObject);
        };
    }

    private void Update() {
        timer.Update(Time.deltaTime);
    }

}
