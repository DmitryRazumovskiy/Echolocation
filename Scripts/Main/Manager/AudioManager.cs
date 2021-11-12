using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    private GameManager gameManager;
    private AudioData audioData;
    private ObjectManager objectManager;
    public bool allSound;
    [SerializeField] private AudioSource[] exceptions;
    List<AudioSource> audioSources;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        objectManager = gameManager.ObjectManager_;
    }

    public void SetAudio(AudioData data)
    {
        audioData = data;

        if (allSound && Application.isPlaying)
        {
            Collider[] gameObjects = FindObjectsOfType<Collider>();
            foreach (Collider gO in gameObjects)
            {
                #region костыль для работы c ObjectAudioExperemental
                if (gO.CompareTag("Audio Source Object"))  
                {
                    continue;
                }
                #endregion
                CreateAudioSource(gO.transform);
            }
        }           


        audioSources = FindObjectsOfType<AudioSource>().ToList();
        
        foreach (AudioSource audioSource in audioSources)
        {
            #region костыль для работы c ObjectAudioExperemental
            if (audioSource.CompareTag("Audio Source"))
            {
                continue;
            }
            #endregion
            SetAudioSourceSettings(audioSource);
        }

        for (int i = 0; i < exceptions.Length; i++)
        {
            audioSources.Remove(exceptions[i]);
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
        InteractebleObject audioObject = audioSource.transform.GetComponent<InteractebleObject>();

        audioSource.maxDistance = audioData.baseSoundDistance;
        audioSource.rolloffMode = AudioRolloffMode.Custom;
        if (!exceptions.Contains(audioSource))
        {
            audioSource.pitch = ScaleToFrequency(audioObject);
            audioSource.volume = ScaleToVolume(audioObject);
        }
        audioSource.SetCustomCurve(AudioSourceCurveType.CustomRolloff, audioData.soundRollOff);
    }

    public void PlaySoundsOfCurrentLayer(int layer)
    {
        foreach (AudioSource audioSource in audioSources)
        {
            int objectLayer = audioSource.GetComponent<InteractebleObject>().GetLayer();
            if (objectLayer == layer+1)
            {
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
            }
        }
    }

    public void MuteAll()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.Stop();
            //audioSource.volume = 0.1f;
        }
    }

    public void PlaySoundsOfCurrentTargetChilds(int idOfTarget)
    {        

        Transform currentTarget = objectManager.FindTranformById(idOfTarget);
        if (currentTarget == null)
        {
            print("Шеф! Все пропало! Все пропало!");
            return;
        }

        MuteAll();

        int childCount = currentTarget.childCount;
        for (int i = 0; i < childCount; i++)
        {
            #region костыль для работы c ObjectAudioExperemental
            if (currentTarget.GetChild(i).CompareTag("Audio Source Object"))
            {
                currentTarget.GetChild(i).GetComponent<ObjectAudioExperemental>().PlaySound();
            }
            #endregion

            if (currentTarget.GetChild(i).GetComponent<AudioSource>()!=null)
            {                
                
                //currentTarget.GetChild(i).GetComponent<AudioSource>().volume = currentTarget.GetChild(i).GetComponent<InteractebleObject>().GetVolume();
                currentTarget.GetChild(i).GetComponent<AudioSource>().Play();
            }            
        }
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
        float frequency = audioData.maxFrequency - sizeModifier;
        
        frequency = Mathf.Clamp(frequency, audioData.minFrequency, audioData.maxFrequency);
        return frequency;
    }
    
}
