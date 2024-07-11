using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEnemies : MonoBehaviour
{
    private Animator animator;
    private Camera cam;
    private bool attacking = false;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        if (!attacking) {
            attacking = true;
            animator.Play("BatHit");
        }
    }

    public void CheckEnemyHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 2))
        {
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(cam.transform.forward * 10, ForceMode.Impulse);
            }
        }
    }

    public void AttackFinished()
    {
        attacking = false;
        Debug.Log("Attack finished");
    }
}
