using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RogueShipController : HealthSystem
{
    [SerializeField] private float raycastOffset;
    private Transform target;
    [SerializeField] private float detectionDistance;
    [SerializeField] private float loseIntrestModifer;
    [SerializeField] private float thrusterStrength;
    [SerializeField] private float wanderThrustDelay;
    [SerializeField] private float chaseThrustDelay;
    private bool thrusterReady;
    private Vector3 wanderLocation;
    private bool seesTarget;
    private Rigidbody2D rb;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootDelay;
    [SerializeField] private float bulletSpawnOffset;
    private Transform leftWing;
    private Transform rightWing;
    private bool canShoot;
    private void Start() {
        wanderLocation = MapInformation.RandomLocation();
        rb = GetComponent<Rigidbody2D>();
        thrusterReady = true;
        canShoot = true;
        Transform[] wingTran = GetComponentsInChildren<Transform>();
        leftWing = wingTran[0];
        rightWing = wingTran[1];
    }

    // Update is called once per frame
    void Update()
    {
        if(!seesTarget){
            //wander around
            DoWander();
            //check for a target
            seesTarget = CheckVision();
        }else{
            //check if the enemy has escaped detection range
            seesTarget = PlayerInDetectRange();
            //if the enemy is still in range, continue to move forward and attack
            if(seesTarget){
                //Debug.Log("Still see "+target.name+" who is at "+target.position);
                TryThrust(target.position - transform.position);
                TryShoot();
            }
        }
    }
    private bool CheckVision(){
        //set up a raycast
        Vector3 upDir = transform.TransformDirection(Vector3.up);
        Vector3 startRay = transform.position+upDir*raycastOffset;
        Debug.DrawRay(startRay, upDir*detectionDistance);
        RaycastHit2D lookInFront = Physics2D.Raycast(startRay, upDir, detectionDistance);
        //check if the raycast hit anything
        if(lookInFront && !lookInFront.transform.name.Equals(gameObject.name)){
            //check if the ship sees an enemy
            if(lookInFront.transform.CompareTag("Player") || lookInFront.transform.CompareTag("Mini Ship") || lookInFront.transform.CompareTag("Enemy")){
                target = lookInFront.transform;
                return true;
            }
        }
        return false;
    }
    private bool PlayerInDetectRange(){
        //check if target has been destroyed first
        if(target.IsDestroyed()){
            return false;
        }
        Vector3 distance = target.position - transform.position;
        if(distance.magnitude > detectionDistance*loseIntrestModifer){
            return false;
        }else{
            return true;
        }
    }
    private void DoWander(){
        if((int)transform.position.x != (int)wanderLocation.x && (int)transform.position.y != (int)wanderLocation.y){
            //continue moving to location
            TryThrust(wanderLocation - transform.position);
        }else{
            //find a new location
            wanderLocation = MapInformation.RandomLocation();
        }
    }
    private void TryThrust(Vector3 direction){
        LookAtDir(direction);
        if(thrusterReady){
            rb.AddForce(direction.normalized * thrusterStrength * Time.deltaTime);
            StartCoroutine(DoThruserDelay());
        }
    }
    private IEnumerator DoThruserDelay(){
        thrusterReady = false;
        if(seesTarget){
            yield return new WaitForSeconds(chaseThrustDelay);
        }else{
            yield return new WaitForSeconds(wanderThrustDelay);
        }
        thrusterReady = true;
    }
    private void TryShoot(){
        if(canShoot){
            //shoot from the two wings on the rogue ship
            Instantiate(bulletPrefab, leftWing.position+transform.up*bulletSpawnOffset, transform.rotation);
            Instantiate(bulletPrefab, rightWing.position+transform.up*bulletSpawnOffset, transform.rotation);
            StartCoroutine(DoShootDelay());
        }
    }
    private IEnumerator DoShootDelay(){
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }
    private void LookAtDir(Vector3 dir){
        transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
    }

    protected override void OnDamage()
    {
        return;
    }

    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
