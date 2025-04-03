namespace Inde;
public interface IMcMemoryCache<TItem>
{
    Task<TItem> Get(object key);
    Task<TItem> Set(object key, TItem item);
}