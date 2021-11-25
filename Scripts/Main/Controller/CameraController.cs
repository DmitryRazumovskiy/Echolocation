using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;

    [Header("Настройка положения камеры")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform firstPersonCameraPosition;

    [Header("Настройка чувствительности")]
    [SerializeField] private Vector2 mouseSensitivity;

    [Header("Ограничение поворота камеры по оси Y")]
    [SerializeField] private Vector2 pitchMinMax;

    [Header("Настройка интерфейса")]
    [SerializeField] private bool lockCursor;      

    private float yaw, pitch;
    public Vector2 MousePosition
    {
        get
        {
            return new Vector2(yaw, pitch);
        }
        set
        {
            yaw = value.x * mouseSensitivity.x;
            pitch -= value.y * mouseSensitivity.y;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        }
    }

    private void Awake()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        playerCamera.transform.position = firstPersonCameraPosition.position;
    }

    private void FixedUpdate()
    {        
        player.Rotate(Vector3.up, yaw);       

        Vector3 currentRotation = playerCamera.transform.eulerAngles;
        currentRotation.x = pitch;
        playerCamera.transform.eulerAngles = currentRotation;
        playerCamera.transform.position = firstPersonCameraPosition.position;
    }

}


