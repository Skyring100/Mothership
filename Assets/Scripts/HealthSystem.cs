using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealthSystem : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    private float currentHealth;
    private void Awake() {
        currentHealth = maxHealth;
    }

    public void DamageEntity(float damage){
        currentHealth -= damage;
        OnDamage();
        if(currentHealth <= 0){
            OnDeath();
        }
    }
    public void HealEntity(float heal){
        currentHealth += heal;
        if(currentHealth > maxHealth){
            currentHealth = maxHealth;
        }
    }
    public void SetMaxHealth(float h){
        maxHealth = h;
        currentHealth = maxHealth;
    }
    public float GetHealth(){
        return currentHealth;
    }
    public float GetMaxHealth(){
        return maxHealth;
    }
    protected abstract void OnDamage();
    protected abstract void OnDeath();

}
