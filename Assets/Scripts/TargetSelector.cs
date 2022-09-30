using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelector : MonoBehaviour {
    [SerializeField] private Transform head;

    private Physics physics;

    [SerializeField] private float coneRadius;
    [SerializeField] private float coneDistance;
    [SerializeField] private float coneAngle;

    private void Awake() {
        physics = new Physics();
    }

    private void Update() {
        RaycastHit[] hits = physics.ConeCastAll(head.position, coneRadius, head.forward, coneDistance, coneAngle);
        if (hits.Length == 0) { return; }

        int closestIndex = 0;
        for (int i = 0; i < hits.Length; i++) {
            if (hits[i].distance < hits[closestIndex].distance) {
                closestIndex = i;
            }
        }

        RaycastHit hit = hits[closestIndex];
        if (hit.transform.TryGetComponent<Targetable>(out Targetable targetable)) {
            targetable.Highlight();
        }
    }
}
