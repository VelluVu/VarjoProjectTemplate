using System;
using UnityEngine;

/// <summary>
/// Makes a monobehavior singleton when inherited.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly Lazy<T> _Instance = new Lazy<T>(CreateSingleton);

    public static T Instance => _Instance.Value;

    private static T CreateSingleton()
    {
        var ownerOBject = new GameObject($"{typeof(T).Name}(singleton)");
        var instance = ownerOBject.AddComponent<T>();
        DontDestroyOnLoad(ownerOBject);
        return instance;
    }
}
