using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour{
    private GameObject primaryButtonsScreen;
    private GameObject controlsScreen;
    [SerializeField] private GameObject menuButtonPrefab;
    private bool keybindDescriptAdded;
    private void Start(){
        primaryButtonsScreen = transform.GetChild(0).gameObject;
        controlsScreen = transform.GetChild(1).gameObject;
        keybindDescriptAdded = false;
        BackToMenu();
    }
    public void StartButtonClick(){
        SceneManager.LoadScene("Space Field");
        if(!keybindDescriptAdded){
            KeyBindings.DefaultKeybinds();
        }
    }
    public void ControlButtonClick(){
        primaryButtonsScreen.SetActive(false);
        controlsScreen.SetActive(true);
        if(!keybindDescriptAdded){
            KeyBindings.DefaultKeybinds();
            int nextYPos = 470;
            foreach(KeyValuePair<string, KeyCode> key in KeyBindings.keyBindings){
                GameObject button = Instantiate(menuButtonPrefab);
                button.transform.SetParent(controlsScreen.transform, false);
                button.transform.localPosition = new Vector3(0, nextYPos, 0);
                button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = key.Key +"\n"+key.Value;
                nextYPos -= 150;
            }
            keybindDescriptAdded = true;
        }
    }
    public void BackToMenu(){
        primaryButtonsScreen.SetActive(true);
        controlsScreen.SetActive(false);
    }
}
