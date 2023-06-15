using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShipThruster : ShipSubModule
{
    public float speed = 2f;
    public float rotationSpeed = 20f;

    public List<ParticleLights> particleLightList;

    public void ToggleParticleLights(bool activated)
    {
        foreach(ParticleLights particleLight in particleLightList)
        {
            particleLight.EnableParticles(activated);
        }
    }
}
