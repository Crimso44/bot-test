
namespace ChatBot.Admin.CommonServices.Services.Abstractions
{
    public interface ITempCache
    {
        TItem Get<TKey, TItem>(TKey key);
        void Set<TKey, TItem>(TKey key, TItem item);
        void Remove<TKey>(TKey key);
    }
}
