using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    const string damagedAnim = "PlayerDamagedHUD";
    const string healAnim = "PlayerHealedHUD";


    [Header("HUD")]
    public Image healthBar;
    public Image shieldBar;
    public TextMeshProUGUI candiesText;
    public Image currentSkillIcon;
    public Image skillCooldownBar;

    [Space(10)]
    [Header("Show new skill layer")]
    // Show skill layer, for showing new skills unlocked
    public GameObject showSkillLayer;
    public Image showSkillIcon;
    public TextMeshProUGUI showSkillDescription;

    [Space(10)]
    [Header("Change skill layer")]
    // Change skill layer, to decide if changing current skill
    public GameObject changeSkillLayer;
    public Image newSkillIcon;

    [Space(10)]
    [Header("Events")]
    public TextMeshProUGUI eventsText;

    [Space(10)]
    public Animation skillAnim;
    public Animation effectsAnim;
    public Animation eventAnim;

    void Awake()
    {
        showSkillLayer.SetActive(false);
        changeSkillLayer.SetActive(false);
        currentSkillIcon.gameObject.SetActive(false);
        eventsText.text = string.Empty;
    }

    public void ShowChangeSkillLayer(SkillData newSkill, SkillData currentSkill)
    {
        newSkillIcon.sprite = newSkill.icon;
        changeSkillLayer.SetActive(true);
    }

    public void HideChangeSkillLayer()
    {
        changeSkillLayer.SetActive(false);
    }

    public void ShowShowSkillLayer(SkillData skill)
    {
        if (skill == null) return;

        if (skillAnim.isPlaying) skillAnim.Stop();

        showSkillIcon.sprite = skill.icon;
        showSkillDescription.text = skill.description;
        skillAnim.Play();
        showSkillLayer.SetActive(true);
    }

    public void PlayDamagedAnim()
    {
        if (effectsAnim.isPlaying) return;

        effectsAnim.Play(damagedAnim);
    }

    public void PlayHealAnim()
    {
        if (effectsAnim.isPlaying) effectsAnim.Stop();

        effectsAnim.Play(healAnim);
    }

    public void HideShowSkillLayer()
    {
        showSkillLayer.SetActive(false);
    }

    public void ChangeCurrentSkill(SkillData newSkill)
    {
        if (newSkill != null)
        {
            currentSkillIcon.gameObject.SetActive(true);
            currentSkillIcon.sprite = newSkill.icon;
        }
    }

    public void SetCandiesAmount(int candiesAmount)
    {
        candiesText.text = candiesAmount.ToString();
    }

    public void UpdateHealthBar(float shield, float health, float maxHealth)
    {
        shieldBar.fillAmount = Mathf.Clamp(shield/maxHealth, 0, 1);
        
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
    }

    public void UpdateSkillCooldownBar(float cooldownPercentage)
    {
        skillCooldownBar.fillAmount = cooldownPercentage;

        currentSkillIcon.color = cooldownPercentage >= 1 ? Color.white : new Color(0.5f, 0.5f, 0.5f, 0.5f);
    }

    public void PlayLavaWarning()
    {
        eventsText.text = "Warning\nThe floor is lava";
        eventAnim.Play();
    }

    public void PlayGasWarning()
    {
        eventsText.text = "Warning\nYou are in a gas chamber";
        eventAnim.Play();
    }
}
