using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour{

    private Camera cam;
    private Transform player;
    [SerializeField] private float zoomRate;
    [SerializeField] private float maxZoomOut;
    [SerializeField] private float baseZoom;
    [SerializeField] private SpriteRenderer background;
    private Vector3 lastPos;
    [SerializeField] private float backgroundSpeed;
    [SerializeField] private bool isZoomedOut;
    
    private void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GetComponent<Camera>();
        lastPos = transform.position;
        isZoomedOut = false;
        cam.orthographicSize = baseZoom;
    }
    private void Update(){
        if(!player.IsDestroyed()){
            transform.position = new Vector3(player.position.x, player.position.y, -10);
        }
        if(Input.GetKeyDown(KeyBindings.GetKeybind("Toggle Zoom"))){
            isZoomedOut = !isZoomedOut;
        }
        if(isZoomedOut){
            //zoom out camera
            if(cam.orthographicSize < maxZoomOut){
                cam.orthographicSize += zoomRate * Time.deltaTime;
                if(cam.orthographicSize > maxZoomOut){
                    cam.orthographicSize = maxZoomOut;
                }
            }
        }else{
            //return to normal zoom
            if(cam.orthographicSize != baseZoom){
                cam.orthographicSize -= zoomRate * Time.deltaTime;
                if(cam.orthographicSize < baseZoom){
                    cam.orthographicSize = baseZoom;
                }
            }
        }
        //moving the background with the player
        if(!transform.position.Equals(lastPos)){
            Vector2 dir = transform.position - lastPos;
            background.material.mainTextureOffset += backgroundSpeed * Time.deltaTime * dir; 
        }
        lastPos = transform.position;
    }

}