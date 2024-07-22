using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Player : MonoBehaviour
{
    const float COOLDOWN_LIMIT = 10f;

    public static Player Instance = null;
    public static int pinatasBroken = 0;
    public static int minutesSurvived = 0;
    public static float secondsSurivived = 0;

    // Player stats
    [Header("Player stats")]
    public float maxHealth = 100.0f;
    private float health = 100.0f;

    [SerializeField]
    private float _movementSpeed = 4.0f;

    private float shield = 0;
    private float maxShield = 0;
    private float shieldRecoverTime = 0;
    private float shieldRecoverTimeWhenBroken = 8f;
    private float shieldRecoverTimeWhenNoBroken = 2f;
    private float shieldRecoverSpeedMultiplier = 1f;

    private float skillCooldown = 0f;
    private float cooldownMultiplier = 1f;

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
    [Header("UI")]
    public HUD hud = null;
    public Shop shopUI = null;
    public DarkPinataUITimer darkPinataUI = null;
    public PauseMenu pauseMenu = null;
    public GameObject gameOverPrefab = null;

    [Header("Components")]
    public HitEnemies damageComp = null;
    private Rigidbody rb = null;
    private Camera cam = null;
    private AudioSource painSound = null;
    private CapsuleCollider playerCollider = null;
    private StarterAssets.StarterAssetsInputs input = null;
    public StarterAssets.FirstPersonController controller = null;

    // Skills
    private SkillData activeSkill = null;

    private List<GameObject> interactionList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        pinatasBroken = 0;
        minutesSurvived = 0;
        secondsSurivived = 0;

        // Get references to necessary components
        rb = GetComponent<Rigidbody>();

        cam = GetComponentInChildren<Camera>();

        painSound = GetComponent<AudioSource>();

        playerCollider = GetComponentInChildren<CapsuleCollider>();

        controller = GetComponent<StarterAssets.FirstPersonController>();

        input = GetComponent<StarterAssets.StarterAssetsInputs>();
        input.attackInputEvent.AddListener(Attack);
        input.interactInputEvent.AddListener(Interaction);
        input.shopInputEvent.AddListener(ToggleShop);
        input.useSkillInputEvent.AddListener(UseSkill);
        input.pauseEvent.AddListener(TogglePauseMenu);


        // Update stats
        health = maxHealth;
        movementSpeed = _movementSpeed;
        candies = 0;

        // Initialize UI
        hud.UpdateHealthBar(shield, health, maxHealth);

        shopUI.gameObject.SetActive(false);
        shopUI.SetPlayer(this);

        darkPinataUI.gameObject.SetActive(false);

        pauseMenu.gameObject.SetActive(false);

        Enemy.onEnemyDead.AddListener(AddCandies);

        EnableUIMode(false);
    }

    // Update is called once per frame
    void Update()
    {
        RecoverShield();
        SkillOnCooldown();

        secondsSurivived += Time.deltaTime;
        if (secondsSurivived >= 60)
        {
            minutesSurvived += 1;
            secondsSurivived -= 60;
        }
    }

    public void Damage(float damage)
    {
        if (damage <= 0) return;

        if (shield > 0)
        {
            DamageShield(damage);
        }
        else
        {
            health -= damage;

            if (health <= 0) PlayerDead();
        }

        hud.UpdateHealthBar(shield, health, maxHealth);
        hud.PlayDamagedAnim();

        if (painSound != null && !painSound.isPlaying) painSound.Play();
    }

    private void PlayerDead()
    {
        EnableUIMode(true);
        Instantiate(gameOverPrefab, transform);
    }

    public void Attack()
    {
        if (!controller.enabled) return;

        damageComp.Attack();
    }

    private void DamageShield(float damage)
    {
        shield -= damage;
        if (shield <= 0)
        {
            // Shield broken
            shield = 0;
            shieldRecoverTime = shieldRecoverTimeWhenBroken;
        }
        else
        {
            // Shield not broken
            shieldRecoverTime = shieldRecoverTimeWhenNoBroken;
        }
    }

    public void Interaction()
    {
        if (!controller.enabled) return;

        if (interactionList.Count > 0)
        {
            Pickable p;
            if (interactionList[interactionList.Count-1].TryGetComponent<Pickable>(out p))
            {
                p.OnPlayerInteract();
            }
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu == null || health <= 0) return;

        bool newPauseState = !pauseMenu.gameObject.activeSelf;

        pauseMenu.gameObject.SetActive(newPauseState);

        if (newPauseState)
        {
            EnableUIMode(true);
        }
        else if (shopUI != null && !shopUI.gameObject.activeSelf)
        {
            EnableUIMode(false);
        }
    }

    public void ToggleShop()
    {
        if (shopUI == null) return;

        if (pauseMenu != null && pauseMenu.gameObject.activeSelf) return;

        bool shopNewState = !shopUI.gameObject.activeSelf;

        if(shopNewState) shopUI.UpdateUI();

        shopUI.gameObject.SetActive(shopNewState);
        EnableUIMode(shopNewState);
    }

    private void EnableUIMode(bool newState)
    {
        Cursor.visible = newState;
        Cursor.lockState = newState ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = newState ? 0 : 1;

        controller.enabled = !newState;
    }

    public void UpdateActiveSkill(SkillData newSkill)
    {
        RemoveActiveSkill();
        AddActiveSkill(newSkill);
    }

    public void RemoveActiveSkill()
    {
        if (activeSkill == null) return;

        activeSkill.skill.OnDeactivate();

        Destroy(activeSkill.skill);
        Destroy(activeSkill);
        activeSkill = null;
    }

    public void AddActiveSkill(SkillData newSkill)
    {
        if (activeSkill != null) return;
        
        activeSkill = gameObject.AddComponent<SkillData>();
        activeSkill.icon = newSkill.icon;
        activeSkill.description = newSkill.description;
        activeSkill.cooldown = newSkill.cooldown;
        activeSkill.skill = newSkill.skill.CopyComponent(gameObject);

        activeSkill.skill.OnActivate();

        skillCooldown = Mathf.Min(skillCooldown, COOLDOWN_LIMIT);

        // HUD visualization
        hud.ChangeCurrentSkill(activeSkill);

        // Show new skill information on HUD
        hud.ShowShowSkillLayer(activeSkill);
    }

    public void UseSkill()
    {
        if (activeSkill == null || activeSkill.skill == null) return;

        if (skillCooldown > 0) return;

        activeSkill.skill.OnUse();

        skillCooldown = activeSkill.cooldown;
    }

    private void SkillOnCooldown()
    {
        if (skillCooldown > 0)
        {
            skillCooldown -= Time.deltaTime * cooldownMultiplier;
            hud.UpdateSkillCooldownBar((activeSkill.cooldown - skillCooldown) / activeSkill.cooldown);
        }
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
        hud.UpdateHealthBar(shield, health, maxHealth);
        hud.PlayHealAnim();
    }

    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        Heal(amount);
    }

    public void AddCandies(int candiesAmount)
    {
        candies += Math.Max(candiesAmount, 0);

        hud.SetCandiesAmount(candies);
    }

    public int GetCandiesAmount()
    {
        return candies;
    }

    public void ReduceCandies(int candiesAmount)
    {
        candies -= candiesAmount;

        hud.SetCandiesAmount(candies);
    }

    public void IncreaseShieldCapacity(float shieldInc)
    {
        maxShield += shieldInc;
        shieldRecoverTime = 0;
    }

    public void RecoverShield()
    {
        if (shield >= maxShield) return;

        if (shieldRecoverTime > 0)
        {
            shieldRecoverTime -= Time.deltaTime;
        }
        else
        {
            shield = Mathf.Min(shield + Time.deltaTime * 3 * shieldRecoverSpeedMultiplier, maxShield);
            hud.UpdateHealthBar(shield, health, maxHealth);
        }
    }

    public void RecoverShield(float shieldInc)
    {
        shield += shieldInc;
        hud.UpdateHealthBar(shield, health, maxHealth);
    }

    public void IncreaseDamage(float damageInc)
    {
        damageComp.weaponDamage += damageInc;
    }

    public void IncreaseCooldownSpeed()
    {
        cooldownMultiplier += 0.2f;
    }
}
