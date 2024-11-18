using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private LayerMask enemyLayer;

    public float pushForce;
    public float pushRange;

    public float timeBetweenAttack = 0.3f;
    private float timer;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }

    public void OnAttack()
    {
        if(timer >= timeBetweenAttack)
        {
            StartCoroutine(AttackStrike());
        }
    }

    IEnumerator AttackStrike()
    {
        timer = 0;
        animator.SetBool("isAttacking", true);
        Collider[] enemies = Physics.OverlapSphere(transform.position + transform.forward, pushRange, enemyLayer);
        for (int i = 0; i < enemies.Length; i++)
        {
            Rigidbody enemyToPush = enemies[i].GetComponent<Rigidbody>();
            if (enemyToPush != null)
            {
                Vector3 knockBackDirection = (enemyToPush.transform.position - transform.position).normalized;
                knockBackDirection.y = 0.8f;
                enemyToPush.AddForce(knockBackDirection * pushForce, ForceMode.Impulse);
            }

        }

        yield return new WaitForSeconds(timeBetweenAttack);
        animator.SetBool("isAttacking", false);
    }
}
