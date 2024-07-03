using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MothershipController : HealthSystem
{
    /*
    TODO
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
    [SerializeField] private PlayerUIController ui;
    [SerializeField] private Transform landingDocs;
    [SerializeField] private string[] targetableTags;
    private bool currentlyOutOfBounds;
    [SerializeField] private float outOfBoundsTimer;
    private void Awake() {
        Physics2D.queriesHitTriggers = false;
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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
        ui.ChangeShipCount(miniShipsExisting.Count, miniShipsLimit);
        InvokeRepeating("DoPassiveRegen",regenRate, regenRate);

        currentlyOutOfBounds = false;
    }

    void Update()
    {
        DoMovement();
        ClickCheck();
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
        if(Input.GetMouseButtonDown(0)){
            //get what the mouse clicked on
            RaycastHit2D detections = Physics2D.Raycast(mouseLoc, Vector3.back);
            //check if there is either nothing clicked or the clicked object is not targetable
            if(!detections || !IsTargetable(detections.transform)){
                //if nothing valid was clicked on, simply send a ship over to that location
                mouseLoc.z = 0;
                Transform marker = Instantiate(goHereMarkerPrefab, mouseLoc, Quaternion.identity).transform;
                bool didSpawn = SpawnMiniship(marker);
                if(!didSpawn){
                    Destroy(marker.gameObject);
                }
            }else{
                //send the ship to attack the target
                SpawnMiniship(detections.transform);
            }
        }
        if(Input.GetMouseButtonDown(1)){
            //get what the mouse clicked on
            RaycastHit2D detections = Physics2D.Raycast(mouseLoc, Vector3.back);
            if(detections && IsTargetable(detections.transform)){
                //use all miniships to target what the player clicked on
                foreach(MinishipController script in miniShipsExisting){
                    script.StartObjective(new MinionTargetInfo(detections.transform, landingDocs));
                }
            }
        }
    }
    private bool IsTargetable(Transform target){
        foreach(string tag in targetableTags){
            if(target.CompareTag(tag)){
                return true;
            }
        }
        return false;
    }
    private void StatsUIToggle(){
        if(!ui.IsShowingStats()){
            ui.ShowStats();
        }else{
            ui.HideStats();
        }
    }
    private void DoPassiveRegen(){
        HealEntity(1);
        ui.ChangePlayerHealth(GetHealth()/GetMaxHealth(), false);
    }
    private void OutOfBoundsCheck(){
        if(MapInformation.IsOutOfBounds(transform.position)){
            //check if this is the first time the player has gone out of bounds
            if(!currentlyOutOfBounds){
                currentlyOutOfBounds = true;
                ui.ShowWarningScreen();
                StartCoroutine("OutOfBoundsCountdown");
            }
        }else{
            //the player is no longer out of bounds so update accordingly
            if(currentlyOutOfBounds){
                currentlyOutOfBounds = false;
                ui.HideWarningScreen();
                StopCoroutine("OutOfBoundsCountdown");
            }
        }
    }
    private IEnumerator OutOfBoundsCountdown(){
        yield return new WaitForSeconds(outOfBoundsTimer);
        MoveInBounds();
        ui.HideWarningScreen();
        ui.DoBlackout();
    }
    private void MoveInBounds(){
        float offsetMultiplier = 0.9f;
        float newX = transform.position.x;
        if(Mathf.Abs(newX) > MapInformation.GetMaxX()){
            newX *= -1;
        }
        float newY = transform.position.y;
        if(Mathf.Abs(newY) > MapInformation.GetMaxY()){
            newY *= -1;
        }
        transform.position = MapInformation.MakeInBounds(new Vector3(newX, newY))*offsetMultiplier;
        currentlyOutOfBounds = true;
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
        ui.ChangeShipCount(miniShipsExisting.Count, miniShipsLimit);
    }
    private bool SpawnMiniship(Transform miniShipTarget){
        if(miniShipsExisting.Count < miniShipsLimit){
            MinishipController spawnedShipScript = Instantiate(miniShipPrefab, new Vector2(transform.position.x, transform.position.y + miniSpawnOffset), transform.rotation).GetComponent<MinishipController>();
            spawnedShipScript.StartObjective(new MinionTargetInfo(miniShipTarget, landingDocs));
            miniShipsExisting.Add(spawnedShipScript);
            ui.ChangeShipCount(miniShipsExisting.Count, miniShipsLimit);
            return true;
        }else{
            ui.MiniShipBarAnimation();
            return false;
        }
    }
    private void ExplodeAllShips(){
        foreach(MinishipController script in miniShipsExisting){
            Destroy(script.gameObject);
        }
        miniShipsExisting.Clear();
        ui.ChangeShipCount(0, miniShipsLimit);
    }
    protected override void OnDamage()
    {
        ui.ChangePlayerHealth(GetHealth()/GetMaxHealth());
    }

    protected override void OnDeath()
    {
        Debug.Log("You died");
        Destroy(gameObject);
        ui.DoBlackout();
        ui.ShowDeathScreen();
    }

    public void ChangeMotherHealth(float h){
        SetMaxHealth(GetMaxHealth()+h);
        ui.ChangePlayerHealth(GetHealth()/GetMaxHealth());
    }
    public void ChangeMotherSpeed(float s){
        movSpeed += s;
        ui.ChangeStat(CalcStatRatio(startMovSpeed, movSpeed, maxMovSpeed), 0);
    }
    public void ChangeMaxShips(int ships){
        miniShipsLimit += ships;
        ui.ChangeStat(CalcStatRatio(startMiniShips, miniShipsLimit, maxMiniShips), 1);
        ui.ChangeShipCount(miniShipsExisting.Count, miniShipsLimit);
    }
    public void ChangeMiniHealth(float v){
        miniHealth += v;
        miniShipPrefabScript.SetMaxHealth(miniHealth);
        ui.ChangeStat(CalcStatRatio(startMiniHealth, miniHealth, maxMiniHealth), 2);
    }
    public void ChangeMiniSpeed(float v){
        miniSpeed += v;  
        miniShipPrefabScript.SetSpeed(miniSpeed);
        ui.ChangeStat(CalcStatRatio(startMiniSpeed, miniSpeed, maxMiniSpeed), 3);
    }
    public void ChangeMiniDamage(float v){
        miniDamage += v;
        miniShipPrefabScript.SetDamage(miniDamage);
        ui.ChangeStat(CalcStatRatio(startMiniDamage, miniDamage, maxMiniDamage), 4);
    }
    public void ChangeMiniBulletSpeed(float v){
        miniBulletSpeed += v;
        miniShipPrefabScript.SetBulletSpeed(miniBulletSpeed);
        ui.ChangeStat(CalcStatRatio(startMiniBulletSpeed, miniBulletSpeed, maxMiniBulletSpeed), 5);
    }
    public void ChangeMiniShootDelay(float v){
        miniShootDelay -= v;
        miniShipPrefabScript.SetShootDelay(miniShootDelay);
        ui.ChangeStat(CalcStatRatio(startMiniShootDelay, miniShootDelay, 1/maxMiniShootDelay), 6);
    }
    private float CalcStatRatio(float starting,float current, float max){
        return (current-starting)/max;
    }
}
