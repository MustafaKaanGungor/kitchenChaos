using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private void Awake() {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();    
    }
    
    public Vector2 GetMovementVectorNormalized() {
        Vector2 movementVector = inputActions.Player.Movement.ReadValue<Vector2>();
        
        movementVector = movementVector.normalized;
        
        return movementVector;
    }
}
