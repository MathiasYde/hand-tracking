using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RuntimeSet<T> : ScriptableObject {
    //[SerializeField] private bool allowOverride = false;
    [SerializeField] private GenericDictionary<string, T> entries = new GenericDictionary<string, T>();

    public T Get(string key) => entries.Get(key, default);

    public void Register(string key, T value) {
        Debug.Log($"{key}:{value}<{value.GetType()}>");
        // prevent overriding
        if (entries[key] != null) { }

        if (entries.ContainsKey(key)) {
            entries[key] = value;
        }
    }

    public void Unregister(string key) {
        if (entries.ContainsKey(key)) {
            entries[key] = default;
        }
    }
}
