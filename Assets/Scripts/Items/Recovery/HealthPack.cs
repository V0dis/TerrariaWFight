using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPack : MonoBehaviour, IPickupable
{
    [SerializeField] private float _healAmount = 25f;
    
    public event Action<IPickupable> Collected;
    
    public float HealAmount => _healAmount;

    public void Collect()
    {
        Collected?.Invoke(this);
    }
}
