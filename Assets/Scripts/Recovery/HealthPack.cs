using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthPack : MonoBehaviour
{
    [SerializeField] private float _value = 25f;

    public event Action<HealthPack> Taken;

    public float Value => _value;

    public void Take()
    {
        Taken?.Invoke(this);
    }
}
