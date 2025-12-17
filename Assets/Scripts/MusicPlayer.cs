using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip musicClip;

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += SceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= SceneLoaded;
    }

    private void SceneLoaded(Scene previousScene, Scene newScene)
    {
        if (newScene == gameObject.scene)
        {
            SoundManager.Instance.SmoothChangeTrack(musicClip);
        }
    }
}