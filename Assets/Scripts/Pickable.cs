using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    private BoxCollider collision = null;
    private Player overlappingPlayer = null;

    // Start is called before the first frame update
    void Start()
    {
        collision = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player;
        if (other.CompareTag("Player") && other.gameObject.TryGetComponent<Player>(out player))
        {
            overlappingPlayer = player;
            OnPlayerOverlaps();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player;
        if (other.CompareTag("Player") && other.gameObject.TryGetComponent<Player>(out player))
        {
            OnPlayerEndOverlap();
        }
    }

    private void OnPlayerOverlaps()
    {
        if (overlappingPlayer != null) overlappingPlayer.AddPickable(gameObject);
    }

    private void OnPlayerEndOverlap()
    {
        if (overlappingPlayer != null)
        {
            overlappingPlayer.RemovePickable(gameObject);
            overlappingPlayer = null;
        }
    }

    public void OnPlayerInteract()
    {
        if (overlappingPlayer == null) return;

        SkillData skill;
        if (TryGetComponent<SkillData>(out skill))
        {
            overlappingPlayer.UpdateActiveSkill(skill);
        }
        OnPlayerEndOverlap();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        OnPlayerEndOverlap();
    }
}
