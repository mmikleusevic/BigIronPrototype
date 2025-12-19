using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private SceneReferenceSO gameSceneReferenceSO;
        
    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }
        
    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }
        
    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        if (newScene.name == gameSceneReferenceSO.AssetName)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Show()
    {
        canvas.gameObject.SetActive(true);
    }
        
    private void Hide()
    {
        canvas.gameObject.SetActive(false);
    }
}