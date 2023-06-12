using System.Collections;
using UnityEngine;
public class ShipLaser : ShipSubModule
{
    public float laserCooldownDuration = 2f;
    private float laserCooldownTimeRemaining = 0f;
    
    public GameObject laser;
    public float laserDuration = 0.1f;

    private Coroutine _laserCoroutine = null;

    void Update()
    {
        if(laserCooldownTimeRemaining > 0f)
        {
            laserCooldownTimeRemaining -= Time.deltaTime;
        }
    }
    
    public void FireLaser()
    {
        Debug.Log("Attempting fire");
        if (_laserCoroutine == null && laserCooldownTimeRemaining <= 0f)
        {
            StartCoroutine(FireLaserCoroutine());
        }
    }
    
    private IEnumerator FireLaserCoroutine()
    {
        laserCooldownTimeRemaining = laserCooldownDuration;
        
        laser.SetActive(true);
        yield return new WaitForSeconds(laserDuration);
        laser.SetActive(false);
        
        _laserCoroutine = null;
    }
}
