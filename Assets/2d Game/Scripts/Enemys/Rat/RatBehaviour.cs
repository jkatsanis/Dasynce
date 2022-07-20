using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RatBehaviour : EnemyBehaviourV2
{
    [SerializeField] private BoxCollider2D attackEnter;
    private readonly int _attackCooldown = 3;
    private bool _canAttack = true;
    private OnAttackEnter _onAttackEnter;
    private DetectDamageOfEnemys _detectDamageOfEnemys;
    private void Start()
    {
        _detectDamageOfEnemys = target.gameObject.GetComponent<DetectDamageOfEnemys>();
        _onAttackEnter = attackEnter.GetComponent<OnAttackEnter>();
        _onAttackEnter.OnSpinningAttackEnter += OnAttackEnter_OnSpinningAttackEnter;
    }

    private new void Update()
    {
        base.Update();
        if (IsEnemyAttackConnected() && _canAttack)
        {
            animator.SetBool("canWalk", false);
            animator.SetBool("attack", true);
            //StartCoroutine(AttackCooldown());
            _canAttack = false;
        }
        if (!IsEnemyAttackConnected())
        {
            animator.SetBool("attack", false);
            _canAttack = true;
        }
        if (!_canAttack)
        {
            ChaseIntoDirection();
        }
    }

    private void OnAttackEnter_OnSpinningAttackEnter(object sender, Collider2D e)
    {
        _detectDamageOfEnemys.TakeDamage(Convert.ToInt32(_damageAmount));
        _detectDamageOfEnemys.TakeKnockback(Convert.ToInt32(_knockbackAmount), e.gameObject);
    }
    private IEnumerator AttackCooldown()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
        animator.SetBool("canWalk", true);
        animator.SetBool("attack", false);
    }
}
