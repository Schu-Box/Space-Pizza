using System.Collections;
using System.Collections.Generic;
using GamePhases;
using Managers;
using ShipParts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static InputManager Current => GameManager.Instance.ReferenceProvider.InputManager;

    [SerializeField]
    private LayerMask grabLayer;

    private void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            Restart();
        }

        if (PhaseManager.Current.CurrentPhase == GamePhase.Fighting && 
            GameplayInterfaceManager.Instance.WASDTutorialShown)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                GameplayInterfaceManager.Instance.HideWASDTutorial();
                
                HazardManager.Instance.StartSpawningHazards();
                
                ShipManager.Current.PlayerShip.StartChargingJumpDrive();
            }
        }

        if (PhaseManager.Current.CurrentPhase == GamePhase.Construction &&
            Input.GetMouseButtonDown(0))
        {
            CheckForGrabbing();
        }
    }

    private void CheckForGrabbing()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
            Vector2.zero, 20, grabLayer);
        
        if (hit.collider == null)
        {
            return;
        }

        ModuleSelector moduleSelector = hit.collider.GetComponent<ModuleSelector>();

        if (moduleSelector == null)
        {
            Debug.LogError($"[InputManager] CheckForGrabbing raycast hit but object did not contain" +
                           $" the expected behaviour", hit.collider);
            return;
        }
        
        moduleSelector.HandleModuleSelected();
    }

    public void Restart()
    {
        Destroy(ShipManager.Current.PlayerShip.RootTransform.gameObject);
        Destroy(GameManager.Instance.gameObject);
        SceneManager.LoadScene(0);  
    }
}
