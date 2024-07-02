using UnityEngine;
using UnityEngine.Assertions;

public class MapInformation
{
    private static int[] mapSize = new int[]{100,100};
    public static bool IsOutOfBounds(Vector3 pos){
        return Mathf.Abs(pos.x) > mapSize[0] || Mathf.Abs(pos.y) > mapSize[1];
    }
    public static Vector3 MakeInBounds(Vector3 pos){
        return new Vector3(Mathf.Min(Mathf.Max(-mapSize[0],pos.x), mapSize[0]),Mathf.Min(Mathf.Max(-mapSize[1],pos.y), mapSize[1]));
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
        Vector3 randLoc = new Vector3(x,y);
        MakeInBounds(randLoc);
        return randLoc;
    }
    /*
    public static Vector3 RandomLocationAwayFrom(Vector3 pos, float minOffset, float maxOffset){
        Assert.IsTrue(minOffset < maxOffset);
        float x;
        float y;

        if(Random.Range(0,2) == 0){
            x = Random.Range(pos.x+minOffset, pos.x+maxOffset);
        }else{
            x = Random.Range(pos.x-minOffset, pos.x-maxOffset);
        }
        if(Random.Range(0,2) == 0){
            y = Random.Range(pos.y+minOffset, pos.y+maxOffset);
        }else{
            y = Random.Range(pos.y-minOffset, pos.y-maxOffset);
        }
        Vector3 randLoc = new Vector3(x,y);
        if(IsOutOfBounds(randLoc)){
            //correct the invalid axis
            if(Mathf.Abs(randLoc.x) > mapSize[0]){
                int flipDir = (int)(-randLoc.x/randLoc.x);
                randLoc.x -= minOffset*2 * flipDir; 
            }
            if(Mathf.Abs(randLoc.y) > mapSize[1]){
                int flipDir = (int)(-randLoc.y/randLoc.y);
                randLoc.y -= minOffset*2 * flipDir; 
            }
        }
        return randLoc;
    }
    */
}
