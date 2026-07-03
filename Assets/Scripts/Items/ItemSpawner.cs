using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _countItems;
    [SerializeField] private bool _isAllPoints;
    [SerializeField] private List<Transform> _spawnPoints = new();

    private void Start()
    {
        if (_prefab == null || _spawnPoints.Count == 0)
            return;

        Spawn();
    }

    private void Spawn()
    {
        var spawnPoints = GetSpawnPoints();
        
        foreach (var point in spawnPoints)
        {
            if (point == null)
                continue;

            var item = Instantiate(_prefab, point.position, Quaternion.identity);
            
            if (item is IPickupable pickupable)
                pickupable.Collected += HandleCollected;
        }
    }

    private IEnumerable<Transform> GetSpawnPoints()
    {
        if (_isAllPoints || _countItems >= _spawnPoints.Count)
            return _spawnPoints;

        return _spawnPoints.OrderBy(x => Guid.NewGuid()).Take(_countItems);
    }

    private void HandleCollected(IPickupable collectedItem)
    {
        if (collectedItem is MonoBehaviour mono)
        {
            collectedItem.Collected -= HandleCollected;
            Destroy(mono.gameObject);
        }
    }
}