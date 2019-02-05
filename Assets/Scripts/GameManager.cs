using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    #region Unity_functions
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region Scene_Transitions
    public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");

    }

    public void WinGame()
    {
        Debug.Log("Win");
        SceneManager.LoadScene("WinScene");
    }

    public void LoseGame()
    {
        Debug.Log("Loss");
        SceneManager.LoadScene("LoseScene");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
