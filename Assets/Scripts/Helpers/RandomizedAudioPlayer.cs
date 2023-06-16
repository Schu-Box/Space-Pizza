using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Helpers
{
    public class RandomizedAudioPlayer: MonoBehaviour
    {
        private Vector2 pitchRange = new Vector2(0.9f, 1.1f);
        private Vector2 volumeRange = new Vector2(0.9f, 1.1f);
        
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

            _audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
            _audioSource.volume = Random.Range(volumeRange.x, volumeRange.y);
            
            _audioSource.clip = randomClip;
            _audioSource.Play();
        }
    }
}