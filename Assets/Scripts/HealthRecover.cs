using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRecover : MonoBehaviour
{
    [Range(0f, 1f)]
    public float health = 0.3f;
    private bool pickable = true;

    private void OnTriggerStay(Collider other)
    {
        Player player;
        if (pickable && other.gameObject.TryGetComponent<Player>(out player))
        {
            if (!player.IsFullyHealed())
            {
                player.Heal(health * player.maxHealth);

                AudioSource sound;
                if (TryGetComponent<AudioSource>(out sound))
                {
                    sound.Play();
                }


                StartCoroutine(DestroyAfterTime(2f));
            }
        }
    }

    private IEnumerator DestroyAfterTime(float waitTime)
    {
        pickable = false;

        MeshRenderer mesh = GetComponentInChildren<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
