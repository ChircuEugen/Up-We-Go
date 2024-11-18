using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 4.0f;
    private float gravityValue = -9.81f;

    private Animator animator;

    public Joystick joystick;

    public float knockBackForce;
    public float knockBackDuration;
    private float knockBackTimer;
    private Vector3 knockBackDirection;

    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0;
            animator.SetBool("isJumping", false);
        }
        else if (!groundedPlayer)
        {
            animator.SetBool("isJumping", true);
        }

        Vector3 move = new Vector3(joystick.Horizontal, 0, 0);
        if (knockBackTimer <= 0)
        {
            animator.SetFloat("speed", Mathf.Abs(joystick.Horizontal));
            controller.Move(playerSpeed * Time.deltaTime * move);
        }
        else
        {
            knockBackTimer -= Time.deltaTime;
            controller.Move(playerSpeed * Time.deltaTime * knockBackDirection);
        }

        if (move != Vector3.zero)
        {
            transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void OnJump()
    {
        if (groundedPlayer)
        {
            animator.SetBool("isJumping", true);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("EnemyHead"))
        {
            Vector3 slideDirection = (transform.position - other.transform.position).normalized;
            Vector3 slideMovement = slideDirection;
            slideMovement.y = playerVelocity.y;
            slideMovement.z = 0f;
            slideMovement = slideMovement * 4;
            //transform.position += slideMovement * Time.deltaTime;
            controller.Move(slideMovement * Time.deltaTime);
        }
    }

    public void ApplyKnockback(Vector3 direction)
    {
        knockBackTimer = knockBackDuration;
        direction *= knockBackForce;
        direction.y = 1f;
        knockBackDirection = direction;        
    }
}
