using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    public Animator animator;
    public void GameOver()
    {
        animator.SetTrigger("isDying");
        Debug.Log("You did, lol");
    }
}
