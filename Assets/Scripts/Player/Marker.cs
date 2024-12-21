using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour{
    [SerializeField] private float lifeTime;
    private void Start() {
        StartCoroutine(DoLifeTime());
    }
    private IEnumerator DoLifeTime(){
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Mini Ship")){
            Destroy(gameObject);
        }
    }
}
