using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VRDebugConsole {
    private static string textBuffer;
    public static void Log(string text) {
        textBuffer += text;
        textBuffer += "\n";
    }

    public static void Reset() {
        textBuffer = "";
    }

    public static string GetText() { return textBuffer; }

}
