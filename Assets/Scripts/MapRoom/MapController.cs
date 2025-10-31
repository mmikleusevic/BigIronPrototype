using System;
using System.Collections.Generic;
using Extensions;
using Managers;
using UnityEngine;

namespace MapRoom
{
    public class MapController : MonoBehaviour, IClearable
    {
        [SerializeField] private LevelNodeGenerator levelNodeGenerator;
        [SerializeField] private MapLevelNodeDrawer mapLevelNodeDrawer;
        [SerializeField] private LevelNodeSpriteSetSO levelNodeSpriteSetSO;
        
        [Header("Map nodes")]
        [SerializeField] private MapLevelNode mapLevelNodePrefab;
        [SerializeField] private GameObject mapLevelNodeGroupPrefab;
        [SerializeField] private RectTransform mapLevelNodeParent;
        
        private readonly MapLevelNodes mapLevelNodes = new MapLevelNodes();
        private MapLevelNode currentLevelNode;

        private void Awake()
        {
            GameManager.Instance.Clearables.Add(this);
        }

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
                
                    mapLevelNodes.AddNode(nodeUI);

                    LevelNode nodeData = floor[j];
                    
                    nodeUI.Initialize(levelNodeSpriteSetSO.GetSprite(nodeData.LevelNodeType), nodeData, HandleNodeClicked);
                }
            }
        
            mapLevelNodeDrawer.DrawConnections(mapLevelNodes);
        
            currentLevelNode = mapLevelNodes.GetNodeByLevelType(LevelNodeType.Start);
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
            
            Debug.Log($"Loading level: {clickedNode.LevelNode.LevelNodeType}");
        }

        private void DisableOtherButtons()
        {
            foreach (MapLevelNode mapLevelNode in mapLevelNodes.LevelNodeGraph)
            {
                mapLevelNode.SetInteractable(false);
            }
        }
    
        private void SetInteractableButtons()
        {
            if (!currentLevelNode) return;
        
            foreach (LevelNode levelNode in currentLevelNode.LevelNode.Connections)
            {
                MapLevelNode mapLevelNode = mapLevelNodes.GetNodeByLevelNode(levelNode);
                mapLevelNode.SetInteractable(true);
            }
        }

        public void ReturnToPool()
        {
            mapLevelNodes.LevelNodeGraph.ReturnAllToPool();
            mapLevelNodeParent.ReturnToPool();
        }
    }
}