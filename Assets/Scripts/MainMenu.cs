using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsObject = null;
    public GameObject tutorialObject = null;

    private void Start()
    {
        SetCreditsVisibility(false);
        SetTutorialVisibility(false);
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

    public void SetTutorialVisibility(bool newVisibility)
    {
        if (tutorialObject != null) tutorialObject.SetActive(newVisibility);
    }
}
