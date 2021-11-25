using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCastController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    private AudioData audioData;
    private GameManager gameManager;
    private ObjectManager objectManager;
    private AudioManager audioManager;
    private AudioSource audioPlayer;

    [Header("Настройка луча")]
    [SerializeField] private float soundRayLenght;
    [SerializeField] private float touchRayLenght;
    [SerializeField] private bool touchSound;

    [Header("Настройка интерфейса")]
    [SerializeField] private Text targetInterface;
    [SerializeField] private Image point;

    private Transform previousHeardTarget;
    private Transform previousTouchedTarget;

    private int lastSelectedTargetId;
    private int layerMask = ~(1 << 8);

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        objectManager = gameManager.ObjectManager_;
        audioData = gameManager.AudioData_;
        audioManager = gameManager.AudioManager_;
        audioPlayer = GetComponent<AudioSource>();
    }

    private void Update()
    {
        RayCasting();
    }

    private void RayCasting()
    {

        RaycastHit hit;

        Ray soundRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward * soundRayLenght);
        Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * soundRayLenght, Color.red);

        if (Physics.Raycast(soundRay, out hit, soundRayLenght, layerMask))
        {
            Transform objectHit = SelectObject(hit.transform);
            if (objectHit != null)
            {                

                targetInterface.text = objectHit.name;
                point.color = new Color(0.34f, 0.68f, 0.87f, 0.6f);
                InteractebleObject targetedObject = objectHit.GetComponent<InteractebleObject>();
                if (targetedObject != null)
                {
                    if (targetedObject.HasAudioSource)
                    {
                        ObjectAudioExperemental objectAudio = targetedObject.GetComponent<ObjectAudioExperemental>();
                        objectAudio.AdjustAudioSource(hit.point);
                        SetSoundWhenTargeted(objectAudio.GetAudioSourceObject().transform);                        
                    }
                    else
                    {
                        SetBasicSoundDistance(previousHeardTarget);
                    }

                    lastSelectedTargetId = objectHit.GetComponent<InteractebleObject>().GetId();
                }

                

            }

        }
        else
        {
            UnselectObject();
        }

        Touching();

    }
    void UnselectObject()
    {
        targetInterface.text = "Нет цели";
        point.color = Color.white;
        lastSelectedTargetId = -1;
        SetBasicSoundDistance(previousHeardTarget);

        
    }

    public void OnSelectObject()
    {
        if (lastSelectedTargetId == -1 )
        {           
            return;
        }
        if (objectManager.FindTranformById(lastSelectedTargetId).GetComponent<InteractebleObject>().GetAllChilds().Count==0)
        {
            return;
        }        

        gameManager.CurrentTarget = lastSelectedTargetId;
    }

    public void OnSelectParent()
    {
        Transform currentTarget = objectManager.FindTranformById(gameManager.CurrentTarget);
        if (currentTarget.parent != null)
        {
            gameManager.CurrentTarget = currentTarget.parent.GetComponent<InteractebleObject>().GetId();
        }
    }

    private Transform SelectObject(Transform hit)
    {
        InteractebleObject hitInteracteble = hit.GetComponent<InteractebleObject>();
        int hitLayer = hitInteracteble.GetLayer();
        int hitId = hitInteracteble.GetId();

        List<InteractebleObject> childrensOfCurrentTarget = objectManager.FindAllChildrenOfObject(gameManager.CurrentTarget, new List<InteractebleObject>());

        if (childrensOfCurrentTarget.Contains(objectManager.FindObjectById(hitId)))
        {
            if (hitLayer > gameManager.CurrentLayer + 1)
            {
                return SelectObject(hitInteracteble.GetParent());
            }
            return hit;
        }

        return null;
    }

    private void Touching()
    {
        Ray touchRay = new Ray(playerCamera.transform.position, playerCamera.transform.forward * touchRayLenght);
        Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.position + playerCamera.transform.forward * touchRayLenght, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(touchRay, out hit, touchRayLenght, layerMask))
        {
            Transform objectHit = hit.transform;
            if (touchSound && audioPlayer != null) 
            {
                PlayTouchSound(objectHit);
            }
            point.color = Color.red;
        }
        else
        {
            previousTouchedTarget = null;
        }
    }

    private void SetSoundWhenTargeted(Transform objectHit)
    {
        AudioSource currentTarget = GetAudioSourceOfObject(objectHit);
        AudioSource previousTarget = GetAudioSourceOfObject(previousHeardTarget);
        if (currentTarget != null)
        {
            if (previousHeardTarget != null && currentTarget.gameObject != previousHeardTarget.gameObject)
            {
                previousTarget.maxDistance = audioData.baseSoundDistance;                
            }


            currentTarget.maxDistance = soundRayLenght + 1;
            if (audioData.playSoundOnlyWhenTargeted && !currentTarget.isPlaying)
            {
                audioManager.MuteAll();
                currentTarget.Play();
            }

            previousHeardTarget = currentTarget.transform;
        }
        else
        {
            SetBasicSoundDistance(previousHeardTarget);
        }
    }

    public AudioSource GetAudioSourceOfObject(Transform target)
    {
        if (target != null)
        {
            InteractebleObject targetObject = target.parent.GetComponent<InteractebleObject>();
            if (targetObject != null)
            {
                if (targetObject.HasAudioSource)
                {
                    return targetObject.GetAudioSource();
                }
            }
        }        
        return null;
    }

    private void SetBasicSoundDistance(Transform targetTranform)
    {
        AudioSource target = GetAudioSourceOfObject(targetTranform);
        if (target != null)
        {
            target.maxDistance = audioData.baseSoundDistance;
            previousHeardTarget = null;
        }

        if (audioData.playSoundOnlyWhenTargeted)
        {
            audioManager.MuteAll();
        }
    }

    private void PlayTouchSound(Transform currentTarget)
    {
        if (currentTarget != null)
        {
            if (previousTouchedTarget != currentTarget)
            {
                audioPlayer.PlayOneShot(audioData.touchSound);
                previousTouchedTarget = currentTarget;
            }
        }
    }

    public int GetLastSelectedTargetId()
    {
        return lastSelectedTargetId;
    }
}

