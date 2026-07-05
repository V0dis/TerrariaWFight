using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PickupableItem : MonoBehaviour
{
    public event Action<PickupableItem> Collected;
    
    public void Collect()
    {
        Collected?.Invoke(this);
    }
}