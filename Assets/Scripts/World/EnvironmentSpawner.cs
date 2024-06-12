using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSpawner : MonoBehaviour
{
    [SerializeField] private float spawnDelay;
    private bool canSpawn;
    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private int startingPlanets;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 1; i <= startingPlanets; i++){
            SpawnPlanet();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canSpawn){
            SpawnPlanet();
        }
    }
    private void SpawnPlanet(){
        int x = MapInformation.GetMaxX();
        int y = MapInformation.GetMaxY();
        Instantiate(planetPrefab, MapInformation.RandomLocation(), Quaternion.identity);
        StartCoroutine(DoSpawnDelay());
    }
    private IEnumerator DoSpawnDelay(){
        canSpawn = false;
        yield return new WaitForSeconds(spawnDelay);
        canSpawn = true;
    }
}
