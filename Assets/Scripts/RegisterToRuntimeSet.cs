using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterToRuntimeSet : MonoBehaviour {
    public PlayerRuntimeSet set;

    [SerializeField] private GenericDictionary<string, GameObject> entries;

    private void OnEnable() {
        foreach ((string key, GameObject value) in entries) {
            set.Register(key, value);
        }
    }

    private void OnDisable() {
        foreach (string key in entries.Keys) {
            set.Unregister(key);
        }
    }
}
