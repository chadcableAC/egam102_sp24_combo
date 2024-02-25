/*using UnityEngine;

public class TriggerAreaChecker : MonoBehaviour
{
    private int count = 0;
    public EgamMicrogameInstance microgameInstance; // 确保在 Inspector 中设置此引用

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpawnedPrefab"))
        {
            count++;
            if (count >= MedicationSpawner.numberOfPrefabsSpawned)
            {
                microgameInstance.WinGame(); // 确保实现了 WinGame 方法
                Debug.Log("Player Wins!");
            }
        }
    }
}*/
