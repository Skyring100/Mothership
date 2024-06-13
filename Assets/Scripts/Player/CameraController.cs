using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private Camera cam;
    [SerializeField] private float zoomOutRate;
    [SerializeField] private float maxZoomOut;
    [SerializeField] private float zoomInRate;
    [SerializeField] private float baseZoom;
    [SerializeField] private SpriteRenderer background;
    private Vector3 lastPos;
    [SerializeField] private float backgroundSpeed;
    
    private void Start(){
        cam = GetComponent<Camera>();
        lastPos = transform.position;
    }
    private void FixedUpdate(){
        if(Input.GetKey(KeyCode.Space)){
            //zoom out camera
            if(cam.orthographicSize < maxZoomOut){
                cam.orthographicSize += zoomOutRate;
                if(cam.orthographicSize > maxZoomOut){
                    cam.orthographicSize = maxZoomOut;
                }
            }
        }else{
            //if no longer pressing, return to normal zoom
            if(cam.orthographicSize != baseZoom){
                cam.orthographicSize -= zoomInRate;
                if(cam.orthographicSize < baseZoom){
                    cam.orthographicSize = baseZoom;
                }
            }
        }
        //moving the background with the player
        if(!transform.position.Equals(lastPos)){
            Vector2 dir = transform.position - lastPos;
            background.material.mainTextureOffset += dir * Time.deltaTime * backgroundSpeed; 
        }
        lastPos = transform.position;
    }
}
