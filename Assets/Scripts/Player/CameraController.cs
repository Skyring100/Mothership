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
    private Vector2 lastPos;
    
    private void Start(){
        cam = GetComponent<Camera>();
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
    }
}
