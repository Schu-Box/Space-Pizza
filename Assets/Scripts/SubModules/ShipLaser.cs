using System.Collections;
using GamePhases;
using Unity.VisualScripting;
using UnityEngine;
public class ShipLaser : ShipSubModule
{ 
    public Laser laserPrefab;
    public Transform laserSpawnTransform;
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
        else
        {
            FireLaser();
        }
    }

    public void FireLaser()
    {
        if (_laserCooldownTimeRemaining > 0f || PhaseManager.Current.CurrentPhase == GamePhase.Construction) {
            return;
        }

        _laserCooldownTimeRemaining = laserCooldownDuration;

        Laser laser = Instantiate(laserPrefab, laserSpawnTransform.position, Quaternion.identity).GetComponent<Laser>();
        laser.Fire(this);
    }
    
    // private IEnumerator FireLaserCoroutine()
    // {
    //     _laserCooldownTimeRemaining = laserCooldownDuration;
    //     
    //     laser.SetActive(true);
    //     yield return new WaitForSeconds(laserDuration);
    //     laser.SetActive(false);
    //     
    //     _laserCoroutine = null;
    // }
}
