using System;
using System.Collections.Generic;
using Extensions;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MapRoom
{
    public class MapController : MonoBehaviour
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
        private MapLevelNode lastSelectedNode;
        private MapLevelNode lastNode;

        private void OnEnable()
        {
            levelNodeGenerator.OnLevelNodesGenerated += OnNodesGenerated;
            if (GameManager.Instance) GameManager.Instance.OnRoomPassed += SetInteractableButtons;
        }

        private void OnDisable()
        {
            levelNodeGenerator.OnLevelNodesGenerated -= OnNodesGenerated;
            if (GameManager.Instance) GameManager.Instance.OnRoomPassed -= SetInteractableButtons;
        }

        private void OnNodesGenerated()
        {
            for (int i = levelNodeGenerator.LevelNodeGraph.Count- 1; i >= 0; i--)
            {
                GameObject mapLevelNodeGroup = Instantiate(mapLevelNodeGroupPrefab, mapLevelNodeGroupPrefab.transform.position, mapLevelNodeGroupPrefab.transform.rotation);
                mapLevelNodeGroup.transform.SetParent(mapLevelNodeParent, false);
            
                IReadOnlyList<LevelNode> floor = levelNodeGenerator.LevelNodeGraph[i];

                for (int j = floor.Count - 1; j >= 0; j--)
                {
                    MapLevelNode nodeUI = Instantiate(mapLevelNodePrefab, mapLevelNodePrefab.transform.position, mapLevelNodePrefab.transform.rotation);
                    nodeUI.transform.SetParent(mapLevelNodeGroup.transform, false);
                    
                    mapLevelNodes.AddNode(nodeUI);

                    LevelNode nodeData = floor[j];
                    
                    nodeUI.Initialize(levelNodeSpriteSetSO.GetSprite(nodeData.LevelNodeType), nodeData, HandleNodeClicked);
                }
            }
        
            mapLevelNodeDrawer.DrawConnections(mapLevelNodes);
        
            currentLevelNode = mapLevelNodes.GetNodeByLevelType(LevelNodeType.Start);
            lastNode = mapLevelNodes.GetNodeByLevelType(LevelNodeType.Boss);
            SetInteractableButtons();
            if (currentLevelNode) currentLevelNode.Highlight(true);
        }
        
        private void HandleNodeSelected(MapLevelNode node)
        {
            if (UIFocusManager.Instance) UIFocusManager.Instance.PopFocus();
            SetLastSelectedNode(node);
        }

        private void HandleNodeClicked(MapLevelNode clickedNode)
        {
            if (!currentLevelNode) return;
        
            currentLevelNode.Highlight(false);
            currentLevelNode.SetInteractable(false);
            currentLevelNode = clickedNode;
            currentLevelNode.Highlight(true);
            
            if (UIFocusManager.Instance) UIFocusManager.Instance.PopFocus();
            
            lastSelectedNode = null;
            
            DisableOtherButtons();
        }

        private void DisableOtherButtons()
        {
            foreach (MapLevelNode mapLevelNode in mapLevelNodes.LevelNodeGraph)
            {
                mapLevelNode.OnNodeSelected -= HandleNodeSelected;
                mapLevelNode.SetInteractable(false);
            }
        }
    
        private void SetInteractableButtons()
        {
            if (!currentLevelNode) return;
            
            List<MapLevelNode> nextNodes = new List<MapLevelNode>();

            for (int i = currentLevelNode.LevelNode.Connections.Count - 1; i >= 0; i--)
            {
                LevelNode levelNode = currentLevelNode.LevelNode.Connections[i];
                MapLevelNode mapLevelNode = mapLevelNodes.GetNodeByLevelNode(levelNode);
                mapLevelNode.SetInteractable(true);

                mapLevelNode.OnNodeSelected -= HandleNodeSelected;
                mapLevelNode.OnNodeSelected += HandleNodeSelected;
                
                nextNodes.Add(mapLevelNode);
                
                if (lastSelectedNode) continue;

                SetLastSelectedNode(mapLevelNode);
            }
            
            if (currentLevelNode == lastNode)
            {
                GameManager.Instance.GameOver(true);
                return;
            }

            int count = nextNodes.Count;
            if (count == 0) return;
            
            for (int i = 0; i < count; i++)
            {
                int rightIndex = (i + 1) % count;
                int leftIndex = (i - 1 + count) % count;

                MapLevelNode current = nextNodes[i];
                MapLevelNode rightNode = nextNodes[rightIndex];
                MapLevelNode leftNode = nextNodes[leftIndex];
                
                current.SetNavigation(leftNode.NodeButton, rightNode.NodeButton);
            }
            
            if (!lastSelectedNode || !lastSelectedNode.NodeButton.interactable)
            {
                int defaultIndex = count / 2; 
                EventSystem.current.SetSelectedGameObject(nextNodes[defaultIndex].NodeButton.gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(lastSelectedNode.NodeButton.gameObject);
            }
        }

        private void SetLastSelectedNode(MapLevelNode mapLevelNode)
        {
            lastSelectedNode = mapLevelNode;
            if (UIFocusManager.Instance) UIFocusManager.Instance.PushFocus(lastSelectedNode.NodeButton);
        }
    }
}