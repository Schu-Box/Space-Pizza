using System;
using System.Collections;
using GamePhases;
using Unity.VisualScripting;
using UnityEngine;

public class ShipLaser : ShipSubModule
{
    public Laser laserPrefab;
    public Transform laserSpawnTransform;
    public int laserDamage = 1;
    public float laserCooldownDuration = 2f;
    private float _laserCooldownTimeRemaining = 0f;
    
    private Coroutine _laserCoroutine = null;

    [SerializeField]
    private GameObject shootDirectionPreview;

    private void Start()
    {
        PhaseManager.Current.PhaseChangedEvent += UpdateFirePreviewVisibility;
    }
    
    private void OnDestroy()
    {
        PhaseManager.Current.PhaseChangedEvent -= UpdateFirePreviewVisibility;
    }


    void Update()
    {
        if(_laserCooldownTimeRemaining > 0f)
        {
            _laserCooldownTimeRemaining -= Time.deltaTime;
        }
        else if(!PhaseManager.Current.IsJumping)
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
        laser.transform.eulerAngles = transform.eulerAngles;
        laser.Fire(this);
    }

    private void UpdateFirePreviewVisibility()
    {
        bool shouldBeVisible = PhaseManager.Current.CurrentPhase == GamePhase.Construction && wasGrabbed;

        shootDirectionPreview.SetActive(shouldBeVisible);
    }

    public override void HandleModuleGrabbed()
    {
        base.HandleModuleGrabbed();
        
        UpdateFirePreviewVisibility();
    }
}
