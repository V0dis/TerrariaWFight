using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private int _value = 1;

    public event Action<Coin> Taken;

    public int Value => _value;

    public void Take()
    {
        Taken?.Invoke(this);
    }
}
