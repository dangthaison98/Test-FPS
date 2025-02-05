using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonSingleScene<PoolManager>
{
    private readonly Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();


    #region Spawn
    public GameObject Spawn(string label, GameObject prefab)
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : Instantiate(prefab);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
            }
            return newObj;
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = Instantiate(prefab);
            return newObj;
        }
    }
    public GameObject Spawn(string label, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : Instantiate(prefab);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
            }
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj;
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = Instantiate(prefab, position, rotation);
            return newObj;
        }
    }
    public GameObject Spawn(string label, GameObject prefab, Transform parent)
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : Instantiate(prefab, parent);
            if (newObj.activeSelf) return newObj;
            newObj.SetActive(true);
            newObj.transform.parent = parent;
            return newObj;
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = Instantiate(prefab, parent);
            return newObj;
        }
    }
    public GameObject Spawn(string label, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : Instantiate(prefab, parent);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
                newObj.transform.parent = parent;
            }
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj;
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = Instantiate(prefab, position, rotation, parent);
            return newObj;
        }
    }



    public T Spawn<T>(string label, T prefab) where T : Component
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : Instantiate(prefab.gameObject);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
            }
            return newObj.GetComponent<T>();
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = Instantiate(prefab);
            return newObj.GetComponent<T>();
        }
    }
    public T Spawn<T>(string label, T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : Instantiate(prefab.gameObject);
            if (!newObj.activeSelf)
            {
                newObj.SetActive(true);
            }
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj.GetComponent<T>();
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = Instantiate(prefab, position, rotation);
            return newObj.GetComponent<T>();
        }
    }
    public T Spawn<T>(string label, T prefab, Transform parent) where T : Component
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : Instantiate(prefab.gameObject, parent);
            if (newObj.activeSelf) return newObj.GetComponent<T>();
            newObj.transform.SetParent(parent, false);
            newObj.SetActive(true);
            return newObj.GetComponent<T>();
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = Instantiate(prefab, parent);
            return newObj.GetComponent<T>();
        }
    }
    public T Spawn<T>(string label, T prefab, Vector3 position, Quaternion rotation, Transform parent) where T : Component
    {
        if (pool.ContainsKey(label))
        {
            var newObj = (pool[label].Count > 0) ? pool[label].Dequeue() : Instantiate(prefab.gameObject, parent);
            if (!newObj.activeSelf)
            {
                newObj.transform.SetParent(parent, false);
                newObj.SetActive(true);
            }
            newObj.transform.position = position;
            newObj.transform.rotation = rotation;
            return newObj.GetComponent<T>();
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
            var newObj = Instantiate(prefab, position, rotation, parent);
            return newObj.GetComponent<T>();
        }
    }
    #endregion


    #region Despawn
    public void Despawn(string label, GameObject prefab)
    {
        prefab.SetActive(false);
        if (pool.TryGetValue(label, out var value))
        {
            if (value.Contains(prefab)) return;
        }
        else
        {
            pool.Add(label, new Queue<GameObject>());
        }

        pool[label].Enqueue(prefab);
    }
    #endregion


    #region Init
    public void Init(string label, GameObject prefab)
    {
        var newObj = Instantiate(prefab);
        Despawn(label, newObj);
    }
    public void Init(string label, GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var newObj = Instantiate(prefab, position, rotation);
        Despawn(label, newObj);
    }
    public void Init(string label, GameObject prefab, Transform parent)
    {
        var newObj = Instantiate(prefab, parent);
        Despawn(label, newObj);
    }
    public void Init(string label, GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
    {
        var newObj = Instantiate(prefab, position, rotation, parent);
        Despawn(label, newObj);
    }
    #endregion
}