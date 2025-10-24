using System;
using Managers;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.InitializeGame();
    }
}
