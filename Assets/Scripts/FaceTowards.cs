using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowards : MonoBehaviour {
    [SerializeField] private PlayerRuntimeSet playerRuntimeSet;
    [SerializeField] private GameObject target;
    [SerializeField] private Vector3 offset;

    private void Start()
    {
        target = playerRuntimeSet.Get("player");
    }

    private void Update() {
        transform.LookAt(target.transform.position + offset);
    }
}
