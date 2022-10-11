using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class WeaponManager : MonoBehaviour {
    [SerializeField] private GenericDictionary<string, Weapon> weapons = new GenericDictionary<string, Weapon>();
    [SerializeField] private GenericDictionary<SteamVR_Input_Sources, Weapon> equippedWeapons;
    [SerializeField] private GenericDictionary<SteamVR_Input_Sources, Hand> sources;

    private void Start() {
        sources = GetComponent<HandUpdater>().sources;
    }

    public void EquipWeapon(string weaponName) {
        GenericDictionary<SteamVR_Input_Sources, Hand> sources = GetComponent<HandUpdater>().sources;

        Debug.Log($"WeaponManager[{this.gameObject.name}].EquipWeapon({weaponName})");
        if (weapons.TryGetValue(weaponName, out Weapon weapon)) {
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
                    Transform unequippedTransform = oldWeapon.GetDefaultUnequipedTransform();
                    oldWeapon.transform.position = unequippedTransform.position;
                    oldWeapon.transform.rotation = unequippedTransform.rotation;
                }
            }

            hand.AttachObject(weapon.gameObject, GrabTypes.Scripted);
            weapon.OnEquip(this);
            equippedWeapons[handSource] = weapon;
        }
    }

}
