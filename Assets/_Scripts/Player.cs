using System;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour, IKitchenObjectParent
{
    //public static Player Instance {get; private set;}

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged; 
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }
    public event EventHandler OnPlayerPick;
    
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private LayerMask counterLayerMask;

    private float playerRadius = 0.7f;
    private float playerHeight = 5f;
    
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;

    [SerializeField] private Transform kitchenObjectHoldPoint;

    private KitchenObject kitchenObject;
    
    private bool isWalking;

    private void Awake() {
        //Instance = this;
    }

    private void Start() {
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
        GameInput.Instance.OnAltInteractAction += GameInput_OnAltInteractAction;
    }

    private void Update()
    {
        if(!IsOwner) {
            return;
        }

        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleMovement() {
        Vector2 movementVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(movementVector.x, 0, movementVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        
        if(!canMove) {
            //attemp moving to x axis
            Vector3 moveDirX = new Vector3(movementVector.x, 0,0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            
            if(canMove) {
                moveDir = moveDirX;
            } else {
                Vector3 moveDirZ = new Vector3(0,0,movementVector.y).normalized;
                canMove = moveDir.y != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                
                if(canMove) {
                    moveDir = moveDirZ;
                }
            }

        }
        if(canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);
    }

    private void HandleInteractions() {
        Vector2 movementVector = GameInput.Instance.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(movementVector.x, 0, movementVector.y);
        if(moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }
        Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactionRange, counterLayerMask);

        if(hit.collider) {
            if(hit.collider.TryGetComponent(out BaseCounter counter)) {
                if(counter != selectedCounter) {
                    SetSelectedCounter(counter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if(!GameManager.Instance.IsGamePlaying()) return;

        if(selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnAltInteractAction(object sender, EventArgs e)
    {
        if(!GameManager.Instance.IsGamePlaying()) return;

        if(selectedCounter != null) {
            selectedCounter.AltInteract(this);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs{
           selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if(kitchenObject != null) {
            OnPlayerPick?.Invoke(this, EventArgs.Empty);
        } else {
            
        }
    }

    public KitchenObject GetKitchenObject() {
        return this.kitchenObject;
    }

    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
