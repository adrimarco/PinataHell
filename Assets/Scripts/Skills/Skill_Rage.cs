using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_Rage : SkillBase
{
    bool playerOnRage = false;
    AudioClip sound = null;

    public override void OnActivate()
    {
        sound = Resources.Load<AudioClip>("Rage");
    }

    public override void OnUse() 
    {
        playerOnRage = true;

        Player p = Player.Instance;

        p.movementSpeed += 2;
        p.damageComp.SetAttackSpeedMultiplier(1.8f);
        p.PlaySound(sound);

        Invoke("FinishEffect", 8);
    }

    public void FinishEffect()
    {
        playerOnRage = false;

        Player p = Player.Instance;

        p.movementSpeed -= 2;
        p.damageComp.SetAttackSpeedMultiplier();
    }

    private void OnDestroy()
    {
        if (playerOnRage) FinishEffect();
    }

    public override SkillBase CopyComponent(GameObject player)
    {
        return player.AddComponent<Skill_Rage>();
    }
}
