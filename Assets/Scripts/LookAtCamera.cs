using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void OnEnable()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (transform) transform.LookAt(mainCamera?.transform);
    }
}