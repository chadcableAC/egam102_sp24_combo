using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour

{
    // 声明一个私有的静态变量来保存PrefabManager的唯一实例
    private static PrefabManager _instance;

    // 存储所有活跃Prefab的部件计数
    private Dictionary<string, int> activePrefabsParts = new Dictionary<string, int>();

    // 存储Prefab原型以便于复制
    public Dictionary<string, GameObject> prefabPrototypes;

    // 公开的静态属性，用于获取PrefabManager的唯一实例
    public static PrefabManager Instance
    {
        get
        {
            // 如果_instance为null，查找场景中是否已经存在PrefabManager实例
            if (_instance == null)
            {
                _instance = FindObjectOfType<PrefabManager>();

                // 如果场景中不存在PrefabManager实例，则创建一个新的实例
                if (_instance == null)
                {
                    GameObject prefabManagerGO = new GameObject("PrefabManager");
                    _instance = prefabManagerGO.AddComponent<PrefabManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        // 如果Instance不等于当前实例，则销毁当前实例，确保只有一个PrefabManager实例存在
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 将当前实例设置为唯一实例
        _instance = this;

        // 在加载新场景时不销毁该对象
        DontDestroyOnLoad(gameObject);
    }


    public void PartDestroyed(string prefabID)
    {
        if (Instance == null)
        {
            Debug.LogError("PrefabManager instance not found.");
            return;
        }

        if (!activePrefabsParts.ContainsKey(prefabID))
        {
            Debug.LogError("Prefab ID not recognized: " + prefabID);
            return;
        }

        activePrefabsParts[prefabID]--;

        if (activePrefabsParts[prefabID] <= 0)
        {
            if (prefabPrototypes.ContainsKey(prefabID))
            {
                GameObject prefabPrototype = prefabPrototypes[prefabID];
                if (prefabPrototype != null)
                {
                    Instantiate(prefabPrototype, Vector3.zero, Quaternion.identity);
                    activePrefabsParts[prefabID] = 2/*重新设置部件计数，根据需要调整*/;
                }
                else
                {
                    Debug.LogError("Prefab prototype is null for ID: " + prefabID);
                }
            }
            else
            {
                Debug.LogWarning("Prefab prototype for ID " + prefabID + " not found.");
            }
        }
    }


    // 在生成Prefab部件时调用
    public void RegisterPart(string prefabID)
    {
        if (!activePrefabsParts.ContainsKey(prefabID))
        {
            activePrefabsParts.Add(prefabID, 1);
            Debug.Log("Registered PrefabPart with ID: " + prefabID);
        }
        else
        {
            activePrefabsParts[prefabID]++;
            Debug.Log("Incremented PrefabPart count for ID: " + prefabID);
        }
    }
}
