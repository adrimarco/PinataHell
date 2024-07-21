using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public void ResumeGame()
    {
        Player.Instance.TogglePauseMenu();
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
