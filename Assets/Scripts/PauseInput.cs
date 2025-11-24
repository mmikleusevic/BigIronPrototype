using Managers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private InputActionReference pauseInput;
    [SerializeField] private PauseUI pauseUI;
    
    private void OnEnable()
    {
        pauseInput.action.performed += OnPausePressed;
        pauseInput.action.Enable();
    }

    private void OnDisable()
    {
        pauseInput.action.performed -= OnPausePressed;
        pauseInput.action.Disable();
    }
        
    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        pauseUI.TogglePause();
    }
}