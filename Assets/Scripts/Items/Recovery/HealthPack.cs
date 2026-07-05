using UnityEngine;

public class HealthPack : PickupableItem
{
    [SerializeField] private float _healAmount = 25f;

    public float HealAmount => _healAmount;
}