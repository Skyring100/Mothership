using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour{
    public void StartButtonClick(){
        SceneManager.LoadScene("Space Field");
    }
}