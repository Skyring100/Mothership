using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinishipBullet : MonoBehaviour{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private string[] damageableTags;
    [SerializeField] private float damage;
    private void Start(){
        StartCoroutine(StartLifeTime());
    }
    private void Update(){
        transform.position += transform.up * speed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other){
        //check if this bullet hit an enemy
        for(int i = 0; i < damageableTags.Length; i++){
            if(other.CompareTag(damageableTags[i])){
                if(other.gameObject != null && !other.gameObject.IsDestroyed()){
                    other.gameObject.SendMessage("DamageEntity", damage);
                }
                Destroy(gameObject);
            }
        } 
    }
    private IEnumerator StartLifeTime(){
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    public void SetSpeed(float s){
        speed = s;
    }
    public void SetDamage(float d){
        damage = d;
    }
}
