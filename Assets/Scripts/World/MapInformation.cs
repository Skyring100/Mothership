using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInformation
{
    private static int[] mapSize = new int[]{100,100};
    public static bool IsOutOfBounds(Vector3 pos){
        return pos.x > mapSize[0] || pos.x < -mapSize[0] || pos.y > mapSize[1] || pos.y < -mapSize[1];
    }
    public static int GetMaxX(){
        return mapSize[0];
    }
    public static int GetMaxY(){
        return mapSize[1];
    }
    public static Vector3 RandomLocation(){
        return new Vector3(Random.Range(-mapSize[0], mapSize[0]), Random.Range(-mapSize[1], mapSize[1]));
    }
    public static Vector3 RandomLocationAround(Vector3 aroundThis, float offset){
        float x = Random.Range(aroundThis.x-offset, aroundThis.x+offset);
        float y = Random.Range(aroundThis.y-offset, aroundThis.y+offset);
        //make sure these x and y coordinates don't go out of bounds
        x = Mathf.Min(Mathf.Max(-mapSize[0],x), mapSize[0]);
        y = Mathf.Min(Mathf.Max(-mapSize[1],y), mapSize[1]);
        return new Vector3(x,y);
    }
}
