using System.Collections.Generic;
using System.Linq;

namespace MapRoom
{
    public class MapLevelNodes
    {
        private readonly List<MapLevelNode> levelNodeGraph = new List<MapLevelNode>();

        public IReadOnlyList<MapLevelNode> LevelNodeGraph => levelNodeGraph;

        public MapLevelNode GetNodeByLevelNode(LevelNode levelNode)
        {
            return levelNodeGraph.FirstOrDefault(a => a.LevelNode == levelNode);
        }

        public MapLevelNode GetNodeByLevelType(LevelNodeType levelNodeType)
        {
            return levelNodeGraph.FirstOrDefault(a => a.LevelNode.LevelNodeType == levelNodeType);
        }

        public void AddNode(MapLevelNode node)
        {
            levelNodeGraph.Add(node);
        }
    }
}
