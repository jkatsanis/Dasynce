using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class DieMenu : MonoBehaviour
{
    [SerializeField] private const int CURRENT_DETECTDMG_HEALTH = 100;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject dieMenu;

    private DetectDamageOfEnemys detectDmg;

    private void Start()
    {
        detectDmg = player.GetComponent<DetectDamageOfEnemys>();
    }

    public void TryAgain()
    {
        SetHealthBack(CURRENT_DETECTDMG_HEALTH);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void GoToMenu()
    {
        SetHealthBack(CURRENT_DETECTDMG_HEALTH);
        SceneManager.LoadScene(StaticSceneManager.enterScreen);
    }

    private void SetHealthBack(int currentHealth)
    {
        dieMenu.SetActive(false);
        Time.timeScale = 1;
        detectDmg.currentHealth = currentHealth;
    }
}
