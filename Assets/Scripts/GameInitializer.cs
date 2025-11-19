using System;
using Managers;
using UnityEngine;

public class GameInitializer : SceneInitializer
{
    public override void Initialize()
    {
        GameManager.Instance.InitializeGame();
    }
}