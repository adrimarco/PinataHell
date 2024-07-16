using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    const int HEALTH_INITIAL_COST = 200;
    const int SHIELD_INITIAL_COST = 500;
    const int DAMAGE_INITIAL_COST = 100;
    const int COOLDOWN_INITIAL_COST = 800;

    private Player player = null;

    // Health
    [Header("Health")]
    public TextMeshProUGUI healthCostText = null;
    public TextMeshProUGUI healthLevelText = null;
    public TextMeshProUGUI healthBuyText = null;
    private int healthCost = HEALTH_INITIAL_COST;
    private int healthLevel = 1;

    // Shield
    [Space(10)]
    [Header("Shield")]
    public TextMeshProUGUI shieldCostText = null;
    public TextMeshProUGUI shieldLevelText = null;
    public TextMeshProUGUI shieldBuyText = null;
    private int shieldCost = SHIELD_INITIAL_COST;
    private int shieldLevel = 1;

    // Damage
    [Space(10)]
    [Header("Damage")]
    public TextMeshProUGUI damageCostText = null;
    public TextMeshProUGUI damageLevelText = null;
    public TextMeshProUGUI damageBuyText = null;
    private int damageCost = DAMAGE_INITIAL_COST;
    private int damageLevel = 1;

    // Cooldown
    [Space(10)]
    [Header("Cooldown")]
    public TextMeshProUGUI cooldownCostText = null;
    public TextMeshProUGUI cooldownLevelText = null;
    public TextMeshProUGUI cooldownBuyText = null;
    private int cooldownCost = COOLDOWN_INITIAL_COST;
    private int cooldownLevel = 1;

    public void SetPlayer(Player p)
    {
        player = p;
    }

    public void UpdateUI()
    {
        healthCostText.text = healthCost.ToString();
        healthLevelText.text = healthLevel.ToString() + " > " + (healthLevel + 1).ToString();
        healthCostText.color = healthBuyText.color = player.GetCandiesAmount() >= healthCost ? Color.white : Color.red;

        shieldCostText.text = shieldCost.ToString();
        shieldLevelText.text = shieldLevel.ToString() + " > " + (shieldLevel + 1).ToString();
        shieldCostText.color = shieldBuyText.color = player.GetCandiesAmount() >= shieldCost ? Color.white : Color.red;

        damageCostText.text = damageCost.ToString();
        damageLevelText.text = damageLevel.ToString() + " > " + (damageLevel + 1).ToString();
        damageCostText.color = damageBuyText.color = player.GetCandiesAmount() >= damageCost ? Color.white : Color.red;

        cooldownCostText.text = cooldownCost.ToString();
        cooldownLevelText.text = cooldownLevel.ToString() + " > " + (cooldownLevel + 1).ToString();
        cooldownCostText.color = cooldownBuyText.color = player.GetCandiesAmount() >= cooldownCost ? Color.white : Color.red;
    }

    public void BuyHealth()
    {
        if (player.GetCandiesAmount() < healthCost) return;

        // Effect
        player.IncreaseMaxHealth(20);
        player.ReduceCandies(healthCost);

        // Update values
        healthCost += HEALTH_INITIAL_COST;
        healthLevel += 1;

        UpdateUI();
    }

    public void BuyShield()
    {
        if (player.GetCandiesAmount() < shieldCost) return;

        // Effect
        player.IncreaseShieldCapacity(20);
        player.ReduceCandies(shieldCost);

        // Update values
        shieldCost += SHIELD_INITIAL_COST;
        shieldLevel += 1;

        UpdateUI();
    }

    public void BuyDamage()
    {
        if (player.GetCandiesAmount() < damageCost) return;

        // Effect
        player.IncreaseDamage(4f);
        player.ReduceCandies(damageCost);

        // Update values
        damageCost += DAMAGE_INITIAL_COST;
        damageLevel += 1;

        UpdateUI();
    }

    public void BuyCooldown()
    {
        if (player.GetCandiesAmount() < cooldownCost) return;

        // Effect
        player.IncreaseCooldownSpeed();
        player.ReduceCandies(cooldownCost);

        // Update values
        cooldownCost += COOLDOWN_INITIAL_COST;
        cooldownLevel += 1;

        UpdateUI();
    }
}
