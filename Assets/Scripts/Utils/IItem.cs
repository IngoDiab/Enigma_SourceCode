public interface IItem<TKey>
{
    /// <summary>
    /// Item id
    /// </summary>
    TKey ID { get; }
    /// <summary>
    /// True if the item is Enable, false if not
    /// </summary>
    bool IsEnabled { get; }
    /// <summary>
    /// Enable the item
    /// </summary>
    void Enable();
    /// <summary>
    /// Disable the item
    /// </summary>
    void Disable();
}
