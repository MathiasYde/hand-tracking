using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Hand Track Recording Set")]
public class HandTrackRecordingSet : ScriptableObject
{
    public List<Tuple<HandTrackRecording, int>> recordings = new List<Tuple<HandTrackRecording, int>>();
}
