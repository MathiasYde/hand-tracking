using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        List<Targetable> targets = new List<Targetable>();
        
        foreach (RaycastHit hit in hits) {
            if (hit.transform.TryGetComponent<Targetable>(out Targetable targetable)) {
                Debug.DrawLine(head.position, hit.point, Color.red);
                targets.Add(targetable);
            }
        }
        
        if (targets.Count == 0) { return; }
        
        // sort by distance and get the closest
        Targetable closestTarget = targets.OrderBy(target => Vector3.Distance(head.position, target.transform.position)).First();
        closestTarget.Highlight();
    }
}
