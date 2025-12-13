using UnityEngine;

public class MapData : MonoBehaviour
{
    public string mapName;
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;

    void Awake()
    {
        if (string.IsNullOrEmpty(mapName))
            mapName = gameObject.name;
    }
}
