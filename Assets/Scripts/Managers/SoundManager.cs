using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Managers
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private AudioSource vfxSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private float transitionTime;
        
        private float sfxVolume;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            float volume = PlayerPrefs.GetFloat(GameStrings.VOLUME);
            SetVolume(volume);
            
            sfxVolume = PlayerPrefs.GetFloat(GameStrings.SFX_VOLUME);
        }

        public void SetVolume(float volume)
        {
            musicSource.volume = volume;   
            PlayerPrefs.SetFloat(GameStrings.VOLUME, musicSource.volume);
        }

        public float GetVolume()
        {
            return musicSource.volume;
        }
        
        public void SetSfxVolume(float sfxVolume)
        {
            this.sfxVolume = sfxVolume;   
            PlayerPrefs.SetFloat(GameStrings.SFX_VOLUME, this.sfxVolume);
        }

        public float GetSfxVolume()
        {
            return sfxVolume;
        }

        public void SmoothChangeTrack(AudioClip clip)
        {
            StartCoroutine(SmoothChangeTrackCoroutine(clip));
        }

        private IEnumerator SmoothChangeTrackCoroutine(AudioClip clip)
        {
            if (musicSource.clip == clip) yield break;
            
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

        public void PlayVFX(AudioClip clip)
        {
            float pitch = Random.Range(0.8f, 1.2f);
            
            vfxSource.pitch = pitch;
            vfxSource.PlayOneShot(clip);
        }
    }
}