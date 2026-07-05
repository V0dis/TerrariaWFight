using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemCollector : MonoBehaviour
{
    public event Action<int> CoinPickedUp;
    public event Action<float> HealthPackPickedUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PickupableItem pickupable) 
            && other.TryGetComponent(out Collider2D itemCollider))
        {
            CollectItem(pickupable);
        }
    }

    private void CollectItem(PickupableItem pickupableItem)
    {
        if (pickupableItem is Coin coin)
        {
            CoinPickedUp?.Invoke(coin.Value);
        }
        else if (pickupableItem is HealthPack healthPack)
        {
            HealthPackPickedUp?.Invoke(healthPack.HealAmount);
        }
        else
        {
            return;
        }
        
        pickupableItem.Collect();
    }
}
