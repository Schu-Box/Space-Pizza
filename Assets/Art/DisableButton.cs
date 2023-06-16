using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisableButton : MonoBehaviour
{
    public TMP_InputField field;
    public Button button;
    // Update is called once per frame
    private void Awake()
    {
        button.interactable = false;
    }
    void Update()
    {
        if(field.text.Length > 0){
            button.interactable = true;
        }
    }
}
