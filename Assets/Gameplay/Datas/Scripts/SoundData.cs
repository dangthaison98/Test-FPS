using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "DTS Data/Sound Data", fileName = "Sound Data")]
public class SoundData : SerializedScriptableObject
{
    public Dictionary<SoundType, AudioClip> Sounds = new();
}

public enum SoundType
{
    
}