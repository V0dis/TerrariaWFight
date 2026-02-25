using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CoinsCollector : MonoBehaviour
{
    public event Action<int> CoinPickedUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Coin coin))
        {
            CoinPickedUp?.Invoke(coin.Value);
            coin.Take();
        }
    }
}
