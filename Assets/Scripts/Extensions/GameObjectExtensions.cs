using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class GameObjectExtensions
    {
        private static readonly Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();
        
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
            }
            else
            {
                gameObject = prefab.transform is RectTransform 
                    ? Object.Instantiate(prefab, parent, false) 
                    : Object.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation, parent);
            }

            return gameObject;
        }
        
        public static TComponent GetPooledObject<TComponent>(this Component prefab, Transform parent = null) where TComponent : Component
        {
            GameObject obj = prefab.gameObject.GetPooledObject(parent);
            return obj.GetComponent<TComponent>();
        }

        private static void ReturnToPool(this GameObject gameObject, GameObject prefab)
        {
            gameObject.SetActive(false);

            if (!pools.TryGetValue(prefab, out Queue<GameObject> pool))
            {
                pool = new Queue<GameObject>();
                pools[prefab] = pool;
            }

            pool.Enqueue(gameObject);
        }
        
        public static void ReturnToPool(this Component obj, Component prefab)
        {
            obj.gameObject.ReturnToPool(prefab.gameObject);
        }
        
        public static void ClearAllPools()
        {
            pools.Clear();
        }
    }
}