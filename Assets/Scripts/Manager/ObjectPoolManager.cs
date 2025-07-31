using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Pool;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [SerializeField] private bool _addToDontDestroyOnLoad = false;
    private GameObject _emptyHolder;

    private static GameObject _emptyParticleSystems;
    private static GameObject _emptyGameObjects;
    private static GameObject _emptySoundEffects;

    private static Dictionary<GameObject, ObjectPool<GameObject>> _objectPools;
    private static Dictionary<GameObject, GameObject> _cloneToPrefabMap;

    public enum PoolType
    {
        GameObjects,
        ParticleSystems,
        SoundEffects
    }
    public static PoolType PoolingType;
    protected override void Awake()
    {
        base.Awake();
        _objectPools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        _cloneToPrefabMap = new Dictionary<GameObject, GameObject>();
        SetupEmpties();
    }

    private void SetupEmpties()
    {
        _emptyHolder = new GameObject("Object Pools");

        _emptyParticleSystems = new GameObject("Particle Effects");
        _emptyParticleSystems.transform.SetParent(_emptyHolder.transform);

        _emptyGameObjects = new GameObject("GameObjects");
        _emptyGameObjects.transform.SetParent(_emptyHolder.transform);

        _emptySoundEffects = new GameObject("Sound FX");
        _emptySoundEffects.transform.SetParent(_emptyHolder.transform);

        if (_addToDontDestroyOnLoad)
            DontDestroyOnLoad(_emptyParticleSystems.transform.root);
    }
    #region Create in world space
    private void CreatePool(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, pos, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );

        _objectPools.Add(prefab, pool);
    }



    private GameObject CreateObject(GameObject prefab, Vector3 pos, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        prefab.SetActive(false);
        GameObject obj = Instantiate(prefab, pos, rot);
        prefab.SetActive(true);

        GameObject parentObject = SetParentObject(poolType);
        obj.transform.SetParent(parentObject.transform);

        return obj;
    }

    private void OnGetObject(GameObject obj)
    {
        //called when getting the object
    }

    private void OnReleaseObject(GameObject obj)
    {
        //called when putting the object back to pool, deactivating
        obj.SetActive(false);
    }

    private void OnDestroyObject(GameObject obj)
    {
        if (_cloneToPrefabMap.ContainsKey(obj))
        {
            _cloneToPrefabMap.Remove(obj);
        }
    }

    private GameObject SetParentObject(PoolType poolType)
    {
        switch (poolType)
        {
            case PoolType.ParticleSystems:
                return _emptyParticleSystems;
            case PoolType.GameObjects:
                return _emptyGameObjects;
            case PoolType.SoundEffects:
                return _emptySoundEffects;
            default:
                return null;
        }
    }

    private T SpawnObject<T>(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, spawnPos, spawnRotation, poolType);
        }
        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.transform.position = spawnPos;
            obj.transform.rotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Object{objectToSpawn.name} doesnt have component of type {typeof(T)}");
            }
            return component;
        }

        return null;
    }



    public T SpawnObject<T>(T typePrefab, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, spawnPos, spawnRotation, poolType);
    }


    public GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPos, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objectToSpawn, spawnPos, spawnRotation, poolType);
    }

    #endregion

    #region Create under parent
    private void CreatePool(GameObject prefab, Transform parent, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        ObjectPool<GameObject> pool = new ObjectPool<GameObject>(
            createFunc: () => CreateObject(prefab, parent, rot, poolType),
            actionOnGet: OnGetObject,
            actionOnRelease: OnReleaseObject,
            actionOnDestroy: OnDestroyObject
        );

        _objectPools.Add(prefab, pool);
    }
    // The special CreatObject method for creating the object under any designated parent
    private GameObject CreateObject(GameObject prefab, Transform parent, Quaternion rot, PoolType poolType = PoolType.GameObjects)
    {
        prefab.SetActive(false);
        GameObject obj = Instantiate(prefab, parent);

        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = rot;
        obj.transform.localScale = Vector3.one;

        prefab.SetActive(true);

        return obj;
    }
    // The special SpawnObject method for spawning the object under any designated parent
    private T SpawnObject<T>(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Object
    {
        if (!_objectPools.ContainsKey(objectToSpawn))
        {
            CreatePool(objectToSpawn, parent, spawnRotation, poolType);
        }
        GameObject obj = _objectPools[objectToSpawn].Get();

        if (obj != null)
        {
            if (!_cloneToPrefabMap.ContainsKey(obj))
            {
                _cloneToPrefabMap.Add(obj, objectToSpawn);
            }

            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = spawnRotation;
            obj.SetActive(true);

            if (typeof(T) == typeof(GameObject))
            {
                return obj as T;
            }

            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogError($"Object{objectToSpawn.name} doesnt have component of type {typeof(T)}");
            }
            return component;
        }

        return null;
    }

    // The special overload method for spawning the object under any designated parent
    public T SpawnObject<T>(T typePrefab, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects) where T : Component
    {
        return SpawnObject<T>(typePrefab.gameObject, parent, spawnRotation, poolType);
    }
    // The special overload method for spawning the object under any designated parent
    public GameObject SpawnObject(GameObject objectToSpawn, Transform parent, Quaternion spawnRotation, PoolType poolType = PoolType.GameObjects)
    {
        return SpawnObject<GameObject>(objectToSpawn, parent, spawnRotation, poolType);
    }

    #endregion

    public void ReturnObjectToPool(GameObject obj, PoolType poolType = PoolType.GameObjects)
    {
        if (_cloneToPrefabMap.TryGetValue(obj, out GameObject prefab))
        {
            GameObject parentObject = SetParentObject(poolType);
            if (obj.transform.parent != parentObject.transform)
            {
                obj.transform.SetParent(parentObject.transform);
            }
            if (_objectPools.TryGetValue(prefab, out ObjectPool<GameObject> pool))
            {
                pool.Release(obj);
            }
        }
        else
        {
            Debug.LogWarning("Attempts to return an unpooled object:" + obj.name);
        }
    }

}
