using UnityEngine;

public class Coin : PickupableItem
{
    [SerializeField] private int _value = 1;
    
    public int Value => _value;
}