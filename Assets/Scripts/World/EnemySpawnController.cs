using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int minSpawnWait = 5;
    [SerializeField] private int maxSpawnWait = 15;
    private Transform player;
    [SerializeField] private float maxSpawnOffset;
    //[SerializeField] private float minSpawnOffset;
    private bool readyToSpawn;

    void Start(){
        player = transform.parent;
        StartCoroutine(DoSpawnCooldown());
    }

    void Update(){
        if(readyToSpawn){
            GameObject enemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Vector3 spawnPos = MapInformation.RandomLocation();
            //Debug.Log(spawnPos);
            Instantiate(enemy, spawnPos, Quaternion.identity);
            StartCoroutine(DoSpawnCooldown());
        }
    }
    private IEnumerator DoSpawnCooldown(){
        readyToSpawn = false;
        yield return new WaitForSeconds(Random.Range(minSpawnWait, maxSpawnWait));
        readyToSpawn = true;
    }
}
