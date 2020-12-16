public interface IItem<TKey>
{
    TKey ID { get; }
    bool IsEnabled { get; }
    void Enable();
    void Disable();
}
