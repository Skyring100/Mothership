using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private float backgroundSpeed;
    private void Update(){
        background.material.mainTextureOffset += backgroundSpeed * Time.deltaTime * new Vector2(1, 1);
    }
}
