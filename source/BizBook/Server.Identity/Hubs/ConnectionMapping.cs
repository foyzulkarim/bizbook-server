using System.Collections.Generic;
using System.Linq;

namespace Server.Identity.Hubs
{
    using System;

    public class ConnectionModel
    {
        public string ConnectionId { get; set; }
        public string UserId { get; set; }
        public string ShopId { get; set; }
        public DateTime SessionStarted { get; set; }
    }   
    
    public class ConnectionMapping<TKey,TValue> where TValue : ConnectionModel, new()
    {
        private readonly Dictionary<TKey, HashSet<TValue>> _connections =
            new Dictionary<TKey, HashSet<TValue>>();

        public int ConnectionCount
        {
            get { return _connections.Values.Sum(x => x.Count()); }
        }

        public int UserCount
        {
            get
            {
                lock (_connections)
                {
                    return _connections.Count();
                }
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (_connections)
            {
                HashSet<TValue> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<TValue>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {                    
                    connections.Add(value);
                }
            }
        }

        public IEnumerable<TValue> GetConnections(TKey key)
        {
            HashSet<TValue> connections;
            lock (_connections)
            {
                if (_connections.TryGetValue(key, out connections))
                {
                    return connections;
                }
            }

            return Enumerable.Empty<TValue>();
        }

        public TKey GetConnectionById(string connectionId)
        {
            if (_connections.Count > 0)
                lock (_connections)
                {
                    var pair = _connections.First(x => x.Value.Any(y => y.ConnectionId == connectionId));
                    return pair.Key;
                }
            return default(TKey);
        }

        public void Remove(TKey key, string connectionId)
        {
            if (key==null)
            {
                return;
            }
            lock (_connections)
            {
                HashSet<TValue> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    TValue value = connections.First(x => x.ConnectionId == connectionId);
                    connections.Remove(value);
                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }

       
    }
}