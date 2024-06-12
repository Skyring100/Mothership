using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInformation
{
    private static int[] mapSize = new int[]{100,100};
    private static System.Random randGen = new System.Random();
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
        return new Vector3(randGen.Next(-mapSize[0], mapSize[0]), randGen.Next(-mapSize[1], mapSize[1]));
    }
}
