using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("HUD")]
    public Image healthBar;
    public Image shieldBar;
    public TextMeshProUGUI candiesText;
    public Image currentSkillIcon;

    [Space(10)]
    [Header("Show new skill layer")]
    // Show skill layer, for showing new skills unlocked
    public GameObject showSkillLayer;
    public Image skillIcon;
    public TextMeshProUGUI skillDescription;

    [Space(10)]
    [Header("Change skill layer")]
    // Change skill layer, to decide if changing current skill
    public GameObject changeSkillLayer;
    public GameObject changeSkillIcon;
    public Image newSkillIcon;

    // Start is called before the first frame update
    void Start()
    {
        showSkillLayer.SetActive(false);
        changeSkillLayer.SetActive(false);
    }

    public void ShowChangeSkillLayer(SkillData newSkill, SkillData currentSkill)
    {
        newSkillIcon.sprite = newSkill.icon;

        if (currentSkill != null)
        {
            currentSkillIcon.gameObject.SetActive(true);
            changeSkillIcon.SetActive(true);
        }
        else
        {
            currentSkillIcon.gameObject.SetActive(false);
            changeSkillIcon.SetActive(false);
        }

        changeSkillLayer.SetActive(true);
    }

    public void HideChangeSkillLayer()
    {
        changeSkillLayer.SetActive(false);
    }

    public void ShowShowSkillLayer(SkillData skill)
    {
        if (skill == null) return;

        skillIcon.sprite = skill.icon;
        skillDescription.text = skill.description;
        showSkillLayer.SetActive(true);
    }

    public void HideShowSkillLayer()
    {
        showSkillLayer.SetActive(false);
    }

    public void ChangeCurrentSkill(SkillData newSkill)
    {
        if (newSkill != null) currentSkillIcon.sprite = newSkill.icon;
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
}
