using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class DeveloperTools {
    [MenuItem("Developer/Kill Player")]
    public static void KillPlayer() {
        PlayerRuntimeSet playerRuntimeSet = (PlayerRuntimeSet)ScriptableObject.FindObjectsOfTypeAll(typeof(PlayerRuntimeSet))[0];
        if (playerRuntimeSet == null) {
            Debug.LogWarning("Could not find object<PlayerRuntimeSet>");
            return;
        }

        GameObject player = playerRuntimeSet.Get("player");
        if (player == null) {
            Debug.LogWarning("player field of PlayerRuntimeSet is null");
            return;
        }

        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth == null) {
            Debug.LogWarning("player does not have PlayerHealth component");
            return;
        }

        playerHealth.OnDeath();
    }

}
