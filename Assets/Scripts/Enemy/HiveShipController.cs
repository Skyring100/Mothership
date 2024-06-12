using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveShipController : HealthSystem
{
    [SerializeField] private GameObject enemyMiniShipPrefab;
    [SerializeField] private float spawnDelay;
    [SerializeField] private GameObject powerupPrefab;
    private bool canSpawn;
    [SerializeField] private GameObject detection;
    private GeneralDetectionHitbox detectScript;
    private void Start() {
        detectScript = Instantiate(detection).GetComponentInChildren<GeneralDetectionHitbox>();
        detectScript.SetFollow(transform);
        canSpawn = true;
    }
    private void Update() {
        if(detectScript.HasDetection() && canSpawn){
            Transform target = detectScript.GetDetection();
            Instantiate(enemyMiniShipPrefab, transform.position, Quaternion.identity).SendMessage("StartObjective", new MinionTargetInfo(target, transform));
            StartCoroutine(DoSpawnDelay());
        }
    }
    private void ShipReturned(MinishipController c){
        Destroy(c.gameObject);
    }
    private IEnumerator DoSpawnDelay(){
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
    protected override void OnDamage()
    {
        return;
    }

    protected override void OnDeath()
    {
        if(Random.Range(0,10) < 8){
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
