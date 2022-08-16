using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VRDebugConsoleDisplat : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI output;

    private void Update() {
        output.text = VRDebugConsole.GetText();
        VRDebugConsole.Reset();
    }
}