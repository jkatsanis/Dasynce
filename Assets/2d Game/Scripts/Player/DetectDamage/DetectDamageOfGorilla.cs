using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDamageOfGorilla : MonoBehaviour, IEnemyAttack
{
    [SerializeField] private Animator animatorPlayer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private Animator gorillaAnim;
    [Space]
    [SerializeField] private int gorillaKnockbackAmount;
    [SerializeField] private int gorillaDamageAmount;
    [SerializeField, Range(0f, 1f)] private float waitForNewAttack;
    [SerializeField] private GameObject p;

    DetectDamageOfEnemys d;
    bool cantAttack;

    private void Start()
    {
        d = p.GetComponent<DetectDamageOfEnemys>();
        animatorPlayer = GetComponentInParent<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyDownAttackGorilla") && !cantAttack)
        {
            gorillaAnim.SetTrigger("doActualAttack");
            StartCoroutine(WaitForNewAttack());
            TakeDamage(gorillaDamageAmount);
            TakeKnockback(gorillaKnockbackAmount, collision.gameObject);
        }
    }

   
    public IEnumerator WaitForNewAttack()
    {
        cantAttack = true;
        yield return new WaitForSeconds(waitForNewAttack);
        cantAttack = false;
    }

    public void TakeDamage(int dmgAmount)
    {
        d.currentHealth -= dmgAmount;
        d.healthBar.SetHealth(d.currentHealth);
    }

    public void TakeKnockback(float knockBackAmount, GameObject coll)
    {
        animatorPlayer.SetTrigger("doHurtAnim");
        if (transform.position.x > coll.transform.position.x)
        {
            rb.AddForce(new Vector2(3, 1) * knockBackAmount);
        }
        else
        {
            rb.AddForce(new Vector2(-3, 1) * knockBackAmount);
        }
    }
}
