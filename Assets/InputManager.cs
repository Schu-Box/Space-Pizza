using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Current => GameManager.Instance.ReferenceProvider.InputManager;

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Restart();
        }
    }
    
    public void Restart()
    {
        Destroy(ShipManager.Current.PlayerShip.RootTransform.gameObject);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);  
    }
}
