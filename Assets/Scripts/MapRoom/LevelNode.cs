using System;
using System.Collections.Generic;

namespace MapRoom
{
    public class LevelNode
    {
        public int Floor { get; private set; }
        public LevelNodeType LevelNodeType { get; private set; }
    
        private readonly List<LevelNode> connections = new List<LevelNode>();
        public IReadOnlyList<LevelNode> Connections => connections.AsReadOnly();

        public LevelNode(int floor, LevelNodeType levelNodeType)
        {
            Floor = floor;
            LevelNodeType = levelNodeType;
        }

        public void TryAddConnection(LevelNode target)
        {
            if (target == null || connections.Contains(target)) return;
            connections.Add(target);
        }
    }
}
