using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : HealthSystem{
    [SerializeField] private GameObject powerUpPrefab;
    protected override void OnDamage(){
        return;
    }

    protected override void OnDeath(){
        Instantiate(powerUpPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
