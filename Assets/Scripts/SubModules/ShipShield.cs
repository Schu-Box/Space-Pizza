using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShield : ShipSubModule
{
   public GameObject shield;
   public int shieldMaxHealth = 2;
   public float shieldRechargeDuration = 4f;
   public int damageDealtOnCollision = 1;
   private float _shieldCooldownTimeRemaining = 0f;
   private int shieldHealthRemaining = 0;
   private bool _shieldEnabled = true;
   private Coroutine _shieldRechargeCoroutine = null;

   void Start()
   {
      EnableShield();
   }
   
   void Update()
   {
      if(_shieldCooldownTimeRemaining > 0f)
      {
         _shieldCooldownTimeRemaining -= Time.deltaTime;
      }
   }
    
   public void EnableShield()
   {
      shield.SetActive(true);
      shieldHealthRemaining = shieldMaxHealth;
      
      //TODO: Animate scale up
   }

   public void AbsorbDamage(int damage)
   {
      shieldHealthRemaining -= damage;

      if (shieldHealthRemaining <= 0f && _shieldRechargeCoroutine == null)
      {
         shield.SetActive(false);
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
