using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawning Settings")]
    public GameObject dummyPrefab;
    public Transform[] spawnPoints;
    public int maxDummies = 5;          // Max number of dummies in the scene
    public float respawnDelay = 3f;     // Time before respawning a dead dummy

    private int currentDummies = 0;

    void Start()
    {
        SpawnInitialDummies();
    }

    void SpawnInitialDummies()
    {
        for (int i = 0; i < maxDummies; i++)
        {
            SpawnDummy();
        }
    }

    public void SpawnDummy()
    {
        if (dummyPrefab == null || spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject dummy = Instantiate(dummyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Increment count
        currentDummies++;

        // Subscribe to dummy death to respawn
        TargetDummy target = dummy.GetComponent<TargetDummy>();
        if (target != null)
        {
            target.OnDeath += () => StartCoroutine(RespawnAfterDelay());
        }
    }

    System.Collections.IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnDummy();
    }
}
