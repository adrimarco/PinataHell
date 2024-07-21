using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsObject = null;

    private void Start()
    {
        if (creditsObject != null) SetCreditsVisibility(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetCreditsVisibility(bool newVisibility)
    {
        if (creditsObject != null) creditsObject.SetActive(newVisibility);
    }
}
