using System.Collections;
using UnityEngine;

public class ShipLaser : ShipSubModule
{
    [SerializeField]
    private Collider2D laserCollider;

    [SerializeField]
    private SpriteRenderer laserBeamVisuals;
    
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
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        Hazard hazard = other.gameObject.GetComponent<Hazard>();
        if (hazard != null)
        {
            hazard.TakeDamage(laserDamage);
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

        ActivateLaser();
        
        yield return new WaitForSeconds(laserDuration);
        
        DeactivateLaser();
        
        _laserCoroutine = null;
    }
    
    private void ActivateLaser()
    {
        laserBeamVisuals.enabled = true;
        laserCollider.enabled = true;
    }
    
    private void DeactivateLaser()
    {
        laserBeamVisuals.enabled = false;
        laserCollider.enabled = false;
    }
}
