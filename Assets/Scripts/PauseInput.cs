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
        GameManager.Instance.OnGameOver += OnGameOver;
        GameManager.Instance.OnGameStarted += OnGameStarted;
        pauseInput.action.performed += OnPausePressed;
        pauseInput.action.Enable();
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= OnGameOver;
        GameManager.Instance.OnGameStarted -= OnGameStarted;
        pauseInput.action.performed -= OnPausePressed;
        pauseInput.action.Disable();
    }

    private void OnGameOver(bool hasWon, bool isGameOver)
    {
        pauseInput.action.Disable();
    }
    
    private void OnGameStarted()
    {
        pauseInput.action.Enable();
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        pauseUI.TogglePause();
    }
}