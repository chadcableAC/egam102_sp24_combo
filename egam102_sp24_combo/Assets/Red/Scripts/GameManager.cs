using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [System.Serializable]
    public class Pill
    {
        public string name; // 药丸名称
        public GameObject prefab; // 药丸的Prefab
        public int maxInstances; // 最大实例数
        [HideInInspector] public int currentInstances = 0; // 当前实例数
    }

    [System.Serializable]
    public class Container
    {
        public string name; // 容器名称
        public GameObject lidPrefab; // 盖子的Prefab
        public GameObject containerPrefab; // 容器的Prefab
        public Transform spawnPoint; // 生成位置
        [HideInInspector] public bool isPresent = false; // 场景中是否存在
    }

    public List<Pill> pills = new List<Pill>();
    public List<Container> containers = new List<Container>();
    private Dictionary<string, int> activePrefabsParts = new Dictionary<string, int>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()

    {

        EnsureMinimumContainerPresence();
    }

    void Update()
    {
        // 可以添加条件，以控制检查频率，例如每几秒检查一次
        EnsureMinimumContainerPresence();
    }

    void EnsureMinimumContainerPresence()
    {
        foreach (var container in containers)
        {
            if (!ContainerIsPresent(container.name))
            {
                SpawnContainer(container);
            }
        }
    }
    bool ContainerIsPresent(string containerName)
    {
        // 检查场景中是否存在指定名称的容器实例
        var containerParts = GameObject.FindGameObjectsWithTag("Container");
        foreach (var part in containerParts)
        {
            // 这里检查对象的名称是否与预期匹配，同时避免将“Clone”字样的对象误判为原始对象
            if (part.name.StartsWith(containerName))
            {
                return true; // 找到至少一个部件匹配容器名称，说明容器存在
            }
        }
        return false;
    }

    void SpawnContainer(Container container)
    {
        Instantiate(container.lidPrefab, container.spawnPoint.position, Quaternion.identity);
        Instantiate(container.containerPrefab, container.spawnPoint.position, Quaternion.identity);
        container.isPresent = true; // 在这里设置为true意味着我们假设场景中至少有一个实例存在
    }
    public void TrySpawnPill(string pillName)
    {
        foreach (var pill in pills)
        {
            if (pill.name == pillName && pill.currentInstances < pill.maxInstances)
            {
                Instantiate(pill.prefab, Vector3.zero, Quaternion.identity);
                pill.currentInstances++;
                break;
            }
        }
    }

    public void PartDestroyed(string prefabID)
    {
        // 当部件被销毁时调用
        if (activePrefabsParts.ContainsKey(prefabID))
        {
            activePrefabsParts[prefabID]--;
            if (activePrefabsParts[prefabID] <= 0)
            {
                foreach (var container in containers)
                {
                    if (container.name == prefabID)
                    {
                        container.isPresent = false; // 设置为false，触发在Update中的重生检查
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Prefab ID not recognized: " + prefabID);
        }
    }

    public void RegisterPart(string prefabID)
    {
        if (!activePrefabsParts.ContainsKey(prefabID))
        {
            activePrefabsParts.Add(prefabID, 1);
        }
        else
        {
            activePrefabsParts[prefabID]++;
        }
    }

    private void CheckAndRespawnContainer(string prefabID)
    {
        foreach (var container in containers)
        {
            if (container.name == prefabID && !container.isPresent)
            {
                Instantiate(container.lidPrefab, container.spawnPoint.position, Quaternion.identity);
                Instantiate(container.containerPrefab, container.spawnPoint.position, Quaternion.identity);
                container.isPresent = true;
                activePrefabsParts[prefabID] = 2; // 重置部件计数
                break;
            }
        }
    }
}

    // 使用此方法在游戏启动时检查并确保至少有一个容器存在
    
