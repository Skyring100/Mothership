using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonkerEnemyController : HealthSystem{
    [SerializeField] private GameObject powerupPrefab;
    [SerializeField] private float chargeForce;
    [SerializeField] private float damage;
    [SerializeField] private float chargeDelay;
    [SerializeField] private float dealDamImmunityDur;
    private bool dealDamImmune;
    private bool canCharge;
    private Rigidbody2D rb;
    [SerializeField] private GameObject detection;
    private GeneralDetectionHitbox detectScript;
    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        detectScript = Instantiate(detection).GetComponentInChildren<GeneralDetectionHitbox>();
        detectScript.SetFollow(transform);
        detectScript.SetRelativeSize(50);
        StartCoroutine(DoChargeCooldown());
        dealDamImmune = false;
    }

    // Update is called once per frame
    void FixedUpdate(){
        Vector3 movDir;
        if(detectScript.HasDetection()){
            movDir = detectScript.GetDetection().position - transform.position;
        }else{
            movDir = MapInformation.RandomLocation() - transform.position;
        }
        if(canCharge){
            rb.AddForce(movDir.normalized * chargeForce * Time.deltaTime);
            StartCoroutine(DoChargeCooldown());
        }
    }
    private IEnumerator DoChargeCooldown(){
        canCharge = false;
        yield return new WaitForSeconds(chargeDelay);
        canCharge = true;
    }

    protected override void OnDamage(){
        return;
    }

    protected override void OnDeath(){
        if(Random.Range(0,2) == 0){
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        Transform otherTran = other.transform;
        if((otherTran.CompareTag("Player") || otherTran.CompareTag("Mini Ship")) && !dealDamImmune){
            otherTran.SendMessage("DamageEntity", damage);
            StartCoroutine(DoPlayerImmunity());
        }
    }
    private IEnumerator DoPlayerImmunity(){
        dealDamImmune = true;
        yield return new WaitForSeconds(dealDamImmunityDur);
        dealDamImmune = false;
    }
}
