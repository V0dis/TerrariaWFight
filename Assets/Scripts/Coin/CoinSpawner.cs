using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private List<Transform> _spawnPoints;

    private void Start()
    {
        if (_coinPrefab == null || _spawnPoints == null || _spawnPoints.Count == 0)
            return;
        
        SpawnCoins();
    }

    private void SpawnCoins()
    {
        foreach (var point in _spawnPoints)
        {
            var coin = Instantiate(_coinPrefab, point.position, Quaternion.identity);
            coin.Taken += HandleCollected;
        }
    }

    private void HandleCollected(Coin coin)
    {
        coin.Taken -= HandleCollected;
        
        if (coin != null)
            Destroy(coin.gameObject);
    }
}
