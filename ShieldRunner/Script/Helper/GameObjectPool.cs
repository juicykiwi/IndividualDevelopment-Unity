using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IGameObjectPool
{
    void ReadyEnqueue();
    void ReadyDequeue();
}

public class GameObjectPool<TKey, TItem> where TItem : MonoBehaviour, IGameObjectPool
{
    Dictionary<TKey, Queue<TItem>> _pool = new Dictionary<TKey, Queue<TItem>>();

    public void Enqueue(TKey key, TItem item)
    {
        if (item == null)
            return;

        if (_pool.ContainsKey(key) == false)
            _pool.Add(key, new Queue<TItem>());

        item.ReadyEnqueue();
        _pool[key].Enqueue(item);
    }

    public TItem Dequeue(TKey key)
    {
        if (_pool.ContainsKey(key) == false)
            return null;

        if (_pool[key].Count <= 0)
            return null;

        TItem item = _pool[key].Dequeue();
        if (item == null)
            return null;
            
        item.ReadyDequeue();
        return item;
    }

    public void Clear()
    {
        _pool.Clear();
    }
}
