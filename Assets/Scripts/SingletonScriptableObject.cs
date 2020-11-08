using System.Linq;
using UnityEngine;

/// <summary>
/// https://connect.unity.com/p/singleton-scriptableobject
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject {
    static T _instance = null;
    public static T Instance
    {
        get
        {
            if (!_instance)
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            return _instance;
        }
    }
}