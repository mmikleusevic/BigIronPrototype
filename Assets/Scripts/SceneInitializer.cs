using UnityEngine;

public abstract class SceneInitializer : MonoBehaviour
{
    protected abstract void Initialize();

    protected virtual void Start()
    {
        Initialize();
    }
}