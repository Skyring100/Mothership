using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueBullet : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    private void Start() {
        StartCoroutine(StartLifeTime());
    }
    private void Update() {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private IEnumerator StartLifeTime(){
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") || other.CompareTag("Mini Ship") || other.CompareTag("Enemy") || other.CompareTag("Planet")){
            other.SendMessage("DamageEntity", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
