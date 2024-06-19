using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MothershipController : HealthSystem
{
    /*
    TODO
    -Out of bounds change
    -Make mini ships auto target enemies
    -add visuals for when an entitiy is damaged
    -Powerup drops to more enemies
    -Planet variety
    -Add more difficult enemies
    */
    private Rigidbody2D rb;
    private Camera cam;
    [SerializeField] private GameObject miniShipPrefab;
    private MinishipController miniShipPrefabScript;
    [SerializeField] private float miniSpawnOffset;
    [SerializeField] private List<MinishipController> miniShipsExisting = new List<MinishipController>();
    [SerializeField] private float regenRate;
    //current stats
    [SerializeField] private float movSpeed;
    [SerializeField] private int miniShipsLimit;
    [SerializeField] private float miniHealth;
    [SerializeField] private float miniSpeed;
    [SerializeField] private float miniDamage;
    [SerializeField] private float miniBulletSpeed;
    [SerializeField] private float miniShootDelay;
    //starting stats
    [SerializeField] public float startMovSpeed = 500;
    [SerializeField] public int startMiniShips = 3;
    [SerializeField] public float startMaxHealth = 20;
    [SerializeField] public float startMiniHealth = 2;
    [SerializeField] public float startMiniSpeed = 125;
    [SerializeField] public float startMiniDamage = 2;
    [SerializeField] public float startMiniBulletSpeed = 5;
    [SerializeField] public float startMiniShootDelay = 2;
    //max stats
    [SerializeField] public float maxMaxHealth;
    [SerializeField] public float maxMovSpeed;
    [SerializeField] public int maxMiniShips;
    [SerializeField] public float maxMiniHealth;
    [SerializeField] public float maxMiniSpeed;
    [SerializeField] public float maxMiniDamage;
    [SerializeField] public float maxMiniBulletSpeed;
    [SerializeField] public float maxMiniShootDelay;

    [SerializeField] private GameObject goHereMarkerPrefab;
    [SerializeField] private StatsUIController statsUI;
    [SerializeField] private Transform landingDocs;
    private void Awake() {
        Physics2D.queriesHitTriggers = false;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GetComponentInChildren<Camera>();
        miniShipPrefabScript = miniShipPrefab.GetComponent<MinishipController>();
        
        SetMaxHealth(startMaxHealth);

        movSpeed = startMovSpeed;
        miniShipsLimit = startMiniShips;
        miniHealth = startMiniHealth;
        miniSpeed = startMiniSpeed;
        miniDamage = startMiniDamage;
        miniBulletSpeed = startMiniBulletSpeed;
        miniShootDelay = startMiniShootDelay;

        miniShipPrefabScript.SetMaxHealth(miniHealth);
        miniShipPrefabScript.SetSpeed(miniSpeed);
        miniShipPrefabScript.SetDamage(miniDamage);
        miniShipPrefabScript.SetBulletSpeed(miniBulletSpeed);
        miniShipPrefabScript.SetShootDelay(miniShootDelay);
        statsUI.ChangeShipCount(miniShipsExisting.Count, miniShipsLimit);
        InvokeRepeating("DoPassiveRegen",regenRate, regenRate);
    }

    void Update()
    {
        DoMovement();
        ClickCheck();
        if(Input.GetKeyDown(KeyCode.E)){
            if(miniShipsExisting.Count < miniShipsLimit){
                 Vector3 mouseLoc = cam.ScreenToWorldPoint(Input.mousePosition);
                mouseLoc.z = 0;
                Transform marker = Instantiate(goHereMarkerPrefab, mouseLoc, Quaternion.identity).transform;
                SpawnMiniship(marker);
            }else{
                statsUI.MiniShipBarAnimation();
            }
        }
        if(Input.GetKeyDown(KeyCode.R)){
            foreach(MinishipController script in miniShipsExisting){
                script.ClearObjectives();
            }
        }
        if(Input.GetKeyDown(KeyCode.V)){
            StatsUIToggle();
        }
        if(Input.GetKeyDown(KeyCode.T)){
            ExplodeAllShips();
        }
        OutOfBoundsCheck();
    }

    private void DoMovement(){
        float horz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horz, vert, 0);
        rb.AddForce(direction * movSpeed * Time.deltaTime);
    }
    private void ClickCheck(){
        Vector3 mouseLoc = cam.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButtonDown(1)){
            //get what the mouse clicked on
            RaycastHit2D detections = Physics2D.Raycast(mouseLoc, Vector2.down);
            if(detections){
                //Debug.Log("You clicked on "+detections.collider.gameObject.name);
                if(!Input.GetKey(KeyCode.LeftShift)){
                    if(detections.transform.CompareTag("Planet") || detections.transform.CompareTag("Enemy")){
                        SpawnMiniship(detections.transform);
                    }
                }else{
                    //clicked on something thats minable or an enemy
                    if(detections.transform.CompareTag("Planet") || detections.transform.CompareTag("Enemy")){
                        //use all miniships to target what the player clicked on
                        foreach(MinishipController script in miniShipsExisting){
                            script.StartObjective(new MinionTargetInfo(detections.transform, landingDocs));
                        }
                    }else if(detections.transform.CompareTag("Marker")){
                        Destroy(detections.transform.gameObject);
                    }
                }
            }
        }
    }
    private void StatsUIToggle(){
        if(!statsUI.IsShowingStats()){
            statsUI.ShowStats();
        }else{
            statsUI.HideStats();
        }
    }
    private void DoPassiveRegen(){
        HealEntity(1);
        statsUI.ChangePlayerHealth(GetHealth()/GetMaxHealth(), false);
    }
    private void OutOfBoundsCheck(){
        if(MapInformation.IsOutOfBounds(transform.position)){
            Debug.Log("You are out of bounds");
            float offsetMultiplier = 0.9f;
            float newX = transform.position.x;
            if(Mathf.Abs(newX) > MapInformation.GetMaxX()){
                newX *= -1;
            }
            float newY = transform.position.y;
            if(Mathf.Abs(newY) > MapInformation.GetMaxY()){
                newY *= -1;
            }

            transform.position = new Vector3(newX*offsetMultiplier, newY*offsetMultiplier);
        }
    }
    public void ShipReturned(MinishipController s){
        bool ownsShip = false;
        foreach(MinishipController script in miniShipsExisting){
            if(script.name.Equals(s.name)){
                ownsShip = true;
                break;
            }
        }
        if(ownsShip){
            miniShipsExisting.Remove(s);
            Destroy(s.gameObject);
        }
        //update the UI
        statsUI.ChangeShipCount(miniShipsExisting.Count, miniShipsLimit);
    }
    private void SpawnMiniship(Transform miniShipTarget){
        if(miniShipsExisting.Count < miniShipsLimit){
            MinishipController spawnedShipScript = Instantiate(miniShipPrefab, new Vector2(transform.position.x, transform.position.y + miniSpawnOffset), transform.rotation).GetComponent<MinishipController>();
            spawnedShipScript.StartObjective(new MinionTargetInfo(miniShipTarget, landingDocs));
            miniShipsExisting.Add(spawnedShipScript);
            statsUI.ChangeShipCount(miniShipsExisting.Count, miniShipsLimit);
        }else{
            statsUI.MiniShipBarAnimation();
        }
    }
    private void ExplodeAllShips(){
        foreach(MinishipController script in miniShipsExisting){
            Destroy(script.gameObject);
        }
        miniShipsExisting.Clear();
        statsUI.ChangeShipCount(0, miniShipsLimit);
    }
    protected override void OnDamage()
    {
        statsUI.ChangePlayerHealth(GetHealth()/GetMaxHealth());
    }

    protected override void OnDeath()
    {
        Debug.Log("You died");
    }

    public void ChangeMotherHealth(float h){
        SetMaxHealth(GetMaxHealth()+h);
        statsUI.ChangePlayerHealth(GetHealth()/GetMaxHealth());
    }
    public void ChangeMotherSpeed(float s){
        movSpeed += s;
        statsUI.ChangeStat(CalcStatRatio(startMovSpeed, movSpeed, maxMovSpeed), 0);
    }
    public void ChangeMaxShips(int ships){
        miniShipsLimit += ships;
        statsUI.ChangeStat(CalcStatRatio(startMiniShips, miniShipsLimit, maxMiniShips), 1);
        statsUI.ChangeShipCount(miniShipsExisting.Count, miniShipsLimit);
    }
    public void ChangeMiniHealth(float v){
        miniHealth += v;
        miniShipPrefabScript.SetMaxHealth(miniHealth);
        statsUI.ChangeStat(CalcStatRatio(startMiniHealth, miniHealth, maxMiniHealth), 2);
    }
    public void ChangeMiniSpeed(float v){
        miniSpeed += v;  
        miniShipPrefabScript.SetSpeed(miniSpeed);
        statsUI.ChangeStat(CalcStatRatio(startMiniSpeed, miniSpeed, maxMiniSpeed), 3);
    }
    public void ChangeMiniDamage(float v){
        miniDamage += v;
        miniShipPrefabScript.SetDamage(miniDamage);
        statsUI.ChangeStat(CalcStatRatio(startMiniDamage, miniDamage, maxMiniDamage), 4);
    }
    public void ChangeMiniBulletSpeed(float v){
        miniBulletSpeed += v;
        miniShipPrefabScript.SetBulletSpeed(miniBulletSpeed);
        statsUI.ChangeStat(CalcStatRatio(startMiniBulletSpeed, miniBulletSpeed, maxMiniBulletSpeed), 5);
    }
    public void ChangeMiniShootDelay(float v){
        Debug.Log("Change by "+v);
        miniShootDelay -= v;
        miniShipPrefabScript.SetShootDelay(miniShootDelay);
        float ratio = CalcStatRatio(startMiniShootDelay, miniShootDelay, 1/maxMiniShootDelay);
        statsUI.ChangeStat(ratio, 6);
    }
    private float CalcStatRatio(float starting,float current, float max){
        Debug.Log("Starting: "+starting+" Current: "+current+" Max: "+max);
        return (current-starting)/max;
    }
}
