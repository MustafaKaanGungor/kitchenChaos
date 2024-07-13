using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent{
    
    public static Player Instance {get; private set;}
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public ClearCounter selectedCounter;
    }
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turningSpeed;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;
    private bool isWalking;

    private Vector3 lastDirection;
    private ClearCounter selectedCounter;
    private KitchenObjects kitchenObject;

    [SerializeField] private GameInput gameInput;

    private void Awake()
    {
        if(Instance != null) {
            Debug.Log("There is more than 1 player");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += InteractAction;
    }

    private void InteractAction(object sender, EventArgs e) {
        if(selectedCounter != null) {
            selectedCounter.Interact(this);
        }    
    }
    
    private void Update() {
        HandleMovement();
        HandleInteraction();
        
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteraction() {
        float interactionDistance = 2f;
        if(Physics.Raycast(transform.position, lastDirection, out RaycastHit raycastHit, interactionDistance, counterLayerMask)) {
            if(raycastHit.transform.TryGetComponent<ClearCounter>(out ClearCounter clearCounter)) {
                if(selectedCounter != clearCounter) {
                    SetSeletedCounter(clearCounter);
                }
            } else {
                SetSeletedCounter(null);
            }
        } else {
            SetSeletedCounter(null);
        }
    }

    private void HandleMovement() {
        Vector3 playerMovement = new Vector3(gameInput.GetMovementVectorNormalized().x, 0, gameInput.GetMovementVectorNormalized().y);
        if(playerMovement != Vector3.zero) {
            lastDirection = playerMovement;
        }

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerSize = 0.7f;
        float playerHeight = 1f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerSize, playerMovement, moveDistance);
        
        if(!canMove) {
            Vector3 playerMovementX = new Vector3(gameInput.GetMovementVectorNormalized().x, 0 ,0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerSize,playerMovementX, moveDistance);

            if(canMove) {
                playerMovement = playerMovementX;
            } else {
                Vector3 playerMovementY = new Vector3(0,0,gameInput.GetMovementVectorNormalized().y);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerSize, playerMovementY, moveDistance);

                if(canMove) {
                    playerMovement = playerMovementY;
                }
            }
        }
        if(canMove) {
            gameObject.transform.position += playerMovement * Time.deltaTime * moveSpeed;
        }
        transform.forward = Vector3.Slerp(transform.forward, playerMovement, Time.deltaTime * turningSpeed);

        isWalking = playerMovement != Vector3.zero;
    }

    private void SetSeletedCounter(ClearCounter selectedCounter) {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
           selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObjects kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObjects GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        this.kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}
