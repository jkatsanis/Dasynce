using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PllayerAttack : MonoBehaviour
{
    #region SerializeField Variables
    [SerializeField] private LayerMask _platformsLayerMask;
    [Space]
    [SerializeField] private float throwSpeed;
    [SerializeField] private UITimer uiTimer;
    [SerializeField] private TMP_Text potionTimer;
    [SerializeField] private GameObject potion;
    [SerializeField] private Rigidbody2D playerRigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator potionAnim;
    #endregion

    #region Public Variables
    public GameObject prefabPotion;             //To Destroy The potion when it collides with da enemy
    public bool canRunGrounded;                 //To set a bool cnt to true to enable in the Potion Attack class a cnt
    public PotionAttack potionAttack;           //To set bools of the potion attack class to false when it collids with an enemy
    #endregion

    #region Private Variables
    private const float GRAVITY_WHEN_DOWN_ATTACK = 4.5F;
    private const float GRAVITY_NORMAL = 3F;
    private bool attacked;
    private BoxCollider2D colliderOfPotion;
    private PlayerController controller;
    private PllayerAttack playerAttacker;
    private PauseMenuInputs pauseMenuInputs;
    #endregion

    void Start()
    {
        playerAttacker = GetComponent<PllayerAttack>();
        pauseMenuInputs = GetComponent<PauseMenuInputs>();
        controller = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();

         potionAttack = new PotionAttack(uiTimer, prefabPotion, controller, potionTimer, transform, potion, 200, playerAttacker  );
    }
    void Update()
    {
        if (!pauseMenuInputs.aMenuEnabled)
        {
            DownAttack downAttack = new DownAttack(KeyCode.S, controller, animator, playerRigidbody);
            downAttack.CheckInputForDownAttack(GRAVITY_WHEN_DOWN_ATTACK, GRAVITY_NORMAL);

            if (controller.inventory.ExistItem(Item.ItemType.ManaPotion))
            {
                PotionAttackSetup();                 
            }
            potionAttack.OnPotionDestroy += PotionAttack_OnPotionDestroy;
            potionAttack.OnPotionRBDestroy += PotionAttack_OnPotionRBDestroy;
            potionAttack.SetPotion();

            if (Input.GetMouseButtonDown(0) && !attacked)
            {
                Attack();
            }
        }   
    }

    private void PotionAttack_OnPotionRBDestroy(object sender, Rigidbody2D e)
    {
        Destroy(e);
    }

    public void InstantiatePotion(Transform transformPos)
    {
        potionAttack.prefabPotion = Instantiate(potion, new Vector2(transformPos.transform.position.x, transformPos.transform.position.y + 1), Quaternion.identity);
    }
    private void PotionAttack_OnPotionDestroy(object sender, GameObject e)
    {
        Destroy(e);
    }

    private void PotionAttackSetup()
    {
        potionAttack.RunPotionAttackSetup();

        if (potionAttack.potionAttack)
        {
            prefabPotion = potionAttack.prefabPotion;
            colliderOfPotion = prefabPotion.GetComponent<BoxCollider2D>();
            canRunGrounded = true;
        }
    }
    private void Attack()
    {
        attacked = true;
        animator.SetTrigger("isAttacking");
        StartCoroutine(WaitForNewAttack());
    }

    private IEnumerator WaitForNewAttack()
    {
        yield return new WaitForSeconds(0.25F);
        attacked = false;
    }

    public bool IsPotionGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(colliderOfPotion.bounds.center, colliderOfPotion.bounds.size, 0f, Vector2.down, .1f, _platformsLayerMask);
        return raycastHit2d.collider != null;
    }

}
