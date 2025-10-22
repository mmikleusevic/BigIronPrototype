using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace MapRoom
{
    public class MapController : MonoBehaviour
    {
        [SerializeField] private LevelNodeGenerator levelNodeGenerator;
        [SerializeField] private LevelNodeSpriteSetSO levelNodeSpriteSetSo;
        [SerializeField] private MapLevelNodeDrawer mapLevelNodeDrawer;
    
        [Header("Map nodes")]
        [SerializeField] private MapLevelNode mapLevelNodePrefab;
        [SerializeField] private RectTransform mapLevelNodeParent;
        [SerializeField] private GameObject mapLevelNodeGroupPrefab;
    
        private readonly MapLevelNodes activeNodes = new MapLevelNodes();
        private MapLevelNode currentLevelNode;

        private void OnEnable()
        {
            levelNodeGenerator.OnLevelNodesGenerated += OnNodesGenerated;
        }

        private void OnDisable()
        {
            levelNodeGenerator.OnLevelNodesGenerated -= OnNodesGenerated;
        }

        private void OnNodesGenerated()
        {
            for (int i = levelNodeGenerator.LevelNodeGraph.Count- 1; i >= 0; i--)
            {
                GameObject mapLevelNodeGroup = mapLevelNodeGroupPrefab.GetPooledObject(mapLevelNodeParent);
            
                IReadOnlyList<LevelNode> floor = levelNodeGenerator.LevelNodeGraph[i];

                for (int j = floor.Count - 1; j >= 0; j--)
                {
                    MapLevelNode nodeUI = mapLevelNodePrefab.GetPooledObject<MapLevelNode>(mapLevelNodeGroup.transform);
                
                    activeNodes.AddNode(nodeUI);

                    LevelNode nodeData = floor[j];
                    
                    nodeUI.Initialize(levelNodeSpriteSetSo.GetSprite(nodeData.LevelNodeType), nodeData, HandleNodeClicked);
                }
            }
        
            mapLevelNodeDrawer.DrawConnections(activeNodes);
        
            currentLevelNode = activeNodes.GetNodeByLevelType(LevelNodeType.Start);
            SetInteractableButtons();
            currentLevelNode?.Highlight(true);
        }

        private void HandleNodeClicked(MapLevelNode clickedNode)
        {
            if (!currentLevelNode) return;
        
            currentLevelNode.Highlight(false);
            currentLevelNode.SetInteractable(false);
            currentLevelNode = clickedNode;
            currentLevelNode.Highlight(true);

            DisableOtherButtons();
            SetInteractableButtons();

            Debug.Log(currentLevelNode.LevelNode.Connections.Count);
            // TODO: load actual level
            Debug.Log($"Loading level: {clickedNode.LevelNode.LevelNodeType}");
        }

        private void DisableOtherButtons()
        {
            foreach (MapLevelNode mapLevelNode in activeNodes.LevelNodeGraph)
            {
                mapLevelNode.SetInteractable(false);
            }
        }
    
        private void SetInteractableButtons()
        {
            if (!currentLevelNode) return;
        
            foreach (LevelNode levelNode in currentLevelNode.LevelNode.Connections)
            {
                MapLevelNode mapLevelNode = activeNodes.GetNodeByLevelNode(levelNode);
                mapLevelNode.SetInteractable(true);
            }
        }
    }
}