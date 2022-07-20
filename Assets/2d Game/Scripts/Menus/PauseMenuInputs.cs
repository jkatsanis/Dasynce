using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuInputs : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    public bool aMenuEnabled;       //The only public variable here bacily just to not do attacks or smth while in playerController  In player controller... 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    private void DisablePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    private void PauseMenu()
    {
        aMenuEnabled = true;
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
}
