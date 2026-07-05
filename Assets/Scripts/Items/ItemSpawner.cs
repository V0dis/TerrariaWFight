using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawner<T> : MonoBehaviour where T : PickupableItem
{
    [SerializeField] private int _countItems;
    [SerializeField] private bool _isAllPoints;
    [SerializeField] private List<Transform> _spawnPoints = new();
    
    [SerializeField] protected T _prefab;

    private void Start()
    {
        if (_prefab == null || _spawnPoints.Count == 0)
            return;

        Spawn();
    }

    protected virtual void Spawn()
    {
        var spawnPoints = GetSpawnPoints();
        
        foreach (var point in spawnPoints)
        {
            if (point == null)
                continue;

            var item = Instantiate(_prefab, point.position, Quaternion.identity);
        }
    }

    protected IEnumerable<Transform> GetSpawnPoints()
    {
        if (_isAllPoints || _countItems >= _spawnPoints.Count)
            return _spawnPoints;

        return _spawnPoints.OrderBy(x => Guid.NewGuid()).Take(_countItems);
    }
}