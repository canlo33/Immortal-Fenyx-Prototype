using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;
    public bool isInvulrable = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth * 100f;
    }
    public int GetHealth()
    {
        return currentHealth;
    }
    public void Damage(int amount)
    {
        if (isInvulrable) return;
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
}
