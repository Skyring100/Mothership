using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatBarInterations : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private StatsUIController crtler; 
    private Color color;
    private void Start() {
        crtler = GameObject.Find("Player Stats UI").GetComponent<StatsUIController>();
        color = transform.GetChild(0).GetComponent<Image>().color;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        crtler.ShowStatText(gameObject.name, color);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        crtler.HideStatText();
    }
}
