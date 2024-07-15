using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    // Player stats
    [Header("Player stats")]
    public float maxHealth = 100.0f;
    [SerializeField]
    private float _movementSpeed = 4.0f;
    private float health = 100.0f;
    private int candies = 0;

    public float movementSpeed
    {
        get { return _movementSpeed; }
        set
        {
            _movementSpeed = value;
            if (controller != null)
            {
                controller.MoveSpeed = _movementSpeed;
                controller.SprintSpeed = _movementSpeed;
            }
        }
    }


    // Components
    public HUD hud = null;
    private Rigidbody rb = null;
    private Camera cam = null;
    private CapsuleCollider playerCollider = null;
    private StarterAssets.StarterAssetsInputs input = null;
    private StarterAssets.FirstPersonController controller = null;

    // Skills
    private SkillData activeSkill = null;

    private List<GameObject> interactionList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        // Get references to necessary components
        rb = GetComponent<Rigidbody>();

        cam = GetComponentInChildren<Camera>();

        playerCollider = GetComponentInChildren<CapsuleCollider>();

        controller = GetComponent<StarterAssets.FirstPersonController>();

        input = GetComponent<StarterAssets.StarterAssetsInputs>();
        input.interactInputEvent.AddListener(Interaction);


        // Update stats
        health = maxHealth;
        movementSpeed = _movementSpeed;

        Enemy.onEnemyDead.AddListener(AddCandies);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        health -= damage;
        Debug.Log(health + "/" + maxHealth);
    }

    public void Interaction()
    {
        if(interactionList.Count > 0)
        {
            Pickable p;
            if (interactionList[interactionList.Count-1].TryGetComponent<Pickable>(out p))
            {
                p.OnPlayerInteract();
            }
        }
    }

    public void UpdateActiveSkill(SkillData newSkill)
    {
        RemoveActiveSkill();
        AddActiveSkill(newSkill);
    }

    public void RemoveActiveSkill()
    {
        if (activeSkill == null) return;

        Destroy(activeSkill);
        activeSkill = null;
    }

    public void AddActiveSkill(SkillData newSkill)
    {
        if (activeSkill != null) return;

        activeSkill = gameObject.AddComponent<SkillData>();
        activeSkill.icon = newSkill.icon;
        activeSkill.description = newSkill.description;
        activeSkill.skill = newSkill.skill;

        // Cancel invocation of hide function, necessary if other skill was picked recently
        hud.CancelInvoke("HideShowSkillLayer");

        // Show new skill information on HUD
        hud.ShowShowSkillLayer(activeSkill);
        hud.Invoke("HideShowSkillLayer", 5.0f);
    }

    public void AddPickable(GameObject p)
    {
        interactionList.Add(p);

        SkillData skill;
        if(p.TryGetComponent<SkillData>(out skill))
        {
            hud.ShowChangeSkillLayer(skill, activeSkill);
        }
    }

    public void RemovePickable(GameObject p)
    {
        interactionList.Remove(p);

        if(interactionList.Count > 0)
        {
            SkillData skill;
            if (interactionList[interactionList.Count-1].TryGetComponent<SkillData>(out skill))
            {
                hud.ShowChangeSkillLayer(skill, activeSkill);
            }
        }
        else
        {
            hud.HideChangeSkillLayer();
        }
    }

    public bool IsFullyHealed()
    {
        return health >= maxHealth;
    }

    public void Heal(float healing)
    {
        health = Mathf.Min(health + healing, maxHealth);
        Debug.Log(health + "/" + maxHealth);
    }

    public void AddCandies(int candiesAmount)
    {
        candies += Math.Max(candiesAmount, 0);
    }
}
