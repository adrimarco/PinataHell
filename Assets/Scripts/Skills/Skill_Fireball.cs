using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_Fireball : SkillBase
{
    Fireball fireballPrefab = null;

    public override void OnActivate()
    {
        fireballPrefab = Resources.Load<Fireball>("Fireball");
    }

    public override void OnUse() 
    {
        Vector3 movementDirection = Camera.main.transform.forward;
        Vector3 spawnPosition = Camera.main.transform.position;
        spawnPosition += movementDirection;

        Fireball spawnedFireball = GameObject.Instantiate(fireballPrefab, spawnPosition, Camera.main.transform.rotation);

        spawnedFireball.direction = movementDirection;
        spawnedFireball.damage = Player.Instance.damageComp.weaponDamage * 2;
        spawnedFireball.speed = 40;
        spawnedFireball.GetComponent<AutoDestroyDisappear>().time = 2f;
    }

    public override SkillBase CopyComponent(GameObject player)
    {
        return player.AddComponent<Skill_Fireball>();
    }
}
