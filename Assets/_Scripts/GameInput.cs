using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance{get; private set;}
    
    private PlayerInputActions inputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnAltInteractAction;
    public event EventHandler OnPauseAction;
    
    private void Awake() {
        Instance = this;
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += interact_performed;
        inputActions.Player.AltInteract.performed += altInteract_performed;
        inputActions.Player.Pause.performed += pause_performed;
    }

    private void OnDestroy() {
        inputActions.Player.Interact.performed -= interact_performed;
        inputActions.Player.AltInteract.performed -= altInteract_performed;
        inputActions.Player.Pause.performed -= pause_performed;

        inputActions.Dispose();
    }

    private void pause_performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    private void interact_performed(InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void altInteract_performed(InputAction.CallbackContext context)
    {
        OnAltInteractAction?.Invoke(this, EventArgs.Empty);
    }


    public Vector2 GetMovementVectorNormalized() {
        Vector2 movementVector = inputActions.Player.Movement.ReadValue<Vector2>();
        
        movementVector = movementVector.normalized;
        
        return movementVector;
    }
}
