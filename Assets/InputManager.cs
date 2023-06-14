using System.Collections;
using System.Collections.Generic;
using GamePhases;
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

        if (PhaseManager.Current.CurrentPhase == GamePhase.Fighting && GameplayInterfaceManager.Instance.WASDTutorialShown)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                GameplayInterfaceManager.Instance.HideWASDTutorial();
            }
        }
    }
    
    public void Restart()
    {
        Destroy(ShipManager.Current.PlayerShip.RootTransform.gameObject);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);  
    }
}
