using System;
using Managers;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Start()
    {
        {
            if (!GameManager.Instance)
            {
                Debug.LogError("GameManager not found — make sure it's loaded before the Game scene.");
                return;
            }

            GameManager.Instance.InitializeGame();
            
            Destroy(gameObject);
        }
    }
}
