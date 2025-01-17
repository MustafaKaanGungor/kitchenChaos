using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameInput gameInput;
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float turnSpeed = 10;
    [SerializeField] private float interactionRange = 2f;
    private float playerRadius = 0.7f;
    private float playerHeight = 5f;
    private Vector3 lastInteractDir;
    private bool isWalking;



    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleMovement() {
        Vector2 movementVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(movementVector.x, 0, movementVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        
        if(!canMove) {
            //attemp moving to x axis
            Vector3 moveDirX = new Vector3(movementVector.x, 0,0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            
            if(canMove) {
                moveDir = moveDirX;
            } else {
                Vector3 moveDirZ = new Vector3(0,0,movementVector.y).normalized;
                canMove =! Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                
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
        Vector2 movementVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(movementVector.x, 0, movementVector.y);
        if(moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }
        Physics.Raycast(transform.position, lastInteractDir, out RaycastHit hit, interactionRange);

        if(hit.collider) {
            Debug.Log(hit.transform);
        }
    }
}
