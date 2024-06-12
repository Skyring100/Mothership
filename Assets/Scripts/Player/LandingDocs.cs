using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingDocs : MonoBehaviour
{
    private MothershipController crtler;
    // Start is called before the first frame update
    void Start()
    {
        crtler = GetComponentInParent<MothershipController>();
    }
    public void ShipReturned(MinishipController c){
        crtler.ShipReturned(c);
    }
}
