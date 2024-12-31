using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindings : MonoBehaviour{
    public static Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    public static void DefaultKeybinds(){
        keyBindings.Add("Deploy Ship", KeyCode.Mouse0);
        keyBindings.Add("All Target", KeyCode.Mouse1);
        keyBindings.Add("Return Ships", KeyCode.R);
        keyBindings.Add("Explode Ships", KeyCode.T);
        keyBindings.Add("Toggle Stats", KeyCode.V);
        keyBindings.Add("Toggle Zoom", KeyCode.Space);
    }
    public static KeyCode GetKeybind(string name){
        return keyBindings[name];
    }
}
