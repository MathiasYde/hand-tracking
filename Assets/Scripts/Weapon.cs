using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Weapon : MonoBehaviour {
    [SerializeField] private int damage;
    
    [SerializeField] private WeaponRuntimeSet weaponRuntimeSet;
    [SerializeField] private Optional<string> runtimeSetRegisteringName;
    [SerializeField] private Transform defaultUnequipedTransform;
    [SerializeField] private Transform handOffsetTransform;
    [SerializeField] private SteamVR_Input_Sources preferedHand;

    private void OnEnable() {
        if (runtimeSetRegisteringName.Enabled) {
            weaponRuntimeSet.Register(runtimeSetRegisteringName.Value, this);
        }
    }

    private void OnDisable() {
        if (runtimeSetRegisteringName.Enabled) {
            weaponRuntimeSet.Unregister(runtimeSetRegisteringName.Value);
        }
    }

    public void OnCollisionEnter(Collision other) {
        if (other.transform.TryGetComponent<Targetable>(out Targetable targetable))
        {
            Debug.Log($"Applying {damage} to Targetable with name {targetable.name}");
            targetable.TakeDamage(damage);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.TryGetComponent<Targetable>(out Targetable targetable)) {
            Debug.Log($"Applying {damage} to Targetable with name {targetable.name}");
            targetable.TakeDamage(damage);
        }
    }

    public Transform GetHandOffsetTransform() { return handOffsetTransform; }
    public Transform GetDefaultUnequipedTransform() { return defaultUnequipedTransform; }
    public SteamVR_Input_Sources GetPreferedHand() { return preferedHand; }

    public void OnEquip(WeaponManager manager) { }

    public void OnUnequip(WeaponManager manager) { }
}
