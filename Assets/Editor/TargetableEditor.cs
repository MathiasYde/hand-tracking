using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Targetable))]
public class TargetableEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Targetable script = (Targetable)target;

        if (GUILayout.Button("Damage", GUILayout.Height(40)))
        {
            script.TakeDamage(1);
        }

        if (GUILayout.Button("Kill", GUILayout.Height(40)))
        {
            script.TakeDamage(999);
        }
    }
}
