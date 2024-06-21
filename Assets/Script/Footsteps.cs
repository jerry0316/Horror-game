using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public AudioSource walkSound;

    private CharacterController characterController;
    private bool isWalking = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // 检查角色是否在地面上
        bool isGrounded = characterController.isGrounded;

        // 检查角色是否在移动
        bool isMoving = characterController.velocity.magnitude > 0;

        // 检查角色是否在地面上且在移动
        if (isGrounded && isMoving)
        {
            if (!isWalking)
            {
                walkSound.Play();
                isWalking = true;
            }
        }
        else
        {
            // 如果角色不在地面上或者不在移动，停止播放脚步声音
            if (isWalking)
            {
                walkSound.Stop();
                isWalking = false;
            }
        }
    }
}
