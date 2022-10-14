using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {
    [SerializeField] private Optional<Transform> target;
    [SerializeField] private float speed;

    public void SetTarget(Transform target) {
        this.target = new Optional<Transform>(target);
    }

    void Update() {
        if (target.Enabled) {
            transform.LookAt(target.Value);
        }

        transform.position += transform.forward * Time.deltaTime * speed;
    }
}
