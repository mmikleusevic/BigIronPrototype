using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        private static readonly Dictionary<GameObject, Queue<GameObject>> pools =
            new Dictionary<GameObject, Queue<GameObject>>();

        private static readonly Dictionary<GameObject, GameObject> instanceToPrefab =
            new Dictionary<GameObject, GameObject>();

        private static GameObject poolParent = new GameObject("ObjectPool");
        
        public static GameObject GetPooledObject(this GameObject prefab, Transform parent = null)
        {
            if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
            {
                pool = new Queue<GameObject>();
                pools[prefab] = pool;
            }

            GameObject gameObject;
            if (pool.Count > 0)
            {
                gameObject = pool.Dequeue();
                gameObject.SetActive(true);
                Debug.Log("Pooled Object " + gameObject.name);
            }
            else
            {
                gameObject = prefab.transform is RectTransform 
                    ? Object.Instantiate(prefab) 
                    : Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
                
                Debug.Log("Instantiated Pooled Object " + gameObject.name);
                instanceToPrefab[gameObject] = prefab;
            }

            if (!parent)
            {
                gameObject.transform.SetParent(null);
                return gameObject;
            }
            
            if (gameObject.transform is RectTransform)
            {
                gameObject.transform.SetParent(parent, false);
            }
            else
            {
                gameObject.transform.SetParent(parent);
            }

            return gameObject;
        }
        
        public static TComponent GetPooledObject<TComponent>(this Component prefab, Transform parent = null) where TComponent : Component
        {
            return prefab.gameObject.GetPooledObject(parent).GetComponent<TComponent>();
        }

        private static void ReturnToPool(this GameObject gameObject)
        {
            if (!instanceToPrefab.TryGetValue(gameObject, out GameObject prefab))
            {
                Object.Destroy(gameObject);
                return;
            }
            
            gameObject.SetActive(false);
            gameObject.transform.SetParent(poolParent.transform);
            gameObject.transform.position = Vector3.zero;
            gameObject.transform.rotation = Quaternion.identity;
            gameObject.transform.localPosition = Vector3.zero;
            
            if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
            {
                pool = new Queue<GameObject>();
                pools[prefab] = pool;
            }

            Debug.Log($"Returned to pool! {gameObject.name}");
            pool.Enqueue(gameObject);
        }

        public static void ReturnToPool(this Component component)
        {
            component.gameObject.ReturnToPool();
        }
        
        public static void ReturnAllToPool(this Transform parent, params Transform[] exceptions)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Transform child = parent.GetChild(i);
                if (exceptions != null && Array.IndexOf(exceptions, child) >= 0) continue;

                child.ReturnToPool();
            }
        }
        
        public static void ReturnAllToPool<T>(this IEnumerable<T> collection) where T : Component
        {
            foreach (T item in collection)
            {
                item.ReturnToPool();
            }
        }

        public static void ClearAllPools()
        {
            pools.Clear();
            instanceToPrefab.Clear();
        }
    }
}