using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoriallaBehaviour : EnemyBehaviourV2
{
    [Space]
    [SerializeField] private Rigidbody2D gorillaBody;
    [SerializeField] private float attackJumpVelocity;
    [SerializeField] private float animationTimeOfAttack;

    private bool targetIsLeft;
    private bool startedAttack;
    float y;

    private void FixedUpdate()
    {
        print(y + " y");
        print(transform.position.y + " pos");
        print(IsEnemyAttackConnected());
        targetIsLeft = target.transform.position.x < transform.position.x;
        if (transform.position.y < y && startedAttack)
        {
            gorillaBody = gorillaBody.GetComponent<Rigidbody2D>();
            if (targetIsLeft)
            {
                transform.Translate(Vector2.left * Time.fixedDeltaTime);
            }
            else
            {
                transform.Translate(Vector2.right * Time.fixedDeltaTime);
            }

            gorillaBody.gravityScale = 2f;
            animator.SetBool("doDownJumpAttack", true);
        }
        if (IsEnemyAttackConnected() && !startedAttack)
        {
            StartCoroutine(SetSec());
            animator.Play("JumpAttack_gorilla");
            animator.SetTrigger("doAttack");
            gorillaBody = gorillaBody.GetComponent<Rigidbody2D>();
            gorillaBody.velocity = Vector2.up * attackJumpVelocity;

            StartCoroutine(WaitForNewAttack());
        }

        //if (IsEnemyGrounded() && startedAttack && animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttackDown_gorilla"));
        //{
        //      StopattackPerfom();
        //}

        y = transform.position.y;
    }

    private void StopattackPerfom()
    {
        StopCoroutine(WaitForNewAttack());
        gorillaBody.gravityScale = 1f;

        animator.SetBool("doDownJumpAttack", false);
        gorillaBody.velocity = Vector2.zero;
    }

    private IEnumerator WaitForNewAttack()
    {
        startedAttack = true;
        yield return new WaitForSeconds(animationTimeOfAttack);
        StopattackPerfom();   
        startedAttack = false;
    }

    private IEnumerator SetSec()
    {
        yield return new WaitForSeconds(1);
    }
}
