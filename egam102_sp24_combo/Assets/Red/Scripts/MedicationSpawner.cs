/*using UnityEngine;

public class MedicationSpawner : MonoBehaviour
{
    public MedicationManager medicationManager;
    public Transform spawnPoint;
    public int minNumberOfPrefabsToSpawn = 2;
    public int maxNumberOfPrefabsToSpawn = 5;
    public static int numberOfPrefabsSpawned; // 供 TriggerAreaChecker 使用

    void Start()
    {
        SpawnRandomPills();
    }

    void SpawnRandomPills()
    {
        numberOfPrefabsSpawned = 0;
        int numberOfPrefabsToSpawn = Random.Range(minNumberOfPrefabsToSpawn, maxNumberOfPrefabsToSpawn + 1);

        for (int i = 0; i < numberOfPrefabsToSpawn; i++)
        {
            MedicationManager.Pill pill = medicationManager.pills[Random.Range(0, medicationManager.pills.Count)];
            if (pill.currentInstances < pill.maxInstances)
            {
                Instantiate(pill.prefab, spawnPoint.position, Quaternion.identity).tag = "SpawnedPrefab";
                pill.currentInstances++;
                numberOfPrefabsSpawned++;
            }
        }
    }
}
*/