using UnityEngine;

public class MapData : MonoBehaviour
{
    [Header("Map Info")]
    public string mapName;

    [Header("Player")]
    public Transform playerSpawnPoint;

    [Header("Enemies")]
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPoints;
}
