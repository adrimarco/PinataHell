using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Skill_Shock : SkillBase
{
    ParticleSystem shockParticles = null;

    public override void OnActivate()
    {
        shockParticles = Resources.Load<ParticleSystem>("ShockParticles");
    }

    public override void OnUse() 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Vector3 playerPosition = Player.Instance.transform.position;

        foreach(GameObject enemy in enemies)
        {
            if(Vector3.Distance(playerPosition, enemy.transform.position) < 11)
            {
                Enemy e;
                if(enemy.TryGetComponent<Enemy>(out e))
                {
                    e.ApplyStun(5f);
                }
            }
        }

        Vector3 particlesPosition = Player.Instance.transform.position;
        particlesPosition.y += 0.1f;
        GameObject.Instantiate(shockParticles, particlesPosition, shockParticles.transform.rotation);
    }

    public override SkillBase CopyComponent(GameObject player)
    {
        return player.AddComponent<Skill_Shock>();
    }
}
