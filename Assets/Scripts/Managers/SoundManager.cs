using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        
        public float SfxVolume { get; set; }
        
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private float transitionTime;
        
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetVolume(float volume)
        {
            musicSource.volume = volume;   
        }

        public IEnumerator SmoothChangeTrack(AudioClip clip)
        {
            float startingVolume = musicSource.volume;
            float endingVolume = 0;
            
            yield return StartCoroutine(SmoothChangeVolume(startingVolume, endingVolume));
            
            musicSource.clip = clip;
            
            float tempVolume = startingVolume;
            startingVolume = musicSource.volume;
            endingVolume = tempVolume;
            
            yield return StartCoroutine(SmoothChangeVolume(startingVolume, endingVolume));
        }

        private IEnumerator SmoothChangeVolume(float startingVolume, float endingVolume)
        {
            float timer = 0;
            
            while (timer < transitionTime)
            {
                float elapsed = timer / transitionTime;
                
                musicSource.volume = Mathf.Lerp(startingVolume, endingVolume, elapsed);
                
                timer += Time.deltaTime;
                yield return null;
            }
            
            musicSource.volume = endingVolume;
        }
    }
}