using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectDamageEnemy : MonoBehaviour
{
    #region SerializeField Variables      
    [SerializeField] private int potionAttackDamageTimes2;
    [SerializeField] private GameObject stickAttack;
    [SerializeField] private LayerMask enemyLayerMask;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject player;
    [SerializeField] private PllayerAttack playerAttacker;
    #endregion

    #region Private Variables    
    private BoxCollider2D stickColl;
    private Animator playerAnim;
    private int currentHealth = 100;
    private bool tookDmg;
    [HideInInspector] public bool isPotionAttackConnected;
    #endregion


    void Start()
    {
        stickColl = stickAttack.GetComponent<BoxCollider2D>();
        playerAnim = player.GetComponent<Animator>();
    }

    void Update()
    {
        if (IsPlayerAttackConnected() && playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Player_Attack") && !tookDmg)
        {
            TakeDamage(10);
            tookDmg = true;
            StartCoroutine(WaitForNewAttack());
        }
        if (isPotionAttackConnected)
        {
            isPotionAttackConnected = false;
            TakeDamage(potionAttackDamageTimes2);
        }
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private IEnumerator WaitForNewAttack()
    {
        yield return new WaitForSeconds(0.5F);
        tookDmg = false;
    }
    private void TakeDamage(int dmgAmount)
    {
        currentHealth -= dmgAmount;
        healthBar.SetHealth(currentHealth);
    }

    private bool IsPlayerAttackConnected()  
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(stickColl.bounds.center, stickColl.bounds.size, 0f, Vector2.down, .1f, enemyLayerMask);
        return raycastHit2d.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Potion"))
        {
            DestroyPotion();
            isPotionAttackConnected = true;
        }
    }

    public void DestroyPotion()
    {
        Destroy(playerAttacker.prefabPotion);
        playerAttacker.potionAttack.setCnt = false;
        playerAttacker.canRunGrounded = false;
        playerAttacker.potionAttack.potionAttack = false;
    }
}
