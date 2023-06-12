using System.Collections;
using UnityEngine;
public class ShipLaser : ShipSubModule
{ 
    public GameObject laser;
    public int laserDamage = 1;
    public float laserDuration = 0.1f;
    public float laserCooldownDuration = 2f;
    private float _laserCooldownTimeRemaining = 0f;
    
    private Coroutine _laserCoroutine = null;

    void Update()
    {
        if(_laserCooldownTimeRemaining > 0f)
        {
            _laserCooldownTimeRemaining -= Time.deltaTime;
        }
    }
    
    public void FireLaser()
    {
        if (_laserCoroutine == null && _laserCooldownTimeRemaining <= 0f)
        {
            StartCoroutine(FireLaserCoroutine());
        }
    }
    
    private IEnumerator FireLaserCoroutine()
    {
        _laserCooldownTimeRemaining = laserCooldownDuration;
        
        laser.SetActive(true);
        yield return new WaitForSeconds(laserDuration);
        laser.SetActive(false);
        
        _laserCoroutine = null;
    }
}
