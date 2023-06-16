using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Helpers
{
    public class RandomizedAudioPlayer: MonoBehaviour
    {
        [SerializeField]
        private List<AudioClip> audioClips = new();

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private bool playAtStart = false;

        private void Start()
        {
            if (playAtStart)
            {
                Play();
            }
        }

        public void Play()
        {
            AudioClip randomClip = audioClips[Random.Range(0, audioClips.Count)];

            _audioSource.clip = randomClip;
            _audioSource.Play();
        }
    }
}