using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SkillBase))]
public class SkillData : MonoBehaviour 
{
    public Sprite icon;
    public string description;
    public float cooldown;
    public SkillBase skill;

    private void Awake()
    {
        skill = GetComponent<SkillBase>();
    }
}
