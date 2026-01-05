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

        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private float transitionTime;
        
        private Coroutine currentTransition;
        private float targetVolume;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            float volume = PlayerPrefs.GetFloat(GameStrings.VOLUME);
            SetVolume(volume);
            targetVolume = volume;
            
            float sfxVolume = PlayerPrefs.GetFloat(GameStrings.SFX_VOLUME);
            SetSfxVolume(sfxVolume);
        }

        public void SetVolume(float volume)
        {
            musicSource.volume = volume;   
            targetVolume = volume;
            PlayerPrefs.SetFloat(GameStrings.VOLUME, musicSource.volume);
        }

        public float GetVolume()
        {
            return musicSource.volume;
        }
        
        public void SetSfxVolume(float sfxVolume)
        {
            sfxSource.volume = sfxVolume;   
            PlayerPrefs.SetFloat(GameStrings.SFX_VOLUME, sfxSource.volume);
        }

        public float GetSfxVolume()
        {
            return sfxSource.volume;
        }

        public void SmoothChangeTrack(AudioClip clip)
        {
            if (!clip)
            {
                Debug.LogError("SmoothChangeTrack called with NULL AudioClip");
                return;
            }
            
            if (currentTransition != null) StopCoroutine(currentTransition);
            
            currentTransition = StartCoroutine(SmoothChangeTrackCoroutine(clip));
        }

        private IEnumerator SmoothChangeTrackCoroutine(AudioClip clip)
        {
            if (musicSource.clip == clip && musicSource.isPlaying && Mathf.Approximately(musicSource.volume, targetVolume))
            {
                currentTransition = null;
                yield break;
            }
            
            if (musicSource.clip == clip && musicSource.isPlaying)
            {
                yield return StartCoroutine(SmoothChangeVolume(musicSource.volume, targetVolume));
                currentTransition = null;
                yield break;
            }
    
            float startingVolume = musicSource.volume;
            float endingVolume = 0;
    
            yield return StartCoroutine(SmoothChangeVolume(startingVolume, endingVolume));
    
            musicSource.clip = clip;
            musicSource.Play();
    
            yield return StartCoroutine(SmoothChangeVolume(0, targetVolume));
    
            currentTransition = null;
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
            
            sfxSource.pitch = pitch;
            sfxSource.PlayOneShot(clip);
        }
    }
}