using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase : MonoBehaviour 
{
    public virtual SkillBase CopyComponent(GameObject player)
    {
        return player.AddComponent<SkillBase>();
    }

    public virtual void OnActivate() { }
    public virtual void OnUse() { }
    public virtual void OnDeactivate() { }
}
