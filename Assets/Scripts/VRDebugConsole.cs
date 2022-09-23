using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VRDebugConsole {
    private static List<string> lines = new List<string>();
    private static uint maxLines = 8;
    public static void Log(string text) {
        Debug.Log(text);
        lines.Add($"{Time.realtimeSinceStartup}:{text}");
        if (lines.Count > maxLines) {
            lines.RemoveAt(0);
        }
    }

    public static string GetText() {
        string text = "";

        foreach (string line in lines) {
            text += line;
            text += "\n";
        }

        return text;
    }

}
