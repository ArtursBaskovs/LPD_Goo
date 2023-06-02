using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndMenu : MonoBehaviour
{
    public void GameExit()
    {
        Debug.Log("GameExit");
        Application.Quit();
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
}
