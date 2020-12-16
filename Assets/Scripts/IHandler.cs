using System.Collections.Generic;

public interface IHandler<TKey,TValue> where TValue : IItem<TKey>
{
    Dictionary<TKey, TValue> Items { get; }
    void Add(TValue _value);
    void Remove(TKey _key);
    bool Exists(TKey _key);
    TValue Get(TKey _key);
    void Enable(TKey _key);
    void Disable(TKey _key);
}
