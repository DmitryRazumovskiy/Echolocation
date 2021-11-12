using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class InputManager : MonoBehaviour
{
    [SerializeField] private MovementController movementController;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private RayCastController rayController;
    [SerializeField] private ActionController actionController;

    private GameControls inputs;
    private GameControls.MovementActions movementActions;
    private GameControls.CameraActions cameraActions;
    private GameControls.ActionsActions actions;

    Vector2 horizontalInput;
    Vector2 mouseInput;

    private void Awake()
    {
        inputs = new GameControls();
        movementActions = inputs.Movement;
        cameraActions = inputs.Camera;
        actions = inputs.Actions;

        movementActions.Movement.performed  += context =>   horizontalInput = context.ReadValue<Vector2>();
        movementActions.Jump.performed      += context =>   movementController.OnJumpPressed();
        movementActions.Running.performed   += context =>   movementController.Running = context.ReadValueAsButton();

        cameraActions.AxisX.performed       += context =>   mouseInput.x = context.ReadValue<float>();
        cameraActions.AxisY.performed       += context =>   mouseInput.y = context.ReadValue<float>();

        actions.SelectObject.performed      += context =>   rayController.OnSelectObject();
        actions.SelectParent.performed      += context =>   rayController.OnSelectParent();

        actions.IncreaseLayer.performed     += context =>   actionController.ChangeLayer( 1 );
        actions.DecreaseLayer.performed     += context =>   actionController.ChangeLayer(-1 );
        actions.Blindfold.performed         += context =>   actionController.BlindScreenToggle();
    }
        

    private void Update()
    {
        movementController.Direction = horizontalInput;
        cameraController.MousePosition = mouseInput;
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
}


