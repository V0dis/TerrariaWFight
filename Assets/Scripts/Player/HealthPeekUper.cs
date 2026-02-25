using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPeekUper : MonoBehaviour
{
    public event Action<HealthPack> HealthPackPickedUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out HealthPack healthPack)) 
        {
            HealthPackPickedUp?.Invoke(healthPack);
        }
    }

    public void PickUp(HealthPack healthPack)
    {
        healthPack.Take();
    }
}
