using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class WeaponManager : MonoBehaviour {
    [SerializeField] private SteamVR_Action_Boolean gripAction;
    
    [SerializeField] private PlayerRuntimeSet playerRuntimeSet;
    
    [SerializeField] private WeaponRuntimeSet weaponRuntimeSet;
    [SerializeField] private GenericDictionary<SteamVR_Input_Sources, Weapon> equippedWeapons;

    private void Start() {
        gripAction.AddOnChangeListener(OnGripAction, SteamVR_Input_Sources.LeftHand);
        gripAction.AddOnChangeListener(OnGripAction, SteamVR_Input_Sources.RightHand);
    }D

    private void OnGripAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState) {
        if (!newState) { return; }

        GameObject player = playerRuntimeSet.Get("player");
        GenericDictionary<SteamVR_Input_Sources, Hand> sources = player.GetComponent<HandUpdater>().sources;

        if (equippedWeapons.TryGetValue(fromSource, out Weapon weapon)) {
            Hand hand = sources[fromSource];
            Debug.Log($"is hand null? {hand == null}");
            Debug.Log($"is weapon null? {weapon == null}");
            Debug.Log($"is weapon gameobject null? {weapon.gameObject == null}");
            hand.DetachObject(weapon.gameObject);
            equippedWeapons[fromSource] = null;
        }
    }
    
    public void EquipWeapon(string weaponName) {
        GameObject player = playerRuntimeSet.Get("player");

        GenericDictionary<SteamVR_Input_Sources, Hand> sources = player.GetComponent<HandUpdater>().sources;
        WeaponManager manager = player.GetComponent<WeaponManager>();
        
        if (manager.weaponRuntimeSet.TryGet(weaponName, out Weapon weapon)) {
            SteamVR_Input_Sources handSource = weapon.GetPreferedHand();
            Debug.Log(sources[SteamVR_Input_Sources.LeftHand] == null);
            Hand hand = sources.Get(handSource, null);
            if (hand == null) {
                Debug.LogWarning($"Given hand source {handSource} yiels null Hand component");
                return;
            }

            if (equippedWeapons.TryGetValue(handSource, out Weapon oldWeapon)) {
                
                if (oldWeapon != null) {
                    hand.DetachObject(oldWeapon.gameObject);
                    oldWeapon.OnUnequip(this);
                    equippedWeapons[handSource] = null;
                    Transform unequippedTransform = oldWeapon.GetDefaultUnequipedTransform();
                    oldWeapon.transform.SetPositionAndRotation(
                        unequippedTransform.position,
                        unequippedTransform.rotation);
                }
            }

            hand.AttachObject(weapon.gameObject, GrabTypes.Scripted);
            weapon.OnEquip(this);
            equippedWeapons[handSource] = weapon;
        }
    }

}
