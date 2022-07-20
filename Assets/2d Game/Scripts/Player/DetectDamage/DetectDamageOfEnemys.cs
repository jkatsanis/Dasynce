using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface IEnemyAttack
{
    void TakeDamage(int dmgAmount);
    void TakeKnockback(float knockbackAmount, GameObject coll);

    IEnumerator WaitForNewAttack();
}

public class DetectDamageOfEnemys : MonoBehaviour, IEnemyAttack
{
    #region SerializeField Fields
    [SerializeField] protected float waitForAttackCooldown;
    [SerializeField] public HealthBar healthBar;
    [SerializeField] private bool needRbAndAnimator;
    [Space]
    [SerializeField] private Rigidbody2D playersRb;
    [SerializeField] private GameObject DieMenu;
    [SerializeField] private int maxHealth = 100;
    #endregion

    #region Private Variables
    [HideInInspector] public int currentHealth = 100; //to set helath in classes
    private Animator animator;
    private const int MONKEY_DMG_AMOUNT = 10;
    bool tookDmg = false;
    #endregion

    private void Start()
    {
        if (needRbAndAnimator)
        {
            animator = GetComponent<Animator>();
            playersRb = GetComponent<Rigidbody2D>();
        }
        healthBar.SetMaxHealth(maxHealth);
        maxHealth = 100;
    }
    private void Update()
    {
        if (currentHealth == 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        DieMenu.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttackMonkey") && !tookDmg)
        {
            TakeDamage(MONKEY_DMG_AMOUNT);
            TakeKnockback(200, collision.gameObject);
            tookDmg = true;
            StartCoroutine(WaitForNewAttack());
        }
    }
    public void TakeDamage(int dmgAmount)
    {
        currentHealth -= dmgAmount;
        healthBar.SetHealth(currentHealth);
    }

    public IEnumerator WaitForNewAttack()
    {
        yield return new WaitForSeconds(waitForAttackCooldown);
        tookDmg = false;
    }

    public void TakeKnockback(float knockBackAmount, GameObject coll)
    {
        animator.SetTrigger("doHurtAnim");
        if (transform.position.x > coll.transform.position.x)
        {
            playersRb.AddForce(new Vector2(3, 1) * knockBackAmount);
        }
        else
        {
            playersRb.AddForce(new Vector2(-3, 1) * knockBackAmount);
        }
    }    

}
