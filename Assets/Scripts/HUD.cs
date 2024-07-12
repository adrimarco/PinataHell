using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    // Show skill layer, for showing new skills unlocked
    [Header("Show new skill layer")]
    public GameObject showSkillLayer;
    public Image skillIcon;
    public TextMeshProUGUI skillDescription;

    [Space(10)]
    [Header("Change skill layer")]
    // Change skill layer, to decide if changing current skill
    public GameObject changeSkillLayer;
    public GameObject changeSkillIcon;
    public Image currentSkillIcon;
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
            currentSkillIcon.sprite = currentSkill.icon;
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
}
