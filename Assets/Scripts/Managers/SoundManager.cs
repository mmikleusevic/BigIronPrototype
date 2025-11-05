using System;
using System.Collections;
using UnityEngine;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private float transitionTime;
        
        private float sfxVolume;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            float volume = PlayerPrefs.GetFloat(GameEvents.VOLUME);
            SetVolume(volume);
            
            float sfxVolume = PlayerPrefs.GetFloat(GameEvents.SFX_VOLUME);
            this.sfxVolume = sfxVolume;
        }

        public void SetVolume(float volume)
        {
            musicSource.volume = volume;   
            PlayerPrefs.SetFloat(GameEvents.VOLUME, musicSource.volume);
        }

        public float GetVolume()
        {
            return musicSource.volume;
        }
        
        public void SetSfxVolume(float sfxVolume)
        {
            this.sfxVolume = sfxVolume;   
            PlayerPrefs.SetFloat(GameEvents.SFX_VOLUME, this.sfxVolume);
        }

        public float GetSfxVolume()
        {
            return sfxVolume;
        }

        public IEnumerator SmoothChangeTrack(AudioClip clip)
        {
            float startingVolume = musicSource.volume;
            float endingVolume = 0;
            
            yield return StartCoroutine(SmoothChangeVolume(startingVolume, endingVolume));
            
            musicSource.clip = clip;
            musicSource.Play();
            
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