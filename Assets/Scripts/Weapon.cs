using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Weapon : MonoBehaviour {
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

    public Transform GetHandOffsetTransform() { return handOffsetTransform; }
    public Transform GetDefaultUnequipedTransform() { return defaultUnequipedTransform; }
    public SteamVR_Input_Sources GetPreferedHand() { return preferedHand; }

    public void OnEquip(WeaponManager manager) { }

    public void OnUnequip(WeaponManager manager) { }
}
