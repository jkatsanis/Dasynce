using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyBehaviour : EnemyBehaviourV2
{
    private new void Update()
    {   
        base.Update();

        //We take knockback in the DetectDamageOfEnemys, we check for a collision and then we take knockback
        if (IsEnemyAttackConnected())
        {
            animator.SetBool("canWalk", false);
            animator.SetBool("isAttacking", true);
        }
    }
}
