using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnAttackEnter : MonoBehaviour
{
    public event EventHandler<Collider2D> OnSpinningAttackEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RatBehaviour ratBehaviour = GetComponentInParent<RatBehaviour>();
        if (ratBehaviour.CanCollide(collision.gameObject))
        {
            OnSpinningAttackEnter?.Invoke(this, collision);
        }
    }
}
