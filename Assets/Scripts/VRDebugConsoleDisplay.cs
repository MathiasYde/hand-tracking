using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VRDebugConsoleDisplay : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI output;

    private void Update() {
        output.text = VRDebugConsole.GetText();
    }
}
