using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class CameraControllerOld : MonoBehaviour/*, GameControls.ICameraActions*/
{
    public AudioData audioData;

    [Header("Настройка чувствительности")]
    public float mouseSensitivity;
    public float rotationSmoothTime;

    [Header("Настройка положения камеры")]
    public Transform firstPersonCameraPosition;

    [Header("Ограничение поворота камеры")]
    public Vector2 pitchMinMax;

    [Header("Настройка интерфейса")]
    public bool lockCursor;

    [Header("Настройка луча")]
    public float soundRayLenght = 1;
    public float touchRayLenght;
    public bool touchSound;

    public Text targetInterface;
    public Image point;
    AudioSource audioPlayer;
    Transform previousTouchedTarget;
    AudioSource previousHeardTarget;

    GameManager gameManager;
    ObjectManager objectManager;

    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    [SerializeField] private GameControls cameraControls;

    int lastSelectedTargetId;

    int layerMask = ~(1 << 8);

    public Vector2 MousePosition
    {
        get
        {
            return new Vector2(yaw, pitch);
        }
        set
        {
            yaw = value.x * mouseSensitivity;
            pitch = value.y * mouseSensitivity;
        }
    }

    float yaw;
    float pitch;

    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        cameraControls = new GameControls();
        //cameraControls.Camera.SetCallbacks(this);
    }

    void LateUpdate()
    {
        //CameraRotation();        
        RayCasting();
    }

    public void SetManagers(GameManager newGameManager, ObjectManager newObjectManager)
    {
        gameManager = newGameManager;
        objectManager = newObjectManager;
    }

    void RayCasting()
    {       

        RaycastHit hit;        

        Ray soundRay = new Ray(transform.position, transform.forward * soundRayLenght);
        Debug.DrawLine(transform.position, transform.position + transform.forward * soundRayLenght, Color.red);

        if (Physics.Raycast(soundRay, out hit, soundRayLenght,layerMask))
        {
            Transform objectHit = SelectObject(hit.transform);
            if (objectHit != null)
            {
                targetInterface.text = objectHit.name;
                point.color = new Color(0.34f, 0.68f, 0.87f, 0.6f);

                SetSoundWhenTargeted(objectHit);

                
                lastSelectedTargetId = objectHit.GetComponent<InteractebleObject>().GetId();              

               
            }            

        }
        else
        {
            targetInterface.text = "Нет цели";
            point.color = Color.white;
            SetBasicSoundDistance(previousHeardTarget);
        }

        Touching();        

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

        /*
        if (hitLayer <= gameManager.CurrentLayer) //Старый метод, работает с уровнями, а не с объектами
        {
            return null;
        }
        if (hitLayer>gameManager.CurrentLayer+1)
        {
            return SelectObject(hitInteracteble.GetParent());
        }*/

        return null;
    }

    private void Touching()
    {
        Ray touchRay = new Ray(transform.position, transform.forward * touchRayLenght);
        Debug.DrawLine(transform.position, transform.position + transform.forward * touchRayLenght, Color.green);

        RaycastHit hit;

        if (Physics.Raycast(touchRay, out hit, touchRayLenght, layerMask))
        {
            Transform objectHit = hit.transform;
            if (touchSound)
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

    private void SetBasicSoundDistance(AudioSource target)
    {
        if (target != null)
        {
            target.maxDistance = audioData.baseSoundDistance;
            previousHeardTarget = null;
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

    private void SetSoundWhenTargeted(Transform objectHit)
    {
        AudioSource currentTarget = objectHit.GetComponent<AudioSource>();
        if (currentTarget != null)
        {
            if (previousHeardTarget != null && currentTarget != previousHeardTarget)
            {
                previousHeardTarget.maxDistance = audioData.baseSoundDistance;
            }

            currentTarget.maxDistance = soundRayLenght + 1;
            previousHeardTarget = currentTarget;
        }
        else
        {
            SetBasicSoundDistance(previousHeardTarget);
        }
    }

    public void OnCameraRotation(InputAction.CallbackContext context)
    {
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
        transform.position = firstPersonCameraPosition.position;
    }

    public void OnSelectObject(InputAction.CallbackContext context)
    {        
        gameManager.CurrentTarget = lastSelectedTargetId;
    }

    public void OnLayerUp(InputAction.CallbackContext context)
    {
        Transform currentTarget = objectManager.FindTranformById(gameManager.CurrentTarget);
        if (currentTarget.parent != null)
        {
            gameManager.CurrentTarget = currentTarget.parent.GetComponent<InteractebleObject>().GetId();
        }
    }
    /*
    private void OnEnable()
    {
        cameraControls.Enable();
    }

    private void OnDisable()
    {
        cameraControls.Disable();
    }
    */    


    /*
    private void CameraRotation()
    {
        
        //yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        //pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;                          
        transform.position = firstPersonCameraPosition.position;
        
    }*/



}
