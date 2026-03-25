using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemCollector : MonoBehaviour
{
    public event Action<int> CoinPickedUp;
    public event Action<float> HealthPackPickedUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IPickupable pickupable) 
            && other.TryGetComponent(out Collider2D itemCollider))
        {
            itemCollider.enabled = false;
            
            CollectItem(pickupable);
        }
    }

    private void CollectItem(IPickupable pickupable)
    {
        if (pickupable is Coin coin)
        {
            CoinPickedUp?.Invoke(coin.Value);
        }
        else if (pickupable is HealthPack healthPack)
        {
            HealthPackPickedUp?.Invoke(healthPack.HealAmount);
        }
        else
        {
            return;
        }
        
        pickupable.Collect();
    }
}
