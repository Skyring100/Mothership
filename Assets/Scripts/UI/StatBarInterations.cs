using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatBarInterations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    private PlayerUIController crtler; 
    private Color color;
    private static float backgroundModif = 0.2f;
    private void Start() {
        crtler = GameObject.Find("Player Stats UI").GetComponent<PlayerUIController>();
        color = transform.GetChild(0).GetComponent<Image>().color;
        //set the background color to be a darkened version of the stat color
        GetComponent<Image>().color = new Color(color.r*backgroundModif, color.g*backgroundModif, color.b*backgroundModif);
    }
    public void OnPointerEnter(PointerEventData eventData){
        crtler.ShowStatText(gameObject.name, color);
    }

    public void OnPointerExit(PointerEventData eventData){
        crtler.HideStatText();
    }
}
