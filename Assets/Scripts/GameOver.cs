using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI brokenPinatasText;
    public TextMeshProUGUI timeSurvivedText;

    void Start()
    {
        brokenPinatasText.text = "Broken pinatas: " + Player.pinatasBroken.ToString();
        timeSurvivedText.text = "Survived time: " + Player.minutesSurvived.ToString() + ":" + Mathf.FloorToInt(Player.secondsSurivived).ToString();
    }

    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
