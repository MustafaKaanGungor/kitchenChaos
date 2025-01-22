using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player player;
    private float footstepTimer = 0f;
    private float footstepTimerMax = 0.1f;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        footstepTimer -= Time.deltaTime;
        if(footstepTimer >= 0) {
            footstepTimer = footstepTimerMax;

            if(player.IsWalking()) {
                SoundManager.Instance.PlayFootstepSound(transform.position, 1);
            }
        }
    }
}
