using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    public List<MapData> maps = new List<MapData>();
    public MapData currentMap;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        maps.AddRange(FindObjectsOfType<MapData>());
    }

    public MapData GetRandomMap(MapData exclude = null)
    {
        List<MapData> pool = new List<MapData>(maps);
        if (exclude != null) pool.Remove(exclude);
        return pool[Random.Range(0, pool.Count)];
    }

    public void TeleportPlayerToMap(MapData map)
    {
        PlayerController player = FindFirstObjectByType<PlayerController>();
        if (player == null) return;

        CharacterController cc = player.GetComponent<CharacterController>();
        cc.enabled = false;
        player.transform.position = map.playerSpawnPoint.position;
        player.transform.rotation = map.playerSpawnPoint.rotation;
        cc.enabled = true;

        currentMap = map;
    }
}
