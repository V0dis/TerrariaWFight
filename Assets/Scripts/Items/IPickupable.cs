using System;

public interface IPickupable
{
    public event Action<IPickupable> Collected;
    
    public void Collect();
}
