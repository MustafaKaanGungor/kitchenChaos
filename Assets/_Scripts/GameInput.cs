using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    public static GameInput Instance{get; private set;}
    
    private PlayerInputActions inputActions;
    public event EventHandler OnInteractAction;
    public event EventHandler OnAltInteractAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnBindingRebind;

    public enum Binding {
        Move_Up,
        Move_Down,
        Move_Right,
        Move_Left,
        Interact,
        AltInteract,
        Pause
    }
    
    private void Awake() {
        Instance = this;

        inputActions = new PlayerInputActions();

        if(PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS)) {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
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

    public string GetBindingText(Binding binding) {
        switch (binding)
        {
            case Binding.Move_Up:
                return inputActions.Player.Movement.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return inputActions.Player.Movement.bindings[2].ToDisplayString();
            case Binding.Move_Right:
                return inputActions.Player.Movement.bindings[4].ToDisplayString();
            case Binding.Move_Left:
                return inputActions.Player.Movement.bindings[3].ToDisplayString();
            case Binding.Interact: 
                return inputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.AltInteract:
                return inputActions.Player.AltInteract.bindings[0].ToDisplayString();
            case Binding.Pause:
                return inputActions.Player.Pause.bindings[0].ToDisplayString();
            default:
            break;
        }
        return null;
    }

    public void Rebind(Binding binding, Action onActionRebound) {
        inputActions.Player.Disable();

        InputAction inputAction = null;
        int bindingIndex = 0;
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = inputActions.Player.Movement;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = inputActions.Player.Movement;
                bindingIndex = 2;
                break;
            case Binding.Move_Right:
                inputAction = inputActions.Player.Movement;
                bindingIndex = 4;
                break;
            case Binding.Move_Left:
                inputAction = inputActions.Player.Movement;
                bindingIndex = 3;
                break;
            case Binding.Interact:
                inputAction = inputActions.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.AltInteract:
                inputAction = inputActions.Player.AltInteract;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = inputActions.Player.Pause;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex).OnComplete(callback => { 
            callback.Dispose();
            inputActions.Player.Enable();
            onActionRebound();

            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, inputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            
            OnBindingRebind?.Invoke(this, EventArgs.Empty);
        })
        .Start();
    }
}
