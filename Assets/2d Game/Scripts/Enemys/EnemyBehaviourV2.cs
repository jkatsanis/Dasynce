using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviourV2 : MonoBehaviour
{
    #region SerializeField private Variables
    [SerializeField] private bool _useAttackAnimations;
    [SerializeField] private string _attackStateName;
    [SerializeField] private string attackName;
    [Space]
    [SerializeField] private bool _shouldGo1YDownOnRaycast;
    [Space]
    [SerializeField] private float _healthbarScale;
    [SerializeField] private GameObject _halthBarScaleChanger;
    [Space]
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private LayerMask _actionLayer;
    [Space]
    [SerializeField] private bool _spriteIsRenderedLeft;
    [SerializeField] private float _scaleOfEnemy;
    [Space]
    [SerializeField] private float _enemyAttakRaySize;
    [SerializeField] private GameObject _castPoint;
    [Space]
    [SerializeField] private float _agroRange;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _linesToDraw;
    [Space]
    [SerializeField] private BoxCollider2D _colliderOfEnemy;
    [SerializeField] private LayerMask _platformLayerMask;
    [Space]
    [SerializeField] protected float _knockbackAmount;
    [SerializeField] protected float _damageAmount;
    #endregion

    #region Public Variables
    [SerializeField] public Transform target;
    [SerializeField] public Animator animator;
    #endregion

    bool raycastingDown;
    private void Start()
    {
        animator = GetComponent<Animator>();
        _colliderOfEnemy = GetComponent<BoxCollider2D>();
        if (_shouldGo1YDownOnRaycast)
        {
            raycastingDown = true;
        }
    }
    protected void Update()
    {
        SetHealthBarRotation();
        if (CanSeePlayer() && !IsEnemyAttackConnected())
        {
            ChasePlayer();
        }
        else
        {
            StopChasingPlayer();
        }   
    }

    protected void SetHealthBarRotation()
    {
        if (!_spriteIsRenderedLeft)
        {
            _halthBarScaleChanger.transform.localScale = new Vector2((target.transform.position.x > transform.position.x)
                ? _healthbarScale
                : -_healthbarScale, _halthBarScaleChanger.transform.localScale.y);  //i jsut change the scale when da local scal is under 0 lol
            return;
        }
        _halthBarScaleChanger.transform.localScale = new Vector2((target.transform.position.x > transform.position.x) 
            ? -_healthbarScale
            : _healthbarScale, _halthBarScaleChanger.transform.localScale.y);  //i jsut change the scale when da local scal is under 0 lol
    }
    protected bool CanSeePlayer()
    {
        if(IsEnemyAttackConnected()) return false;
       
        return IsRayCastConnected(_agroRange, _linesToDraw, raycastingDown);        
    }

    private bool IsRayCastConnected(float agroRange, int linesToDraw, bool rayCastingDown)
    {
        Vector2 endPosition;
        if (raycastingDown)
        {
            _shouldGo1YDownOnRaycast = true;
        }
        for (int i = 0; i < linesToDraw; i++)
        {
            bool agroRangeChanged = false;
            int x = 2;
            if (_shouldGo1YDownOnRaycast)
            {
                agroRangeChanged = true;
                x = i;
                i = -1;
            }
            if (i >= 4)
            {
                agroRange--;
            }
            if (target.transform.position.x > transform.position.x)
            {
                endPosition = new Vector2(_castPoint.transform.position.x + agroRange, _castPoint.transform.position.y + i);
                transform.localScale = new Vector2((!_spriteIsRenderedLeft) ? _scaleOfEnemy : -_scaleOfEnemy, _scaleOfEnemy);
            }
            else
            {
                endPosition = new Vector2(_castPoint.transform.position.x - agroRange, _castPoint.transform.position.y + i);
                transform.localScale = new Vector2((!_spriteIsRenderedLeft) ? -_scaleOfEnemy : _scaleOfEnemy, _scaleOfEnemy);
            }
            RaycastHit2D hit = Physics2D.Linecast(_castPoint.transform.position, endPosition);

            Debug.DrawLine(_castPoint.transform.position, endPosition, Color.blue);

            if (agroRangeChanged)
            {
                i = x;
                i--;
                _shouldGo1YDownOnRaycast = false;
            }
            if (hit.collider != null)
            {
                if (CanCollide(hit))
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool CanCollide(RaycastHit2D hit)
    {
        List<bool> canCollide = new List<bool>();

        canCollide.Add(hit.collider.gameObject.CompareTag("Player"));
        canCollide.Add(hit.collider.gameObject.CompareTag("Stick"));
        canCollide.Add(hit.collider.gameObject.CompareTag("ScientistHead"));
        
        foreach (bool b in canCollide)
        {
            if (b)
            {
                return true;
            }
        }

        return false;
    }

    public bool CanCollide(GameObject hit)
    {
        List<bool> canCollide = new List<bool>();

        canCollide.Add(hit.CompareTag("Player"));
        canCollide.Add(hit.CompareTag("Stick"));
        canCollide.Add(hit.CompareTag("ScientistHead"));

        foreach (bool b in canCollide)
        {
            if (b)
            {
                return true;
            }
        }

        return false;
    }
    protected bool IsEnemyAttackConnected()   //the collider before the enemy
    {
        return IsRayCastConnected(_enemyAttakRaySize, 1, false);
    }
    protected void ChasePlayer()
    {
        if (_useAttackAnimations)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(_attackStateName))
            {
                animator.SetBool(attackName, false);
                return;
            }
            animator.SetBool(attackName, false);
        }
        ChaseIntoDirection();
        animator.SetBool("canWalk", true);
    }

    protected void ChaseIntoDirection()
    {
        if (transform.position.x < target.position.x)
        {
            transform.Translate(Vector3.left * -_moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
        }
    }
    protected void StopChasingPlayer()
    {
        transform.Translate(Vector3.left * 0);
        animator.SetBool("canWalk", false);
    }

    protected bool IsEnemyGrounded()
    {
        //Keep in mind the layer "platforms"
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(_colliderOfEnemy.bounds.center, _colliderOfEnemy.bounds.size, 0f, Vector2.down, .1f, _platformLayerMask);
        return raycastHit2d.collider != null;
    }
}
