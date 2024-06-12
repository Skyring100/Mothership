using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneralDetectionHitbox : MonoBehaviour
{
    private Transform followThis;
    [SerializeField] private string[] detectTags;
    private Transform currentDetection;
    private bool hasDetection;
    private void Start() {
        hasDetection = false;
    }
    private void Update() {
        if(followThis != null){
            if(!followThis.IsDestroyed()){
                transform.position = followThis.position;
            }else{
                Destroy(gameObject);
            }
        }
    }
    public void SetFollow(Transform t){
        followThis = t;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        //if not currently detecting something, check this detection
        if(!hasDetection){
            hasDetection = DetectionCheck(other.transform);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        //check if our detected thing is the one leaving
        if(hasDetection && other.transform.Equals(currentDetection)){
            hasDetection = false;
        }
    }
    private void OnTriggerStay(Collider other) {
        if(!hasDetection){
            hasDetection = DetectionCheck(other.transform);
        }
    }
    private bool DetectionCheck(Transform other){
        for(int i = 0; i < detectTags.Length; i++){
            if(other.CompareTag(detectTags[i])){
                currentDetection = other.transform;
                return true;
            }
        }
        return false;
    }
    public bool HasDetection(){
        return hasDetection;
    }
    public Transform GetDetection(){
        return currentDetection;
    }
    public void SetRelativeSize(float size, Transform t){
        if(followThis != null){
            transform.localScale =  t.localScale * size;
        }else{
            Debug.LogError("Detection relative size set before given an owner");
        }
    }
    public void SetRelativeSize(float size){
        SetRelativeSize(size, followThis);
    }
}
