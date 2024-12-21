using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour{
    [SerializeField] private Camera cam;

    // Update is called once per frame
    void Update(){
        transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
