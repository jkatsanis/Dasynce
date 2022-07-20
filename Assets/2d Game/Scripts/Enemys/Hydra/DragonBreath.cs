using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBreath : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
   
    private bool _attacked;

    void Start()
    {
        StartCoroutine(WaitForDestroy());
        FlipGameobjectAndThrowProjectile(GameObject.FindWithTag("Hydra"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_attacked)
        {
            SetDmgAndDestroyObject(collision);   
        }
    }
    private void FlipGameobjectAndThrowProjectile(GameObject ob)
    {
        HydraBehaviour hydraBehaviour = ob.GetComponent<HydraBehaviour>();

        if (hydraBehaviour.hydraIsLeft)
        {
            ThrowProjectileToPlayer(1);
        }
        else
        {
            ThrowProjectileToPlayer(-1);
        }

    }

    private void ThrowProjectileToPlayer(int scale)
    {
        Transform player = GameObject.FindWithTag("Player").transform;

        transform.localScale = new Vector3(scale, 1, 1);
        //transform.rotation = GenerateRotationBasedOnHeight(player, transform);

        _rigidbody.AddForce(player.position * _speed);
    }

    private Quaternion GenerateRotationBasedOnHeight(Transform player, Transform transform)
    {
        float posY = player.position.y;
        float rotationZ = posY / -0.018F;
        return new Quaternion(rotationZ, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }

    private void SetDmgAndDestroyObject(Collider2D collision)
    {
        _attacked = true;
        GameObject player = collision.gameObject;

        DetectDamageOfEnemys detectDmg = player.GetComponent<DetectDamageOfEnemys>();
        StartCoroutine(Wait());

        detectDmg.TakeKnockback(400, player);
        detectDmg.TakeDamage(20);
        Destroy(gameObject);
    }
    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5F);
        _attacked = false;
    }
    private IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(3);
        if (gameObject.scene.isLoaded)
        {
            Destroy(gameObject);
        }
    }
}
