using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] private PlayerRuntimeSet playerRuntimeSet;

    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float deathTime = 30.0f;
    private Timer deathTimer;


    private void Awake()
    {
        deathTimer = new Timer();
    }
    void Start() {
        target = playerRuntimeSet.Get("player").transform;
        deathTimer.SetTimeout(deathTime);
        deathTimer.onTimerEnd += () =>
        {
            Destroy(this.gameObject);
        };
    }

    void Update() {
        deathTimer.Update(Time.deltaTime);
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {

        }   
    }
}
