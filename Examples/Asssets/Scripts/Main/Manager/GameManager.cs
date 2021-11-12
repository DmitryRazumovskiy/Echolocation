using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private ObjectManager objectManager;
    [SerializeField] private AudioData audioData;

    [SerializeField] private Text currentLayerInterface;
    [SerializeField] private Text currentTargetInterface;   

    int currentTarget;
    public int CurrentTarget
    {
        get
        {
            return currentTarget;
        }
        set
        {
            if (value==currentTarget)
            {
                return;
            }
            currentTarget = value;
            RefreshTarget();
        }

    }

    int currentLayer;
    public int CurrentLayer
    {
        get
        {
            return currentLayer;
        }
        set
        {
            if (value == currentLayer)
            {
                return;
            }
            currentLayer = value;
            RefreshLayer();
        }

    }

    public AudioManager AudioManager_
    {
        get
        {
            return audioManager;
        }
    }

    public ObjectManager ObjectManager_
    {
        get
        {
            return objectManager;
        }
    }

    public AudioData AudioData_
    {
        get
        {
            return audioData;
        }
    }

    private void Start()
    {       
        ApplySettings();
    }

    public void ApplySettings()
    {
        if (Application.isPlaying)
        {
            currentTarget = 0;
            currentLayer = 0;
            objectManager.CreateObjectHierarchy();

            audioManager.SetAudio(audioData);
            audioManager.PlaySoundsOfCurrentTargetChilds(currentTarget);
        }                
    }  

    void RefreshUI()
    {
        currentLayerInterface.text = currentLayer.ToString();
        currentTargetInterface.text = currentTarget.ToString();
    }

    void RefreshTarget()
    {
        currentLayer = objectManager.FindLayerById(currentTarget);
        audioManager.PlaySoundsOfCurrentTargetChilds(currentTarget);

        RefreshUI();
    }

    void RefreshLayer()
    {
        currentTarget = 0;
        audioManager.PlaySoundsOfCurrentLayer(currentLayer);

        RefreshUI();
    }   
}
