using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnims : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator animator;
    private Player player;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(player.IsWalking()) {
            animator.SetBool(IS_WALKING, true);
        } else {
            animator.SetBool(IS_WALKING, false);
        }
    }
}
