using UnityEngine;

public class PickupableSpawner : ItemSpawner<PickupableItem>
{
    protected override void Spawn()
    {
        var spawnPoints = GetSpawnPoints();
        
        foreach (var point in spawnPoints)
        {
            if (point == null)
                continue;

            var item = Instantiate(_prefab, point.position, Quaternion.identity);
            
            item.Collected += HandleCollected;
        }
    }
    
    private void HandleCollected(PickupableItem collectedItem)
    {
        collectedItem.Collected -= HandleCollected;
        Destroy(collectedItem.gameObject);
    }
}