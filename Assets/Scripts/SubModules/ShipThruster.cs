using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShipThruster : ShipSubModule
{
    public float speed = 2f;
    public float rotationSpeed = 20f;
    
    public ParticleSystem particleSystem;
    public Light2D light2D;

    private void Start()
    {
        EnableParticles(false);
    }

    public void EnableParticles(bool activate)
    {
        var emission = particleSystem.emission;
        emission.enabled = activate;

        light2D.gameObject.SetActive(activate);
    }
}
