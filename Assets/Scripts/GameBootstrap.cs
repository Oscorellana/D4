using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    void Start()
    {
        Debug.Log("GameBootstrap START");

        SpawnManager spawnManager = FindFirstObjectByType<SpawnManager>();
        if (spawnManager == null)
        {
            Debug.LogError("SpawnManager NOT FOUND in scene!");
            return;
        }

        MapData startMap = FindFirstObjectByType<MapData>();
        if (startMap == null)
        {
            Debug.LogError("No MapData found in scene!");
            return;
        }

        MapManager.Instance.currentMap = startMap;
        MapManager.Instance.TeleportPlayerToMap(startMap);

        spawnManager.StartWave();
    }
}
