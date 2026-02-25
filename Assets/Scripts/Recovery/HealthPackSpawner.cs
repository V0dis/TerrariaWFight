using System.Collections.Generic;
using UnityEngine;

public class HealthPackSpawner : MonoBehaviour
{
    [SerializeField] private HealthPack _healthPackPrefab;
    [SerializeField] private int _maxPacksOnScene = 2;
    [SerializeField] private List<Transform> _spawnPoints;
    
    private Queue<Transform> _spawnPointsQueue;
    
    private void Start()
    {
        CreateRandomSpawnPointsQueue();
        SpawnPacks();
    }

    private void SpawnPacks()
    {
        if (_spawnPointsQueue.Count == 0)
            return;
        

        for (int i = 0; i < _maxPacksOnScene; i++)
        {
            var healthPack = Instantiate(
                _healthPackPrefab,
                _spawnPointsQueue.Dequeue().position,
                Quaternion.identity
            );

            healthPack.Taken += DestroyPack;
        }
    }

    private void DestroyPack(HealthPack healthPack)
    {
        healthPack.Taken -= DestroyPack;
        Destroy(healthPack.gameObject);
    }

    private void CreateRandomSpawnPointsQueue()
    {
        _spawnPointsQueue = new Queue<Transform>();

        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            _spawnPointsQueue.Enqueue(_spawnPoints[Random.Range(0, _spawnPoints.Count)]);
        }
    }
}
