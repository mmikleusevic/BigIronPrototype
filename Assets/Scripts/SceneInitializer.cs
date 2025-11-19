using UnityEngine;

public abstract class SceneInitializer : MonoBehaviour
{
    public abstract void Initialize();

    protected virtual void Start()
    {
        Initialize();
    }
}