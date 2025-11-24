using System;
using Managers;
using UnityEngine;

public class GameInitializer : SceneInitializer
{
    protected override void Initialize()
    {
        GameManager.Instance.InitializeGame();
    }
}