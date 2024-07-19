using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DarkPinataUITimer : MonoBehaviour
{
    const string warningAnim = "DarkPinataWarning";
    const string timeoutAnim = "DarkPinataTimeout";
    const string defeatedAnim = "DarkPinataDefeated";

    public TextMeshProUGUI timerText;
    public int changeColorTime = 10;

    private float timer = 0f;
    private bool activeTimer = false;
    // Time displayed in UI
    private int uiSeconds = 0;

    private Animation anim;

    private void Start()
    {
        anim = GetComponent<Animation>();
    }

    private void Update()
    {
        if (!activeTimer) return;

        timer -= Time.deltaTime;

        int remainingSeconds = Mathf.CeilToInt(timer);
        if (remainingSeconds < uiSeconds)
        {
            uiSeconds = remainingSeconds;
            SetRemainingSeconds(uiSeconds);
        }

        // Deactivate timer when reaching 0
        if (timer < 0f)
        {
            activeTimer = false;

            if (anim != null) anim.Play(timeoutAnim);

            Invoke("HideTimer", 10);
        }
    }

    public void SetTimer(float time)
    {
        if (time > 0f)
        {
            timer = time;
            activeTimer = true;
            uiSeconds = Mathf.CeilToInt(timer);
        }

        gameObject.SetActive(true);
        if (anim != null) anim.Play(warningAnim);
    }

    public void SetRemainingSeconds(int seconds)
    {
        if (timerText == null) return;

        // Make sure seconds is never below 0
        seconds = Mathf.Max(seconds, 0);

        timerText.text = seconds.ToString();

        if (seconds < changeColorTime)
        {
            timerText.color = seconds % 2 == 0 ? Color.red : Color.white;
        }
    }

    public void HideTimer()
    {
        if (anim != null)
        {
            anim.Play(defeatedAnim);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void DeactivateObject()
    {
        gameObject.SetActive(false);
    }
}
