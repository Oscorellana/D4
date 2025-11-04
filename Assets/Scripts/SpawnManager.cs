using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject dummyPrefab;         // Your test dummy prefab
    public Transform[] spawnPoints;        // Empty GameObjects marking spawn positions
    public int numberToSpawn = 5;          // How many dummies to spawn

    void Start()
    {
        SpawnDummies();
    }

    void SpawnDummies()
    {
        if (dummyPrefab == null || spawnPoints.Length == 0) return;

        for (int i = 0; i < numberToSpawn; i++)
        {
            // Pick a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            // Instantiate the dummy at that location
            Instantiate(dummyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
