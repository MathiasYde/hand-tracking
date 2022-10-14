using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerEnterEvent : MonoBehaviour {

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("I collided with something!");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"I collided with a trigger {other.name}");
    }

}
