using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository
{
    public interface ISessionRepository
    {
        void SaveValues(string key, IEnumerable<KeyValuePair<string, string>> values);
        IEnumerable<KeyValuePair<string, string>> RetrieveValues(string key);
        void RemoveValues(string key);
    }

    public class SessionRepository : ISessionRepository
    {
        // todo: change out to be database
        private readonly MemoryCache _cache = MemoryCache.Default;

        public void SaveValues(string key, IEnumerable<KeyValuePair<string, string>> values)
        {
            var existing = RetrieveValues(key);
            var final = existing.Where(x => values.All(y => y.Key != x.Key)).Union(values).ToArray();
            _cache.Set(new CacheItem(key, final), new CacheItemPolicy());
        }

        public IEnumerable<KeyValuePair<string, string>> RetrieveValues(string key)
        {
            var exiting = _cache.GetCacheItem(key);
            if (exiting == null) return Enumerable.Empty<KeyValuePair<string, string>>();

            return exiting.Value as IEnumerable<KeyValuePair<string, string>>;
        }

        public void RemoveValues(string key)
        {
            _cache.Remove(key);
        }
    }
}