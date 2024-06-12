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
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
