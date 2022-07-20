
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class DownAttack
{
    public bool IsDownAttacking { get; set; }

    private KeyCode _inputKey;
    private PlayerController _controller;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    public DownAttack(KeyCode _inputKey, PlayerController _controller, Animator anim, Rigidbody2D rb)
    {
        this._inputKey = _inputKey;
        this._controller = _controller;
        _animator = anim;
        _rigidbody2D = rb;
    }
    public void CheckInputForDownAttack(float gravityScaleWhenDown, float gravityNormal)
    {
        if (Input.GetKeyDown(_inputKey))
        {
            IsDownAttacking = true;
            _controller.SetWall();
            _animator.SetBool("isAttackingDown", true);
            _rigidbody2D.gravityScale = gravityScaleWhenDown;
        }
        if (Input.GetKeyUp(_inputKey) || _controller.IsGrounded())
        {
            IsDownAttacking = false;
            _rigidbody2D.gravityScale = gravityNormal;
            _animator.SetBool("isAttackingDown", false);
        }
    }

}