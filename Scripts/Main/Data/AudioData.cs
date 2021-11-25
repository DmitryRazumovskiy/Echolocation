using UnityEngine;

[CreateAssetMenu()]
public class AudioData : ScriptableObject
{
    public AudioClip basicSound;
    public AudioClip touchSound;

    [Header("Настройки частоты")]
    [Range(0.1f, 3)] public float minPitch;
    [Range(0.1f, 3)] public float maxPitch;
    public bool reversePitch;

    [Header("Настройки громкости")]
    [Range(0, 1)] public float baseVolume;
    [Range(0, 1)] public float minVolume;
    [Range(0, 1)] public float maxVolume;

    [Header("Настройки дистанции проигрывания звука")]
    [Range(1, 15)] public float baseSoundDistance;
    [Range(1, 15)] public float minSoundDistance;
    [Range(1, 15)] public float maxSoundDistance;

    public AnimationCurve soundRollOff;

    [Header("Настройки дистанции прослушивания")]
    public float currentObjectDistance;
    public float minObjectDistance;
    public float maxObjectDistance;
    public float objectDistanceStep;
    public bool showObjectRay;

    public bool playSoundOnlyWhenTargeted;

    

}
