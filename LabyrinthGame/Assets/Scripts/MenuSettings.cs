using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSettings : MonoBehaviour
{
    public string nextSceneName;
   
    public void B_LoadScene() {
        SceneManager.LoadScene(nextSceneName);
    }

    public void B_QuitGame() {
        Application.Quit();
    }
}
