using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ParticleLights : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public Light2D light2D;

    private void Start()
    {
        EnableParticles(false);
    }

    public void EnableParticles(bool activate)
    {
        if (particleSystem != null)
        {
            var emission = particleSystem.emission;
            emission.enabled = activate;
        }

        if (light2D != null)
        {
            light2D.gameObject.SetActive(activate);
        }
    }
}
