using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [SerializeField] private float spawnRingRadius = 10f;
    [SerializeField] private Transform origin;
    [SerializeField] List<GameObject> enemyPrefabs;
    [SerializeField] float spawnTimeInterval = 1f;
    [SerializeField] float spawnTimeMultiplier = 0.998f;
    
    private Timer spawnTimer;

    private void Awake() {
        spawnTimer = new Timer(spawnTimeInterval);
    }

    private void OnDrawGizmos() {
        Gizmos.Draw
    }

    private void Start() {
        spawnTimer.onTimerEnd += () => {
            SpawnEnemy();
            spawnTimer.SetTimeout(spawnTimer.GetTimeout() * spawnTimeMultiplier);
            spawnTimer.Reset();
        };
    }

    private void SpawnEnemy() {
        // spawn a random enemy in a ring around the origin
        GameObject enemyPrefab = enemyPrefabs[UnityEngine.Random.Range(0, enemyPrefabs.Count)];
        GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
        
        float angle = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
        enemy.transform.position = origin.position + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * spawnRingRadius;
    }

    private void Update() {
        spawnTimer.Update(Time.deltaTime);
    }
}