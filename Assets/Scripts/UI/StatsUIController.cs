using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour{
    [SerializeField] private float infoBarFullSize;
    //health bar
    [SerializeField] private RectTransform healthBar;
    private RectTransform healthBarValue;
    private bool doHealthBarAnim;
    [SerializeField] private float healthBarAnimSize;
    [SerializeField] private float healthBarAnimRate;
    //mini ship bar
    [SerializeField] private RectTransform miniShipBar;
    private RectTransform miniShipBarValue;
    private TextMeshProUGUI miniShipText;
    private bool doMiniShipBarAnim;
    [SerializeField] private float miniShipBarAnimSize;
    [SerializeField] private float miniShipBarAnimRate;
    //stats tab
    [SerializeField] private RectTransform statsTab;
    [SerializeField] private RectTransform[] stats;
    [SerializeField] private float hideStatsCountdown;
    [SerializeField] private GameObject statPopupPanel;
    private TextMeshProUGUI statText;
    private IEnumerator hideStatTimer;
    //screen effects
    [SerializeField] private RectTransform warningScreen; 
    [SerializeField] private Image blackoutScreen;
    private bool doBlackout;
    [SerializeField] private float blackoutRate;
    [SerializeField] private RectTransform deathScreen;
    private void Awake(){
        healthBarValue = healthBar.GetChild(0).GetComponent<RectTransform>();
        miniShipBarValue = miniShipBar.GetChild(0).GetComponent<RectTransform>();
        miniShipText = miniShipBar.GetChild(1).GetComponent<TextMeshProUGUI>();
        HideStats();
        HideStatText();
        statText = statPopupPanel.GetComponentInChildren<TextMeshProUGUI>();
        hideStatTimer = null;
        warningScreen.gameObject.SetActive(false);
        blackoutScreen.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(false);
    }
    //Check if there are animations to do
    private void Update(){
        if(doHealthBarAnim){
            doHealthBarAnim = BounceBarOut(healthBar, healthBarAnimRate, healthBarAnimSize);
        }else{
            BounceBarIn(healthBar, healthBarAnimRate);
        }
        if(doMiniShipBarAnim){
            doMiniShipBarAnim = BounceBarOut(miniShipBar, miniShipBarAnimRate, miniShipBarAnimSize);
        }else{
            BounceBarIn(miniShipBar, miniShipBarAnimRate);
        }
        if(doBlackout){
            Color blackoutShade = blackoutScreen.color;
            blackoutShade.a -= blackoutRate * Time.deltaTime;
            blackoutScreen.color = blackoutShade;
            if(blackoutShade.a <= 0){
                doBlackout = false;
                blackoutScreen.gameObject.SetActive(false);
            }
        }
    }
    private bool BounceBarOut(RectTransform bar, float growRate, float maxSize){
        if(bar.localScale.x < maxSize){
            bar.localScale += new Vector3(growRate,growRate) * Time.deltaTime;
            return true;
        }else if(bar.localScale.x >= maxSize){
            return false;
        }
        return true;
    }
    private void BounceBarIn(RectTransform bar, float growRate){
        if(bar.localScale.x != 1){
            bar.localScale -= new Vector3(growRate,growRate) * Time.deltaTime;
            if(bar.localScale.x < 1){
                bar.localScale = new Vector3(1,1);
            }
        }
    }
    public void HealthBarAnimation(){
        doHealthBarAnim = true;
    }
    public void MiniShipBarAnimation(){
        doMiniShipBarAnim = true;
    }
    public void ChangePlayerHealth(float healthPercentage, bool doAnimation){
        float newHealthSize = infoBarFullSize * healthPercentage;
        if(newHealthSize < 0){
            newHealthSize = 0;
        }
        healthBarValue.localScale = new Vector3(newHealthSize, healthBarValue.localScale.y);
        if(doAnimation){
            HealthBarAnimation();
        }
    }
    public void ChangePlayerHealth(float healthPercentage){
        ChangePlayerHealth(healthPercentage, true);
    }
    public void ChangeShipCount(int currentShips, int maxShips){
        float ratio = 1-(currentShips/((float)maxShips));
        miniShipBarValue.localScale = new Vector3(infoBarFullSize * ratio,0.5f);
        miniShipText.text = maxShips-currentShips+"";
        miniShipText.color = Color.white;
        //recolor the text if the player has all miniship available
        if(currentShips == 0){
            miniShipText.color = Color.yellow;
        }
        MiniShipBarAnimation();
    }
    public void ShowStats(){
        statsTab.gameObject.SetActive(true);
        CancelStatHideTimer();
    }
    public void HideStats(){
        statsTab.gameObject.SetActive(false);
        HideStatText();
    }
    public void ChangeStat(float statPercentage, int statIndex){
        float newStatSize = infoBarFullSize * statPercentage;
        RectTransform statBar = stats[statIndex];
        statBar.localScale = new Vector3(newStatSize, statBar.localScale.y);
        //if the player is not looking at stats and we pop it up suddenly, the panel will only show for a brief period to not intefere with gameplay
        bool showingStats = IsShowingStats();
        ShowStats();
        if(!showingStats){
            RestartStatHideTimer();
        }
    }
    private IEnumerator HideCountDown(){
        yield return new WaitForSeconds(hideStatsCountdown);
        HideStats();
        hideStatTimer = null;
    }
    public bool IsShowingStats(){
        return statsTab.gameObject.activeSelf;
    }
    public void ShowStatText(string text, Color c){
        statText.text = text;
        statText.color = c;
        statPopupPanel.SetActive(true);
    }
    public void HideStatText(){
        statPopupPanel.SetActive(false);
    }
    private void RestartStatHideTimer(){
        if(hideStatTimer != null){
            StopCoroutine(hideStatTimer);
        }
        hideStatTimer = HideCountDown();
        StartCoroutine(hideStatTimer);
    }
    private void CancelStatHideTimer(){
        if(hideStatTimer != null){
            StopCoroutine(hideStatTimer);
        }
    }
    public void ShowWarningScreen(){
        warningScreen.gameObject.SetActive(true);
    }
    public void HideWarningScreen(){
        warningScreen.gameObject.SetActive(false);
    }
    public void DoBlackout(){
        Color blackoutShade = blackoutScreen.color;
        blackoutShade.a = 1;
        blackoutScreen.color = blackoutShade;
        blackoutScreen.gameObject.SetActive(true);
        doBlackout = true;
    }
    public void ShowDeathScreen(){
        deathScreen.gameObject.SetActive(true);
    }
}
