using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAudioExperemental : MonoBehaviour
{
    [SerializeField] private AudioData audioData;
    //[SerializeField] private AudioManager audioManager;
    private RayCastController rayCastController;

    AudioListener audioListener;
    private AudioClip sound;
    private float volume;
    private GameObject audioSourceObject;
    bool isTargeted;    

    void Start()
    {
        rayCastController = FindObjectOfType<RayCastController>();
        audioListener = FindObjectOfType<AudioListener>();
        Initialization();
    }

    void LateUpdate()
    {
        AdjustAudioSource();
    }

    public void Initialization()
    {
        transform.tag = "Audio Source Object";
        audioSourceObject = new GameObject("Audio Source");
        audioSourceObject.transform.SetParent(transform);
        audioSourceObject.transform.localPosition = Vector3.zero;
        audioSourceObject.tag = "Audio Source";

        CreateAudioSource(audioSourceObject.transform);
        SetAudioSourceSettings(audioSourceObject.GetComponent<AudioSource>());        
    }

    void AdjustAudioSource()
    {
        isTargeted = rayCastController.GetLastSelectedTargetId() == transform.GetComponent<InteractebleObject>().GetId() ? true : false;
        if (!isTargeted)
        {
            Bounds bounds = transform.GetComponent<MeshRenderer>().bounds;
            Vector3 playerPosition = audioListener.transform.position;
            Vector3 audioSourcePosition = playerPosition;

            audioSourcePosition.x = Mathf.Clamp(audioSourcePosition.x, bounds.min.x, bounds.max.x);
            audioSourcePosition.y = Mathf.Clamp(audioSourcePosition.y, bounds.min.y, bounds.max.y);
            audioSourcePosition.z = Mathf.Clamp(audioSourcePosition.z, bounds.min.z, bounds.max.z);

            audioSourceObject.transform.position = audioSourcePosition;
        }
    }

    public void AdjustAudioSource(Vector3 rayHitPoint)    
    {
        audioSourceObject.transform.position = rayHitPoint;
    }

    #region после переноса в audioManager - удалить
    private void CreateAudioSource(Transform AudioSourceObject)
    {
        if (AudioSourceObject.GetComponent<AudioSource>() == null)
        {
            AudioSource audioSource = AudioSourceObject.gameObject.AddComponent<AudioSource>();
            
            audioSource.clip = sound != null ? sound : audioData.basicSound;
            audioSource.loop = true;
            audioSource.spatialBlend = 1;
        }
    }
    
    public void SetAudioSourceSettings(AudioSource audioSource)
    {
        Transform parent = audioSource.transform.parent;

        audioSource.maxDistance = audioData.baseSoundDistance;
        audioSource.rolloffMode = AudioRolloffMode.Custom;        
        audioSource.pitch = ScaleToFrequency(parent);
        audioSource.volume = ScaleToVolume(parent);        
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioData.soundRollOff);
    }

    public float ScaleToVolume(Transform currentObject)
    {
        float sizeModifier = (currentObject.localScale.x + currentObject.localScale.y + currentObject.localScale.z) / 3;
        volume = audioData.baseVolume * sizeModifier;
        volume = Mathf.Clamp(volume, audioData.minVolume, audioData.maxVolume);
        return volume;
    }

    public float ScaleToFrequency(Transform currentObject)
    {
        float sizeModifier = (currentObject.localScale.x + currentObject.localScale.y + currentObject.localScale.z) / 3;
        float frequency = audioData.maxFrequency - sizeModifier;

        frequency = Mathf.Clamp(frequency, audioData.minFrequency, audioData.maxFrequency);
        return frequency;
    }
    #endregion

    public GameObject GetAudioSourceObject()
    {
        return audioSourceObject;
    }

    public void SetAudioSound(AudioClip audioClip)
    {
        sound = audioClip;
    }

    public void StopSound()
    {
        audioSourceObject.GetComponent<AudioSource>().Stop();
    }

    public void PlaySound()
    {
        audioSourceObject.GetComponent<AudioSource>().Play();
    }

    public void SetAudioData(AudioData data)
    {
        audioData = data;
    }
}
