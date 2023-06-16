using System;
using System.Collections;
using GamePhases;
using Helpers;
using Unity.VisualScripting;
using UnityEngine;
using Random = System.Random;

public class ShipLaser : ShipSubModule
{
    public Laser laserPrefab;
    public Transform laserSpawnTransform;
    public int laserDamage = 1;
    public float laserCooldownDuration = 2f;
    private float _laserCooldownTimeRemaining = 0f;

    [SerializeField]
    private GameObject shootDirectionPreview;

    [SerializeField]
    private RandomizedAudioPlayer laserAudio;

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
        
        laserAudio.Play();

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
