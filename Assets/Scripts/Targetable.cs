using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Targetable : MonoBehaviour {
    [SerializeField] private GameObject highlightGameObject;

    [SerializeField] [Min(1)] private int maxHealth;
    [SerializeField] private int currentHealth;
    public float healthProgress => currentHealth / maxHealth;

    public IntUnityEvent onTakeDamage;
    public UnityEvent onDeath;

    private float highlightDisableTime = 1.0f;
    private Timer timer;


    private void Awake() {
        onTakeDamage = new IntUnityEvent();
        onDeath = new UnityEvent();
        timer = new Timer(highlightDisableTime);
    }

    private void Start() {
        currentHealth = maxHealth;
        timer.onTimerEnd += () => {
            highlightGameObject.SetActive(false);
            timer.Reset();
        };
    }

    public void Highlight() {
        highlightGameObject.SetActive(true);
    }

    private void Update() {
        if (highlightGameObject.activeSelf) {
            timer.Update(Time.deltaTime);
        }
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;
        onTakeDamage?.Invoke(damage);
        if (currentHealth <= 0) {

            onDeath?.Invoke();
        }
    }
}
