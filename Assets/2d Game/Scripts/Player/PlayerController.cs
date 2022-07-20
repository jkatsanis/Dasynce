using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    #region Serialized private variables
    [SerializeField] private float _slideDownWallSpeed;
    [SerializeField] private GameObject _healthBarScaleChanger;
    [SerializeField] private GameObject _wall;
    [Space]
    [SerializeField] private float _start_x = -7.7F;
    [SerializeField] private float _start_y = -0.3808011F;
    [Space]
    [SerializeField] private LayerMask _platformsLayerMask;
    [SerializeField] private float _speed = 4.5F;
    #endregion

    #region Serialized public variables
    [SerializeField] public int slideDownOfWallCooldown;     //Setting the cooldown in the trigger class
    [SerializeField] public new Rigidbody2D rigidbody2D;     //To add force while sliding in PlayerMovement
    [SerializeField] public float jumpVelocity = 15F;        //To add JumpVelocity, to jump higher after a slide
    [SerializeField] public float slideSpeed = 500F;         //To combine the force with the speed to slide
    #endregion

    #region HideInInspector public variables
    [HideInInspector] public bool setWallPosDown;            //To disable bool which constantly sets the position down when slideing down of wall used in trigger ig
    [HideInInspector] public bool PositionToMove;            //To set the Position Where the Player Should Move in the Move method in PlayerMovement
    [HideInInspector] public bool onWall;                    //To set a bool if the player is on wall in the "Trigger" class
    [HideInInspector] public bool isMoving = false;          //To set Moving to true in the Move method in "PlayerMovement"
    [HideInInspector] public bool isJumping = false;         //To set Jumpinh to true in the inputSpace method in "PlayerMovement"
    [HideInInspector] public bool isSliding = false;         //To set Moving to true in the DoSlide method in "PlayerMovement"
    [HideInInspector] public float transformPosY;            //When the player is sliding he gets a transform position y when hes colliding with the wall. I safe that and after this i make this pos -0.01F to like "slideDown" the wall in "Trigger" class
    [HideInInspector] public float isLookingLeft;            //To give data to the PlayerMovement class if the player looks left or not
    [HideInInspector] public bool isWallLeft;                //To give data to the PlayerMovement class if the wall is left to the player or not
    [HideInInspector] public bool gotKeyAOrDPressed;         //also only used in PlayerMovement
    [HideInInspector] public bool enableSlideCooldown;       //only used in PlayerMovement - To check when to input slide
    [HideInInspector] public float turnScale;                //To set the turn scale in the PlayerMovement class
    [HideInInspector] public bool setSmallDelayAterWallConnection;  //Jut to set a small delay when whall connection used in input method

    #endregion

    #region Private variables
    private PauseMenuInputs _pauseMenuInputs;        //The 1 variable from PauseMenuInputs, to not do smth if im for example in da pause menu
    private PlayerController _playerController;
    private BoxCollider2D _boxCollider2d;
    private Animator _anim;
    private int _frameCnt;
    private bool _temp;
    private bool _setMomentum;
    private int _smallWallDelayAfterConnectCnt;
    #endregion

    #region Inventory
    [SerializeField] private InstantiateInventory _inventoryInstance;
    public Inventory inventory;
    public ItemSprites itemSprites;
    private GameObject _inv;
    #endregion

    void Start()
    {
        IntializeStart();
    }

    #region Intalize Start
    private void IntializeStart()
    {
        InstantiateInventory.GetInventory(out _inv, _inventoryInstance, out inventory, out itemSprites);
        _pauseMenuInputs = GetComponent<PauseMenuInputs>();
        _playerController = GetComponent<PlayerController>();
        transform.position = new Vector3(_start_x, _start_y, 0);
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        _boxCollider2d = transform.GetComponent<BoxCollider2D>();
        _anim = GetComponent<Animator>();
    }
    #endregion

    void Update()
    {
        if (!_pauseMenuInputs.aMenuEnabled)
        {
            DoWallSetup();
            DoMovemMentSetup();
            if (Input.GetKeyDown(KeyCode.L))
            {
                Item item = new Item(Item.ItemType.ManaPotion, itemSprites.manaPotion);
                inventory.AddItem(item);
            }
        }
    }
    private void FixedUpdate()
    {   
        if (!_pauseMenuInputs.aMenuEnabled)
        {
            DoSlideSetuo();
            WaitForMove();
            SetWallShortJump();
            SetMomentumAfterSlide();
            if (setWallPosDown)
            {
                transformPosY = transformPosY - _slideDownWallSpeed;
            }
            if (isJumping)
            {
                rigidbody2D.velocity = Vector2.up * jumpVelocity;
                isJumping = false;
                jumpVelocity = 11;
            }
        }
    }

    #region Sets
    private void SetWallShortJump()
    {
        if (setSmallDelayAterWallConnection)
        {
            _smallWallDelayAfterConnectCnt++;
        }
        if (_smallWallDelayAfterConnectCnt == 5)
        {
            setSmallDelayAterWallConnection = false;
            _smallWallDelayAfterConnectCnt = 0;
        }
    }
    public void SetWall()           //Just to use this Method in PlayerMovement
    {
        onWall = false;
        _anim.SetBool("onWall", false);
        rigidbody2D.gravityScale = 3F;

    }
    private void SetMomentumAfterSlide()
    {
        if (_setMomentum)
        {
            _speed = _speed - 0.01F;
        }
        if (_speed <= 4.5F)
        {
            _speed = 4.5F;
            _setMomentum = false;
        }
    }
    #endregion

    #region Setups
    private void WaitForMove()
    {
        if (isMoving)
        {
            _temp = true;
            _frameCnt++;
        }
        if (_frameCnt == 10)
        {
            _anim.SetBool("isRunning", false);
            isMoving = false;
            _frameCnt = 0;
        }
    }
    private void DoWallSetup()
    {
        if (_wall.transform.position.x < 2)
        {
            isWallLeft = true;
        }
        if (onWall)
        {
            rigidbody2D.velocity = new Vector2(0, 0);
            transform.position = new Vector2(transform.position.x, transformPosY);
        }
        if (!onWall)
        {
            setWallPosDown = false;
        }
        isLookingLeft = transform.rotation.y;
    }

    private void DoMovemMentSetup()
    {
        PlayerMovement movement = new PlayerMovement(_anim, _playerController, _healthBarScaleChanger, _speed, _wall);
        movement.SetPressAbleFields();
        movement.CheckInputForMove();
        FinalMove(movement);
        SetJumpingTrigger();
    }

    private void DoSlideSetuo()
    {
        if (isSliding && !gotKeyAOrDPressed)
        {
            _anim.SetBool("isSliding", false);
        }
        if (isSliding)
        {
            StartCoroutine(SetSlide());
        }
        if (enableSlideCooldown)
        {
            StartCoroutine(SetSlideCooldown());
        }

    }
    #endregion     

    #region Move
    private void FinalMove(PlayerMovement movement)
    {
        if (_temp)
        {
            var move = movement.Move(turnScale, PositionToMove);
            _temp = false;
        }
    }
    private void SetJumpingTrigger()
    {
        if (IsGrounded())
        {
            _anim.SetBool("isJumping", false);
        }
        else
        {
            _anim.SetBool("isJumping", true);
        }
    }

    public bool IsGrounded()
    {
        //Keep in mind the layer "platforms"
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(_boxCollider2d.bounds.center, _boxCollider2d.bounds.size, 0f, Vector2.down, .1f, _platformsLayerMask);
        return raycastHit2d.collider != null;
    }
    #endregion

    #region IEnemerator
    private IEnumerator SetSlide()
    {
        yield return new WaitForSeconds(0.6F);
        enableSlideCooldown = true;
        _anim.SetTrigger("doIdleAnimation");
        _anim.SetBool("isSliding", false);
        isSliding = false;
        _speed = 5.5F;
        _setMomentum = true;

    }
    private IEnumerator SetSlideCooldown()
    {
        yield return new WaitForSeconds(0.4F);
        enableSlideCooldown = false;
    }

    #endregion

}
