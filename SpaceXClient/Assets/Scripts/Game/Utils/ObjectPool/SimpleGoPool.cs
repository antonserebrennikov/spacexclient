using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Utils.ObjectPool
{
    public class SimpleGoPool : IObjectPool<GameObject>
    {
        private readonly Queue<GameObject> pool = new();

        public GameObject GetFromPool()
        {
            if (pool.Count <= 0)
                throw new Exception("No objects in pool");

            return pool.Dequeue();
        }

        public void ReturnToPool(GameObject obj)
        {
            pool.Enqueue(obj);
        }
        
        public int Count => pool.Count;
    }
}