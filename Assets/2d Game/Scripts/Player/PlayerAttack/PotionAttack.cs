using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class PotionAttack
{
    #region Constructor Variables
    public bool potionAttack { get; set; }
    public float throwSpeed { get; set; }
    public bool potionAttacking { get; set; }
    public GameObject potion { get; set; }
    public GameObject prefabPotion { get; set; }
    #endregion

    #region Private Variables#
    private int cnt = 0;     
    private Animator potionAnim;
    private Rigidbody2D potionRB;
    private UITimer uiTimer;
    private PlayerController controller;
    private TMP_Text potionTimer;
    private Transform transformPos;
    private PllayerAttack playerAttacker;
    public bool setCnt;
    public event EventHandler<GameObject> OnPotionDestroy;
    public event EventHandler<Rigidbody2D> OnPotionRBDestroy;
    #endregion

    public PotionAttack(UITimer _timer, GameObject _prefabPotion, PlayerController _controller, TMP_Text _potionTimer, Transform _pos,
        GameObject _potion, float _throwSpeed, PllayerAttack _playerAttacker)
    {
        playerAttacker = _playerAttacker;
        potion = _potion;
        transformPos = _pos;
        uiTimer = _timer;
        prefabPotion = _prefabPotion;
        controller = _controller;
        potionTimer = _potionTimer;
        throwSpeed = _throwSpeed;

    }
    public void RunPotionAttackSetup()
    {
        if (!setCnt && !playerAttacker.canRunGrounded)
        {
            cnt = 0;
            InputPotionAttack();
        }
        if (potionAttack && prefabPotion.transform.position.y < -6F)
        {
            OnPotionDestroy.Invoke(this, prefabPotion);
            cnt = 0;
            setCnt = false;
            potionAttack = false;
            playerAttacker.canRunGrounded = false;
        }
     
    }
  
    public void SetPotion()
    {
        if (playerAttacker.canRunGrounded)
        {
            if (playerAttacker.IsPotionGrounded())
            {
                potionAnim = prefabPotion.GetComponent<Animator>();
                potionRB = prefabPotion.GetComponent<Rigidbody2D>();
                potionAnim.SetTrigger("destruct");
                playerAttacker.canRunGrounded = false;
                setCnt = true;
                potionAttack = false;

                OnPotionRBDestroy.Invoke(this, potionRB);
            }
        }
        if (setCnt)
        {
            cnt++;
        }
        if (cnt == 100)
        {
            cnt = 0;
            setCnt = false;
            OnPotionDestroy.Invoke(this, prefabPotion);
        }

    }

    private void DoPotionAttack()
    {
        potionAttack = true;
        uiTimer.timerStarted = true;
        playerAttacker.InstantiatePotion(transformPos);
        potionRB = prefabPotion.GetComponent<Rigidbody2D>();
        potionAnim = prefabPotion.GetComponent<Animator>();

        if (controller.isLookingLeft == 0)
        {
            potionRB.AddForce(new Vector2(1.0F, 1.2F) * throwSpeed);
            potionRB.AddTorque(-360);
        }
        else
        {
            potionRB.AddTorque(360);
            potionRB.AddForce(new Vector2(-1.0F, 1.2F) * throwSpeed);
        }
    }
    private void InputPotionAttack()
    {
        if (Input.GetKeyDown(KeyCode.E) && potionTimer.text == uiTimer.startTime)
        {
            if (controller.inventory.IsSliderOnItem(Item.ItemType.ManaPotion))
            {
                controller.inventory.RemoveItem();
            }
            else
            {
                controller.inventory.RemoveItem(GetManaPotion());
            }
            DoPotionAttack();
        }
    }

    private Item GetManaPotion()
    {
        Item original;
        if (controller.inventory.ExistItem(Item.ItemType.ManaPotion, out original))
        {
            return original;
        }
        return original;
    }
}
