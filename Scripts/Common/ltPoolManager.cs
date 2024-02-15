using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ltPoolManager : BasicSingleton<ltPoolManager>
{
    [System.Serializable]
    public class PreCacheData
    {
        public GameObject Template;
        public int CacheCount;
        public int MaxPool = -1;
    }
    [System.Serializable]
    public class CacheItemData
    {
        public List<GameObject> ItemList = new List<GameObject>();
        public int MaxPool = -1;
    }
    public List<PreCacheData> PreCacheList = new List<PreCacheData>();
#if UNITY_EDITOR
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
#endif
    public Dictionary<GameObject, CacheItemData> PoolingDict = new Dictionary<GameObject, CacheItemData>();
    public Dictionary<GameObject, int> MaxCntDict = new Dictionary<GameObject, int>();
    public void Start()
    {
        foreach (var item in PreCacheList)
        {
            var cacheData = new CacheItemData() { MaxPool = item.MaxPool };
            PoolingDict.Add(item.Template, cacheData);
            for (int i = 0; i < item.CacheCount; i++)
            {
                var obj = Instantiate(item.Template);
                cacheData.ItemList.Add(obj);
                obj.SetActive(false);
            }
        }
    }
    public int GetObjectCnt(GameObject prefab)
    {
        if (!PoolingDict.ContainsKey(prefab))
        {
            return 0;
        }
        var list = PoolingDict[prefab].ItemList;
        list.RemoveAll(m => m == null);
        return list.Count;
    }
    public GameObject GetObject(GameObject prefab)
    {
        if (!PoolingDict.ContainsKey(prefab))
        {
            PoolingDict.Add(prefab, new CacheItemData() { MaxPool = -1 });
        }
        var data = PoolingDict[prefab];
        var list = data.ItemList;
        list.RemoveAll(m => m == null);
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            if(item.activeSelf == false)
            {
                return item;
            }
        }
        if (data.MaxPool > -1)
        {
            if (list.Count >= data.MaxPool)
                return null;
        }
        var obj = Instantiate(prefab);
        list.Add(obj);
        return obj;
    }
}
#if UNITY_EDITOR
public abstract class BasicSingleton<T> : SerializedMonoBehaviour
#else
public abstract class BasicSingleton<T> : MonoBehaviour
#endif
where T : BasicSingleton<T>
{
    protected static T _sharedInstance = null;
    public static T Instance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (_sharedInstance == null)
                {
                    var obj = new GameObject(typeof(T).ToString());
                    _sharedInstance = obj.AddComponent<T>();
                }
            }
            return _sharedInstance;
        }
    }

    protected virtual void OnDestroy()
    {
        if (_sharedInstance == this)
        {
            _sharedInstance = null;
        }
    }
    private void Awake()
    {
        _sharedInstance = this as T;
    }
}