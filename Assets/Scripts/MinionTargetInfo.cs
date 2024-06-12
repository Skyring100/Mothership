using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionTargetInfo
{
    private Transform target;
    private Transform leader;
    public MinionTargetInfo(Transform t, Transform l){
            target = t;
            leader = l;
    }
    public Transform GetTarget(){
        return target;
    }
    public Transform GetLeader(){
        return leader;
    }
}
