namespace BigODungeon;

// Counts "operations" so the player can SEE how much work an algorithm does.
public class OpCounter
{
    public int Count { get; private set; }

    // Fires every time the count changes (the form uses this to update the label)
    public event Action<int>? Changed;

    public void Add(int amount = 1)
    {
        Count += amount;
        Changed?.Invoke(Count);
    }

    public void Reset()
    {
        Count = 0;
        Changed?.Invoke(Count);
    }
}