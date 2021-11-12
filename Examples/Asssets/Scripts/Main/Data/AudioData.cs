using UnityEngine;

[CreateAssetMenu()]
public class AudioData : ScriptableObject
{
    public AudioClip basicSound;
    public AudioClip touchSound;

    [Range(0.1f, 3)] public float minFrequency;
    [Range(0.1f, 3)] public float maxFrequency;

    [Range(0, 1)] public float baseVolume;
    [Range(0, 1)] public float minVolume;
    [Range(0, 1)] public float maxVolume;

    [Range(1, 15)] public float baseSoundDistance;
    [Range(1, 15)] public float minSoundDistance;
    [Range(1, 15)] public float maxSoundDistance;

    public AnimationCurve soundRollOff;
}
