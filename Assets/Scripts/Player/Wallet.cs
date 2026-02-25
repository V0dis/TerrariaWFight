using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int _coins = 0;

    public int Coins => _coins;

    public void AddCoins(int amount)
        => _coins += amount;
}
