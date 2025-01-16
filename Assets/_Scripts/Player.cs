using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float turnSpeed = 10;
    private bool isWalking;
    private void Update()
    {
        Vector2 movementVector = new Vector2(0,0);

        if(Input.GetKey(KeyCode.W)) {
            movementVector.y = 1;
        }
        if(Input.GetKey(KeyCode.A)) {
            movementVector.x = -1;
        }
        if(Input.GetKey(KeyCode.S)) {
            movementVector.y = -1;
        }
        if(Input.GetKey(KeyCode.D)) {
            movementVector.x = 1;
        }

        movementVector = movementVector.normalized;
        Vector3 moveDir = new Vector3(movementVector.x, 0, movementVector.y);
        
        isWalking = moveDir != Vector3.zero;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, turnSpeed * Time.deltaTime);
    }

    public bool IsWalking() {
        return isWalking;
    }
}
