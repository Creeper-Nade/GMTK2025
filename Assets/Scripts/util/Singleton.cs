using UnityEngine;

// Overrides current instance instead of destroying old instance, good for resetting state
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }
    protected virtual void Awake() => Instance = this as T;
    protected virtual void OnApplicationQuit()
    {
        Instance = null;
        Destroy(gameObject);
    }
}
//transforms static instance to singleton, destroys new instances and keeps original instance
public abstract class Singleton<T> : StaticInstance<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        base.Awake();
    }
}

//For system classes, persistent through all scenes
public abstract class SingletonPersistent<T> : Singleton<T> where T : MonoBehaviour
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}
