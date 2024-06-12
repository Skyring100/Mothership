using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAI : HealthSystem
{
    [SerializeField] private GameObject detection;
    private GeneralDetectionHitbox detectScr;
    private BomberRadius radiusScr;
    [SerializeField] private float speed;
    private Rigidbody2D rb;

    protected override void OnDamage()
    {
        for(int i = 1; i <= 10; i++){
            radiusScr.StopGrow();
        }
    }

    protected override void OnDeath()
    {
        Destroy(gameObject);
    }

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        //set up the detection hitbox
        GameObject d = Instantiate(detection,transform.position, Quaternion.identity); 
        detectScr = d.GetComponent<GeneralDetectionHitbox>();
        detectScr.SetFollow(transform);

        radiusScr = GetComponentInChildren<BomberRadius>();
        //make the detection hitbox the same size as the max raduis for the explosion
        d.transform.localScale = new Vector3(radiusScr.maxRadius, radiusScr.maxRadius);
    }
    private void FixedUpdate() {
        if(detectScr.HasDetection()){
            radiusScr.GrowRadius();
            Vector3 direction = detectScr.GetDetection().position - transform.position;
            rb.AddForce(direction.normalized * speed * Time.deltaTime);
        }else{
            radiusScr.StopGrow();
            rb.velocity = new Vector2(0,0);
        }
    }
}
