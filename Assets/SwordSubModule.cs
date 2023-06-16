using System.Collections;
using System.Collections.Generic;
using GamePhases;
using UnityEngine;

public class SwordSubModule : ShipSubModule
{
    public Animator swordAnimator;
    public GameObject swordCollider;
    
    public int swordDamage = 1;
    public float swordCooldownDuration = 2f;
    private float _swordCooldownTimeRemaining = 0f;
    
    void Update()
    {
        if(_swordCooldownTimeRemaining > 0f)
        {
            _swordCooldownTimeRemaining -= Time.deltaTime;
        }
        else if(!PhaseManager.Current.IsJumping)
        {
            TriggerSword();
        }
    }

    public void TriggerSword()
    {
        // Debug.Log("SWORD");
        
        if (_swordCooldownTimeRemaining > 0f || PhaseManager.Current.CurrentPhase == GamePhase.Construction) {
            return;
        }

        _swordCooldownTimeRemaining = swordCooldownDuration;

        swordAnimator.Play("Sword");
        swordCollider.gameObject.SetActive(true);

        StartCoroutine(SwordCoroutine());
    }

    private IEnumerator SwordCoroutine()
    {
        yield return new WaitForFixedUpdate();

        swordCollider.gameObject.SetActive(false);
    }
}
