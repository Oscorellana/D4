using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public MapData[] maps;
    public MapData startingMap;

    public MapData currentMap;
    public Transform player;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (startingMap == null)
        {
            Debug.LogError("MapManager: No starting map assigned!");
            return;
        }

        LoadMap(startingMap);
    }

    public void LoadMap(MapData map)
    {
        currentMap = map;

        if (map.playerSpawnPoint == null)
        {
            Debug.LogError("MapManager: Player spawn point missing on map!");
            return;
        }

        Debug.Log($"Loading Map: {map.name}");

        // Teleport player
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.position = map.playerSpawnPoint.position;
        player.rotation = map.playerSpawnPoint.rotation;

        if (cc != null) cc.enabled = true;


        // Reset and start waves
        SpawnManager.Instance.ResetForNewMap();
        SpawnManager.Instance.StartNextWave();
    }

    public MapData GetRandomMap()
    {
        return maps[Random.Range(0, maps.Length)];
    }
}
