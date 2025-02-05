using UnityEngine;

public class SingletonSingleScene<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance) return _instance;
            _instance = FindObjectOfType<T>();
            if (_instance) return _instance;
            GameObject obj = new GameObject(typeof(T).ToString());
            _instance = obj.AddComponent<T>();
            return _instance;
        }
    }
}