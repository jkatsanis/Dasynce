using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLevelSelector : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private LayerMask _solidObjectLayer;

    private bool _isMoving;
    private Vector2 _input;

    public Vector3 targetPos;
    private Animator _animator;

    [SerializeField] private InstantiateInventory _inventoryInstance;
    [HideInInspector] public GameObject inv;
    private Inventory inventory;
    private ItemSprites _itemSprites;

    private Item manaPotion;


    private void Start()
    {
        InstantiateInventory.GetInventory(out inv, _inventoryInstance, out inventory, out _itemSprites);

        manaPotion = new Item(Item.ItemType.ManaPotion, _itemSprites.manaPotion);

        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (!_isMoving)
        {
            _input.x = Input.GetAxisRaw("Horizontal");
            _input.y = Input.GetAxisRaw("Vertical");

            if (_input.x != 0) _input.y = 0;
                
            if (_input != Vector2.zero)
            {
                _animator.SetFloat("moveX", _input.x);
                _animator.SetFloat("moveY", _input.y);
                targetPos = transform.position;
                targetPos.x += _input.x;
                targetPos.y += _input.y;

             
               if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }


       _animator.SetBool("isMoving", _isMoving);
    }

    private IEnumerator Move(Vector3 targetPos)
    {
        _isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * _moveSpeed);
            yield return null;
        }
        transform.position = targetPos;
        _isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, _solidObjectLayer) != null)
        {
            return false;
        }
        return true;
    }
}
