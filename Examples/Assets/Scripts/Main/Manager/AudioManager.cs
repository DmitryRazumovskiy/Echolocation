using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    private GameManager gameManager;
    private ObjectManager objectManager;
    public bool allSound;

    private AudioData audioData;
    //private AudioSource[] exceptions;
    List<AudioSource> audioSources;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        objectManager = gameManager.ObjectManager_;
        audioData = gameManager.AudioData_;
        audioSources = new List<AudioSource>();
    }

    public void SetAudio()
    {      
        if (allSound && Application.isPlaying)
        {
            List<InteractebleObject> allObjects = objectManager.GetListOfAllObjects();
            foreach (InteractebleObject interactebleObject in allObjects)
            {
                if (interactebleObject.HasAudioSource ||  interactebleObject.gameObject.GetComponent<MeshRenderer>() == null)
                {
                    continue;
                }
                interactebleObject.gameObject.AddComponent<ObjectAudioExperemental>();
                interactebleObject.HasAudioSource = true;
            }

        }    
        
    }

    public void AddAudioSource(AudioSource newAudioSource)
    {
        audioSources.Add(newAudioSource);
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

        List<InteractebleObject> childs = currentTarget.GetComponent<InteractebleObject>().GetAllChilds();
        AdjustPitchOfObjects(childs);

        if (!audioData.playSoundOnlyWhenTargeted)
        {
            foreach (InteractebleObject child in childs)
            {
                if (child.GetComponent<InteractebleObject>().HasAudioSource)
                {
                    child.GetComponent<ObjectAudioExperemental>().PlaySound();
                }

            }
        }        
    }    

    void AdjustPitchOfObjects(List<InteractebleObject> objects)
    {
        if (objects.Count == 0)
        {
            return;
        }
        float pitchStep = (audioData.maxPitch - audioData.minPitch)/objects.Count;
        objects.Sort(InteractebleObject.SortBySize);

        if (!audioData.reversePitch)
        {
            objects.Reverse();
        }
        

        for (int i = 0; i < objects.Count; i++)
        {
            float pitch = audioData.minPitch + i * pitchStep;
            objects[i].SetPitch(pitch);
            if (objects[i].GetAudioSource() != null)
            {
                objects[i].GetAudioSource().pitch = pitch;
            }
        }

    }
    
}
