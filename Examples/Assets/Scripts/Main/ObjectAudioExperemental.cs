using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectAudioExperemental : MonoBehaviour
{
    private GameManager gameManager;
    private ObjectManager objectManager;
    private AudioManager audioManager;


    private AudioData audioData;
    private RayCastController rayCastController;
    private Collider objectCollider;
    private AudioListener audioListener;
    private GameObject audioSourceObject;
    bool isTargeted;

    void Awake()
    {
        rayCastController = FindObjectOfType<RayCastController>();
        audioListener = FindObjectOfType<AudioListener>();
        objectCollider = GetComponent<Collider>();

        gameManager = FindObjectOfType<GameManager>();
        objectManager = gameManager.ObjectManager_;
        audioManager = gameManager.AudioManager_;
        audioData = gameManager.AudioData_;
    }


    void LateUpdate()
    {
        AdjustAudioSource();
    }

    public void Initialization()
    {
        //transform.tag = "Audio Source Object";
        audioSourceObject = new GameObject("Audio Source");
        audioSourceObject.transform.SetParent(transform);
        audioSourceObject.transform.localPosition = Vector3.zero;
        //audioSourceObject.tag = "Audio Source";

        CreateAudioSource(audioSourceObject.transform);
        AudioSource audio = audioSourceObject.GetComponent<AudioSource>();
        SetAudioSourceSettings(audio);
        GetComponent<InteractebleObject>().SetAudioSource(audio);
        audioManager.AddAudioSource(audio);
    }

    void AdjustAudioSource()
    {
        isTargeted = rayCastController.GetLastSelectedTargetId() == transform.GetComponent<InteractebleObject>().GetId() ? true : false;
        if (!isTargeted)
        {
            Bounds bounds = transform.GetComponent<MeshRenderer>().bounds;
            Vector3 playerPosition = audioListener.transform.position;

            Vector3 directionToPlayer =   playerPosition - bounds.center;
            //directionToPlayer.y = bounds.center.y;

            Ray ray = new Ray(bounds.center, directionToPlayer.normalized);
            float rayDistance = audioData.currentObjectDistance;
            if (audioData.showObjectRay)
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.yellow);
            }            
            ray.origin = ray.GetPoint(rayDistance);
            ray.direction = (playerPosition - ray.origin).normalized;

            RaycastHit hit;
            if (objectCollider.Raycast(ray, out hit, rayDistance))
            {
                
                audioSourceObject.transform.position = hit.point;                             
            }

            //Vector3 audioSourcePosition = playerPosition;

            //audioSourcePosition.x = Mathf.Clamp(audioSourcePosition.x, bounds.min.x, bounds.max.x);
            //audioSourcePosition.y = Mathf.Clamp(audioSourcePosition.y, bounds.min.y, bounds.max.y);
            //audioSourcePosition.z = Mathf.Clamp(audioSourcePosition.z, bounds.min.z, bounds.max.z);

            //audioSourceObject.transform.position = audioSourcePosition;
        }
    }

    public void CreateAudioSource(Transform AudioSourceObject)
    {
        if (AudioSourceObject.GetComponent<AudioSource>() == null)
        {
            AudioSource audioSource = AudioSourceObject.gameObject.AddComponent<AudioSource>();
            audioSource.clip = audioData.basicSound;
            audioSource.loop = true;
            audioSource.spatialBlend = 1;
        }
    }

    public void SetAudioSourceSettings(AudioSource audioSource)
    {
        InteractebleObject audioObject = audioSource.transform.parent.GetComponent<InteractebleObject>();

        audioSource.maxDistance = audioData.baseSoundDistance;
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        
        audioSource.pitch = ScaleToFrequency(audioObject);
        audioSource.volume = ScaleToVolume(audioObject);
        
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioData.soundRollOff);
    }

    public float ScaleToVolume(InteractebleObject interactebleObject)
    {
        float sizeModifier = interactebleObject.GetSizeModifier();
        float volume = audioData.baseVolume * sizeModifier;

        volume = Mathf.Clamp(volume, audioData.minVolume, audioData.maxVolume);
        interactebleObject.SetVolume(volume);
        return volume;
    }


    public float ScaleToFrequency(InteractebleObject interactebleObject)
    {
        float sizeModifier = interactebleObject.GetSizeModifier();
        float frequency = audioData.maxPitch - sizeModifier;

        frequency = Mathf.Clamp(frequency, audioData.minPitch, audioData.maxPitch);
        return frequency;
    }


    public void AdjustAudioSource(Vector3 rayHitPoint)
    {
        audioSourceObject.transform.position = rayHitPoint;
    }

    public GameObject GetAudioSourceObject()
    {
        return audioSourceObject;
    }       

    public void PlaySound()
    {
        audioSourceObject.GetComponent<AudioSource>().Play();
        audioSourceObject.GetComponent<AudioSource>().mute = false;
    }

    public void StopSound()
    {
        audioSourceObject.GetComponent<AudioSource>().Stop();
    }

}
