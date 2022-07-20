using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraBehaviour : EnemyBehaviourV2
{
    [SerializeField] private GameObject _fireprojectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Animator _anim;
    private bool _setAttack;

    public int damageAmount { get; private set; }
    public bool hydraIsLeft { get; private set; }

    private new void Update()
    {
        base.Update();

        if (IsEnemyAttackConnected() && !_setAttack)
        {
            _anim.SetTrigger("doAttack");
            _setAttack = true;
            StartCoroutine(WaitForeNewAttack());
            StartCoroutine(SpawnProjectile());
        }

        if (transform.localScale == new Vector3(4, 4, 0))
        {
            hydraIsLeft = true;
        }
        else
        {
            hydraIsLeft = false;
        }
    }

    private IEnumerator SpawnProjectile()
    {
        yield return new WaitForSeconds(0.4F);
        Instantiate(_fireprojectilePrefab, _firePoint.position, _firePoint.rotation);
    }
    private IEnumerator WaitForeNewAttack()
    {
        yield return new WaitForSeconds(3.5F);
        _setAttack = false;
    }
}
