using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    private Transform _playerTransform;
    private Animator _animator;   
    private bool _isPlayerLeft;
    private void Start()
    {
        BushData bushData = GetComponentInParent<BushData>();

        _playerTransform = bushData.playerTransform;
        _animator = GetComponent<Animator>();
        _animator.SetBool("isTriggered", false);
    }
    private void Update()
    {
        _isPlayerLeft = transform.position.x > _playerTransform.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _animator.SetBool("isPlayerLeft", _isPlayerLeft);
        _animator.SetBool("isTriggered", true);

    }
}
