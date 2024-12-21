using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AllSeeingEyeAI : MonoBehaviour{
    private Transform eyeWhite;
    private Transform pupil;
    [SerializeField] private GameObject detectionPrefab;
    [SerializeField] private GameObject viewingSpotPrefab;
    private GeneralDetectionHitbox personalDetection;
    private GeneralDetectionHitbox viewingSpot;
    // Start is called before the first frame update
    void Start(){
        eyeWhite = transform.GetChild(0).transform;
        pupil = eyeWhite.GetChild(0).transform;    
        personalDetection = Instantiate(detectionPrefab,transform.position, Quaternion.identity).GetComponent<GeneralDetectionHitbox>();
        personalDetection.SetFollow(transform);
        personalDetection.SetRelativeSize(2);
        viewingSpot = Instantiate(viewingSpotPrefab, MapInformation.RandomLocation(), Quaternion.identity).GetComponent<GeneralDetectionHitbox>();
    }

    // Update is called once per frame
    void Update(){
        if(personalDetection.HasDetection()){
            SuprisedVisual();
        }else{
            NormalVisual();
        }
        if(viewingSpot.HasDetection()){
            
        }
    }
    private void SuprisedVisual(){
        pupil.transform.localScale = new Vector3(0.05f,0.9f);
    }
    private void NormalVisual(){
        pupil.transform.localScale = new Vector3(0.31f, 0.9f);
    }
}
