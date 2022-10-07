using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterToRuntimeSet : MonoBehaviour {
    public PlayerRuntimeSet set;
    public string key;

    private void OnEnable() {
        set.Register(key, this.gameObject);
    }

    private void OnDisable() {
        set.Unregister(key);
    }
}
