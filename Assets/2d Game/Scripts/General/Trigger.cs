
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField] private Animator anim;
    [SerializeField] private new Rigidbody2D rigidbody;
    #endregion

    #region Private Variables
    private new string name = " ";
    private PlayerController playerController;
    private bool enableSlideWallCooldown;
    private int cnt;
    private bool _canDoWallSetup;
    #endregion

    public void Start()
    {
        playerController = GetComponent<PlayerController>();
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        _canDoWallSetup = false;
        if (playerController.IsGrounded())
        {
            name = " ";
        }
        if (enableSlideWallCooldown)
        {
            cnt++;
        }
        if (cnt == playerController.slideDownOfWallCooldown /*|| !playerController.onWall*/)
        {
            enableSlideWallCooldown = false; 
            cnt = 0;
            playerController.setWallPosDown = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _canDoWallSetup = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            _canDoWallSetup = true;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)          //The player always collides so "other" are the objects the player 
    {                                                       //  collides with
        if (other.CompareTag("Door")) 
        {
            SwitchScene();
        }
        if (other.CompareTag("Wall") && other.gameObject.name != name && _canDoWallSetup)
        {
            DoWallSetup(other);
        }
    }

    private void SwitchScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void DoWallSetup(Collider2D other)
    {
        playerController.setSmallDelayAterWallConnection = true;
        enableSlideWallCooldown = true;
        name = other.gameObject.name;
        rigidbody.gravityScale = 0F;
        playerController.transformPosY = playerController.transform.position.y;
        playerController.onWall = true;
        anim.SetBool("onWall", true);
        _canDoWallSetup = false;
    }
}