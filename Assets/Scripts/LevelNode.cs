using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelNode
{
    public int Floor { get; private set; }
    public Vector2 Position { get; private set; }
    public LevelNodeType LevelNodeType { get; private set; }
    
    private readonly List<LevelNode> connections = new List<LevelNode>();
    public IReadOnlyList<LevelNode> Connections => connections.AsReadOnly();

    public LevelNode(int floor, Vector2 position, LevelNodeType levelNodeType)
    {
        Floor = floor;
        Position = position;
        LevelNodeType = levelNodeType;
    }

    public void TryAddConnection(LevelNode target)
    {
        if (target == null || connections.Contains(target)) return;
        connections.Add(target);
    }
}
