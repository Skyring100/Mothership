using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueBullet : MonoBehaviour{
    [SerializeField] private float lifeTime;
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    private void Start(){
        StartCoroutine(StartLifeTime());
    }
    private void Update(){
        transform.position += transform.up * speed * Time.deltaTime;
    }

    private IEnumerator StartLifeTime(){
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(!other.name.Contains("Rogue Ship")&& (other.transform.CompareTag("Player") || other.transform.CompareTag("Mini Ship") || other.transform.CompareTag("Enemy"))){
            other.SendMessage("DamageEntity", damage, SendMessageOptions.DontRequireReceiver);
            Destroy(gameObject);
        }
    }
}
