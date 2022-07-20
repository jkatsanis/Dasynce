
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class PlayerMovement
{ 
    #region Constructor Variables
    public float Speed { get; set; }
    public Animator anim { get; }
    public PlayerController controller { get; }
    public GameObject healthBarScaleChanger { get; set; }
    public GameObject wall { get; }
    public Rigidbody2D rb { get;  } 
    #endregion

    #region Private Variables
    private bool[] pressAbleKey = new bool[2];
    #endregion

    public PlayerMovement(Animator _anim, PlayerController _controller, GameObject _healthBarScaleChanger, float _speed, GameObject _wall)
    {
        wall = _wall;
        healthBarScaleChanger = _healthBarScaleChanger;
        Speed = _speed;
        controller = _controller;
        anim = _anim;
    }
    public void CheckInputForMove()
    {
        InputA();
        InputD();
        InputLShift();
        InputSpace();
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            controller.gotKeyAOrDPressed = false;

        }
        if (controller.IsGrounded())
        { 
            anim.SetTrigger("doIdleAnimation");
        }
    }

    #region Inputs
    private void InputA()
    {
        if ((Input.GetKey(KeyCode.A) && !controller.onWall) || (Input.GetKey(KeyCode.A) && controller.onWall && controller.isLookingLeft != 1F))
        {
            SetRunAnimWhenSlide(0);
            if (pressAbleKey[0])
            {
                controller.SetWall();
            }
            IntializeMove(999999999F, true);
        }
    }
    private void InputD()
    {
        if ((Input.GetKey(KeyCode.D) && !controller.onWall) || (Input.GetKey(KeyCode.D) && controller.onWall && controller.isLookingLeft != 0F))
        {
            SetRunAnimWhenSlide(1);
            if (pressAbleKey[1])
            {
                controller.SetWall();
            }
            IntializeMove(0F, false);
        }
    }
    private void InputLShift()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && controller.isMoving && controller.IsGrounded() && !controller.enableSlideCooldown && !controller.isSliding)  
        {
            DoSlide();
        }
    }
    private void InputSpace()
    {
        if (controller.setSmallDelayAterWallConnection) return;

        if ((controller.IsGrounded() && Input.GetKeyDown(KeyCode.Space)) || Input.GetKeyDown(KeyCode.Space) && controller.onWall)
        {
            if (controller.isSliding)
            {
                controller.jumpVelocity = 12;
            }
            anim.SetBool("isSliding", false);
            anim.SetTrigger("takeOf");
            controller.onWall = false;
            controller.isJumping = true;
            controller.SetWall();
        }
        else if (Input.GetKey(KeyCode.Space) || controller.IsGrounded())
        {
            controller.SetWall();
        }
    }
    #endregion


    #region Move
    public float Move(float rotateScale, bool ifMovementToA)
    {
        healthBarScaleChanger.transform.localScale = new Vector3((rotateScale > 0) ? -0.045F : 0.045F, 0.045F, 0);  //i jsut change the scale when da local scal is under 0 lol
        controller.transform.rotation = new Quaternion(0F, rotateScale, 0F, 0F);
        var movement = Speed * Time.fixedDeltaTime;
        var newPos = new Vector2((ifMovementToA)
            ? controller.transform.position.x - movement
            : controller.transform.position.x + movement, controller.transform.position.y);
        controller.transform.position = newPos;

        return newPos.x;
    }

    public void IntializeMove(float turnScaleRm, bool pos)
    {
        anim.SetBool("isRunning", true);
        controller.PositionToMove = (pos) ? true : false;
        controller.turnScale = turnScaleRm;
        controller.isMoving = true;
    }

    public void SetPressAbleFields()
    {
        if (controller.isLookingLeft == 0F)
        {
            pressAbleKey[0] = true;
            pressAbleKey[1] = false;
        }
        if (controller.isLookingLeft == 1F)
        {
            pressAbleKey[0] = false;
            pressAbleKey[1] = true;
        }
    }

    private void DoSlide()
    {
        anim.SetBool("isSliding", true);

        if (controller.isLookingLeft == 0)
        {
            controller.rigidbody2D.AddForce(Vector2.right * controller.slideSpeed);
        }
        else
        {
            controller.rigidbody2D.AddForce(Vector2.left * controller.slideSpeed);
        }
        controller.isSliding = true;
    }

    #endregion

    private void SetRunAnimWhenSlide(float lookIndx)
    {
        controller.gotKeyAOrDPressed = true;
        if (controller.isLookingLeft == lookIndx && controller.isSliding)
        {
            anim.SetBool("isSliding", false);
            anim.SetBool("isRunning", true);
        }
    }

}