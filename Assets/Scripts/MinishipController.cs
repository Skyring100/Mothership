using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MinishipController : HealthSystem{
    [SerializeField] private GameObject powerupPrefab;
    [SerializeField] private float planetHoverDist;
    [SerializeField] private float enemyShipHoverDist;
    [SerializeField] private float movSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float shootDelay;
    [SerializeField] private MinishipBullet bulletPrefabScript;
    private bool canShoot;
    private float destinationOffset;
    private Transform leader;
    private Stack<Transform> objectives = new Stack<Transform>();
    private Stack<float> objectiveOffsets = new Stack<float>();
    private Rigidbody2D rb;
    private Camera cam;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
        //scale the drag with the movement speed so the ship doesn't fly out of control
        rb.drag = movSpeed/250;
        canShoot = true;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void FixedUpdate(){
        bool doneTask = false;
        Vector3 targetPos;
        //check if the mouse is being pressed down
        if(CompareTag("Mini Ship") && Input.GetMouseButton(0)){
            destinationOffset = 0;
            targetPos = cam.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
        }else if(objectives.Count != 0){
            //check if there is an objective
            Transform objectiveTransform = objectives.Peek();
            //check if the target exists still
            if(objectiveTransform != null && !objectiveTransform.IsDestroyed()){
                targetPos = objectiveTransform.position;
            }else{
                //find a target next frame if destroyed
                objectives.Pop();
                destinationOffset = objectiveOffsets.Pop();
                return;
            }
        }else{
            //if there is no objective, return home
            //but first check if the leader exists still
            if(leader == null || leader.gameObject.IsDestroyed()){
                NoLeaderSulk();
                return;
            }
            targetPos = leader.position;
            destinationOffset = 0;
            doneTask = true;
        }
        //calculate the direction to the target
        Vector3 targetDir = targetPos - transform.position;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, targetDir);

        //check if this ship is within range of its destination
        if(targetDir.magnitude > destinationOffset){
            //continue to fly towards the target
            rb.AddForce(targetDir.normalized * movSpeed * Time.deltaTime);
        }else{
            //check if this ship is too close to target
            if(targetDir.magnitude < destinationOffset/1.75f){
                //back away from the target
                float extraForce = destinationOffset - targetDir.magnitude;
                rb.AddForce(targetDir.normalized * -movSpeed*extraForce * Time.deltaTime);
            }
        }
        //if we are going to the leader, this mini ship doesn't need to shoot
        if(!doneTask){
            if(canShoot){
                Instantiate(bulletPrefab, transform.position, transform.rotation);
                StartCoroutine(DoShootDelay());
            }
        }
    }
    public void StartObjective(MinionTargetInfo info){
        objectives.Push(info.GetTarget());
        leader = info.GetLeader();
        Transform tar = info.GetTarget();
        switch(tar.tag){
            case "Planet":
                destinationOffset = planetHoverDist * Mathf.Max(tar.lossyScale.x, tar.lossyScale.y);
                break;

            case "Player":
                destinationOffset = planetHoverDist;
                break;

            case "Mini Ship":
            case "Enemy":
                destinationOffset = enemyShipHoverDist;
                break;

            default:
                destinationOffset = 0;
                break;
        }
        objectiveOffsets.Push(destinationOffset);
        //Debug.Log("Leader is "+leader.gameObject.name);
    }
    public void ClearObjectives(){
        objectives.Clear();
        objectiveOffsets.Clear();
    }
    public int ObjectiveCount(){
        return objectives.Count;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        TryDocking(other.transform);
    }
    private void OnTriggerStay2D(Collider2D other) {
        TryDocking(other.transform);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        TryDocking(other.transform);
    }
    private void TryDocking(Transform other){
        //return to the leader if no more objectives
        if(other.Equals(leader) && objectives.Count == 0){
            //if owned by player, make sure mouse isnt being pressed
            if(!CompareTag("Mini Ship") || !Input.GetMouseButton(0)){
                leader.SendMessage("ShipReturned", this);
            }
        }
    }
    private void NoLeaderSulk(){
        
    }
    public void SetSpeed(float s){
        movSpeed = s;
    }
    public void SetDamage(float d){
        bulletPrefabScript.SetDamage(d);
    }
    public void SetBulletSpeed(float s){
        bulletPrefabScript.SetSpeed(s);
    }
    public void SetShootDelay(float d){
        shootDelay = d;
    }
    private IEnumerator DoShootDelay(){
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    protected override void OnDamage(){
        return;
    }

    protected override void OnDeath(){
        if(leader != null && !leader.IsDestroyed()){
            leader.SendMessage("ShipReturned", this);
        }else{
            Destroy(gameObject);
        }
        if(CompareTag("Enemy") && Random.Range(0,5) == 0){
            Instantiate(powerupPrefab, transform.position, Quaternion.identity);
        }
    }
}
