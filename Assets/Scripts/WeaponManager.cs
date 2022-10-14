using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class WeaponManager : MonoBehaviour {
    [SerializeField] private Transform head;

    [SerializeField] private SteamVR_Action_Boolean gripAction;
    
    [SerializeField] private PlayerRuntimeSet playerRuntimeSet;
    
    [SerializeField] private WeaponRuntimeSet weaponRuntimeSet;
    [SerializeField] private GenericDictionary<SteamVR_Input_Sources, Weapon> equippedWeapons;

    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Vector3 fireballSpawnOffset;

    [SerializeField] private int fireBallDamage = 40;

    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private float wallDistance = 2;

    [SerializeField] private LayerMask groundLayerMask;

    private void Start() {
        gripAction.AddOnChangeListener(OnGripAction, SteamVR_Input_Sources.LeftHand);
        gripAction.AddOnChangeListener(OnGripAction, SteamVR_Input_Sources.RightHand);
    }

    private void OnGripAction(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState) {
        if (!newState) { return; }
        
        Debug.Log("Grip action");

        GameObject player = playerRuntimeSet.Get("player");
        GenericDictionary<SteamVR_Input_Sources, Hand> sources = player.GetComponent<HandUpdater>().sources;
        GenericDictionary<SteamVR_Input_Sources, Weapon> equippedWeapons = player.GetComponent<WeaponManager>().equippedWeapons;

        if (equippedWeapons.TryGetValue(fromSource, out Weapon weapon)) {
            Debug.Log(weapon == null);  
            if (weapon == null) {
                return;}
            
            Debug.Log("Detaching weapon from gripAction");
            Hand hand = sources[fromSource];
            hand.DetachObject(weapon.gameObject);
            Transform unequippedPosition = weapon.GetDefaultUnequipedTransform();
            weapon.transform.SetPositionAndRotation(unequippedPosition.position, unequippedPosition.rotation);
            equippedWeapons[fromSource] = null;
        }
    }

    public void CastWall()
    {
        GameObject head = playerRuntimeSet.Get("head");

        GameObject wall = Instantiate(wallPrefab, head.transform.position, Quaternion.identity);
        wall.transform.position = head.transform.forward * wallDistance;

        RaycastHit hit;
        if (Physics.Raycast(wall.transform.position, Vector3.up, out hit, Mathf.Infinity, groundLayerMask))
        {
            wall.transform.position = hit.transform.position;
        }

        if (Physics.Raycast(wall.transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayerMask))
        {
            wall.transform.position = hit.transform.position;
        }

        wall.transform.LookAt(head.transform);

    }

    public void CastFireball() {
        GameObject player = playerRuntimeSet.Get("player");
        GameObject head = playerRuntimeSet.Get("head");
        

        GameObject fireball = Instantiate(fireballPrefab, head.transform.position + fireballSpawnOffset, Quaternion.identity);
        fireball.transform.forward = head.transform.forward;

        if (player.TryGetComponent<TargetSelector>(out TargetSelector targetSelector)) {
            Targetable target = targetSelector.GetClosestTarget;
            if (target == null) { return; }

            if (fireball.TryGetComponent<MoveForward>(out MoveForward moveForward)) {
                moveForward.SetTarget(target.transform);
            }
        }
    }
    
    public void EquipWeapon(string weaponName) {
        GameObject player = playerRuntimeSet.Get("player");

        GenericDictionary<SteamVR_Input_Sources, Weapon> equippedWeapons = player.GetComponent<WeaponManager>().equippedWeapons;
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
