using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberRadius : MonoBehaviour
{
    [SerializeField] public float maxRadius;
    [SerializeField] public float growRate;
    [SerializeField] private float explosionLifetime;
    [SerializeField] private float explosionDamage;
    private CircleCollider2D hitCollider;
    private void Start() {
        hitCollider = GetComponent<CircleCollider2D>();
        hitCollider.enabled = false;
        transform.localScale = new Vector3(0,0);
    }
    public void GrowRadius(){
        transform.localScale += new Vector3(growRate, growRate) * Time.deltaTime;
        if(transform.localScale.x >= maxRadius){
            Explode();
        }
    }
    public void StopGrow(){
        transform.localScale -= new Vector3(growRate, growRate) * Time.deltaTime;
        //check if this is under 0
        if(transform.localScale.x < 0){
            transform.localScale = new Vector3(0,0);
        }
    }
    private void Explode(){
        hitCollider.enabled = true;
        StartCoroutine(ExplosionCountDown());
    }
    private IEnumerator ExplosionCountDown(){
        yield return new WaitForSeconds(explosionLifetime);
        Destroy(transform.parent.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        other.SendMessage("DamageEntity", explosionDamage, SendMessageOptions.DontRequireReceiver);
    }
}
