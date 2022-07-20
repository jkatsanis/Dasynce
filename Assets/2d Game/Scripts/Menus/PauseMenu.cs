using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    PauseMenuInputs swC;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject pauseMenu;

    private const int CURRENT_DETECTDMG_HEALTH = 100;
    private DetectDamageOfEnemys detectDmg;                 //Just to set the health back when needed

    private void Start()
    {
        swC = player.GetComponent<PauseMenuInputs>();
        detectDmg = player.GetComponent<DetectDamageOfEnemys>();
    }
    public void Resume()    
    {
        swC.aMenuEnabled = false;
        SetHealthBack(detectDmg.currentHealth);
    }

    public void GoToMenu()
    {
        SetHealthBack(CURRENT_DETECTDMG_HEALTH);
        SceneManager.LoadScene(StaticSceneManager.enterScreen);
    }

    private void SetHealthBack(int currentHealth)
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        detectDmg.currentHealth = currentHealth;
    }
}
