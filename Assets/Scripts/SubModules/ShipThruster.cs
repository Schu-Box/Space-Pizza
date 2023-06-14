using System;
using UnityEngine;

public class ShipThruster : ShipSubModule
{
    public float speed = 2f;
    public float rotationSpeed = 20f;
    
    public ParticleSystem particleSystem;

    private void Start()
    {
        EnableParticles(false);
    }

    public void EnableParticles(bool activate)
    {
        var emission = particleSystem.emission;
        emission.enabled = activate;
    }
}
