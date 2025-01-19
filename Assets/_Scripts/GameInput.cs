using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    
    private PlayerInputActions inputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnAltInteractAction;
    
    private void Awake() {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += interact_performed;
        inputActions.Player.AltInteract.performed += altInteract_performed;
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
