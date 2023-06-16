using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisableButton : MonoBehaviour
{
    public TMP_InputField field;
    public Button button;
    void Update()
    {
        if(field.text.Length > 0){
            button.interactable = true;
        }
    }
}
