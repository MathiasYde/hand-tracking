using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowards : MonoBehaviour {
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    private void Update() {
        transform.LookAt(target.transform.position + offset);
    }
}
