using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform playerPosition;
    public float detectionHeight = 1.5f;
    public float detectionHorizontal = 4.5f;
    public float stopDistance = 3f;
    public float speed = 2f;

    private Animator animator;
    private void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float heightDistance = Mathf.Abs(playerPosition.position.y - transform.position.y);

        // if same height
        if(heightDistance <= detectionHeight)
        {
            float horizontalDistance = Vector3.Distance(transform.position, playerPosition.position);

            // if same height, in detection range, outside stop range
            if(horizontalDistance > stopDistance && horizontalDistance < detectionHorizontal)
            {
                Vector3 directionToPlayer = (playerPosition.position - transform.position).normalized;
                transform.forward = directionToPlayer;
                transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
                transform.position += speed * Time.deltaTime * new Vector3(directionToPlayer.x, 0, 0);
                animator.SetBool("isMoving", true);
                animator.SetBool("isAttacking", false);
            }
            else if (horizontalDistance > detectionHorizontal) // if on same height, but on different platform
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", false);
            }
            else // same height, distance less than stopDistance, can attack
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isAttacking", true);
                AttackPlayer();
            }
        }
    }

    public void AttackPlayer()
    {
        Vector3 knockBackDirection = (playerPosition.position - transform.position).normalized;
        knockBackDirection.z = 0f;
        PlayerMovement playerMovement = playerPosition.GetComponent<PlayerMovement>();
        if(playerMovement != null)
        {
            playerMovement.ApplyKnockback(knockBackDirection);
        }
    }
}
