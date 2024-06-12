using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private float spawnOffset;
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int minSpawnWait = 5;
    [SerializeField] private int maxSpawnWait = 15;
    private bool readyToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoSpawnCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        if(readyToSpawn){
            GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(enemy, MapInformation.RandomLocation(), Quaternion.identity);
            StartCoroutine(DoSpawnCooldown());
        }
    }
    private IEnumerator DoSpawnCooldown(){
        readyToSpawn = false;
        yield return new WaitForSeconds(Random.Range(minSpawnWait, maxSpawnWait));
        readyToSpawn = true;
    }
}
