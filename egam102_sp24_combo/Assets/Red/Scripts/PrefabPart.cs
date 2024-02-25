using System.Collections;
using UnityEngine;

public class PrefabPart : MonoBehaviour
{
    public PrefabManager manager; // 在Inspector中设置
    public string prefabID; // 在Inspector中为每个Prefab部件设置唯一ID

    void OnDestroy()
    {
        if (PrefabManager.Instance != null)
        {
            PrefabManager.Instance.PartDestroyed(prefabID);
        }
        else
        {
            Debug.LogError("PrefabManager instance not found.");
        }
    }

    void Start()
    {
        if (PrefabManager.Instance == null)
        {
            StartCoroutine(WaitAndInitialize());
        }
        else
        {
            InitializeNow();
        }
    }

    IEnumerator WaitAndInitialize()
    {
        yield return new WaitForSeconds(0.1f); // 等待0.1秒
        if (PrefabManager.Instance != null)
        {
            InitializeNow();
        }
        else
        {
            Debug.LogError("PrefabManager instance still not found after waiting.");
        }
    }

    void InitializeNow()
    {
        // 初始化代码，比如注册PrefabPart
        PrefabManager.Instance.RegisterPart(prefabID);
    }
}
