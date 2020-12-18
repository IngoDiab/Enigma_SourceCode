using System.Collections.Generic;

public interface IHandler<TKey,TValue> where TValue : IItem<TKey>
{
    /// <summary>
    /// Items Handled
    /// </summary>
    Dictionary<TKey, TValue> Items { get; }
    /// <summary>
    /// Add an item
    /// </summary>
    /// <param name="_value">Value to add</param>
    void Add(TValue _value);
    /// <summary>
    /// Remove an item handled
    /// </summary>
    /// <param name="_key">Key to remove</param>
    void Remove(TKey _key);
    /// <summary>
    /// Check the item is handled
    /// </summary>
    /// <param name="_key">Key to check</param>
    /// <returns></returns>
    bool Exists(TKey _key);
    /// <summary>
    /// Get the item handled
    /// </summary>
    /// <param name="_key">Key to get</param>
    /// <returns></returns>
    TValue Get(TKey _key);
    /// <summary>
    /// Enable the item
    /// </summary>
    /// <param name="_key"></param>
    void Enable(TKey _key);
    /// <summary>
    /// Disable the item
    /// </summary>
    /// <param name="_key"></param>
    void Disable(TKey _key);
}
