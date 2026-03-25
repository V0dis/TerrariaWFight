using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour, IPickupable
{
    [SerializeField] private int _value = 1;

    public event Action<IPickupable> Collected;

    public int Value => _value;

    public void Collect()
    {
        Collected?.Invoke(this);
    }
}
