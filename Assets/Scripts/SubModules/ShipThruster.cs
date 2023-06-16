using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShipThruster : ShipSubModule
{
    private float speedIncreaseFactor = 1.1f;
    private float rotationSpeedIncreaseFactor = 17f;
    
    [SerializeField]
    private float speed = 1f;
    public float Speed => speed * speedIncreaseFactor;
    
    [SerializeField]
    private float rotationSpeed = 1f;

    public float RotationSpeed => rotationSpeed * rotationSpeedIncreaseFactor;

    public List<ParticleLights> particleLightList;
    

    public void ToggleParticleLights(bool activated)
    {
        foreach(ParticleLights particleLight in particleLightList)
        {
            particleLight.EnableParticles(activated);
        }
    }
}
