using System;
using System.Collections;
using System.Collections.Generic;
using GamePhases;
using UnityEngine;

public class ShipShield : ShipSubModule
{
   [SerializeField]
   private GameObject activeShieldVisuals;

   [SerializeField]
   private Collider2D shieldCollider;
   
   public int shieldMaxHealth = 2;
   public float shieldRechargeDuration = 4f;
   public int damageDealtOnCollision = 1;
   private float _shieldCooldownTimeRemaining = 0f;
   private int shieldHealthRemaining = 0;
   private bool _shieldEnabled = true;
   private Coroutine _shieldRechargeCoroutine = null;

   void Start()
   {
      PhaseManager.Current.PhaseChangedEvent += UpdateShieldState;
      UpdateShieldState();
   }

   void Update()
   {
      if(_shieldCooldownTimeRemaining > 0f)
      {
         _shieldCooldownTimeRemaining -= Time.deltaTime;
      }
   }

   private void OnDestroy()
   {
      PhaseManager.Current.PhaseChangedEvent -= UpdateShieldState;
   }
   
   private void UpdateShieldState()
   {
      if (PhaseManager.Current.CurrentPhase == GamePhase.Fighting)
      {
         EnableShield();
         return;
      }
      
      DisableShield();
   }

   public void OnCollisionEnter2D(Collision2D other)
   {
      Hazard hazard = other.gameObject.GetComponent<Hazard>();
      if (hazard != null)
      {
         hazard.TakeDamage(damageDealtOnCollision);
         AbsorbDamage(hazard.damage);
      }
   }
    
   public void EnableShield()
   {
      activeShieldVisuals.SetActive(true);
      shieldCollider.enabled = true;
      shieldHealthRemaining = shieldMaxHealth;
      
      //TODO: Animate scale up
   }

   private void DisableShield()
   {
      activeShieldVisuals.SetActive(false);
      shieldCollider.enabled = false;
   }

   public void AbsorbDamage(int damage)
   {
      shieldHealthRemaining -= damage;

      if (shieldHealthRemaining <= 0f && _shieldRechargeCoroutine == null)
      {
         DisableShield();
         _shieldRechargeCoroutine = StartCoroutine(RechargeShieldCoroutine());
      }
   }

   private IEnumerator RechargeShieldCoroutine()
   {
      yield return new WaitForSeconds(shieldRechargeDuration);
        
      _shieldRechargeCoroutine = null;

      EnableShield();
   }
}
