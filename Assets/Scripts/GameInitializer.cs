using System;
using Managers;
using UnityEngine;

public class GameInitializer : SceneInitializer
{
    protected override void Initialize()
    {
        if (GameManager.Instance) GameManager.Instance.InitializeGame();
    }
}