using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StaticSceneManager 
{
    public static int enterScreen = 0;
    public static int levelSelectorScene = 1;
    public static int levelOneScene = 2;

    public static void LoadScene(int indx)
    {
        SceneManager.LoadScene(indx);
    }

    public static void LoadScene(int indx, GameObject go)
    {
        Scene scene = SceneManager.GetSceneByBuildIndex(indx);
        SceneManager.LoadScene(indx);    
    }
}
