using System.Collections.Generic;

namespace GeoLibrary.IO.GeoJson
{
    internal class JsonObject
    {
        private readonly Dictionary<string, object> _members;

        public int Count => _members?.Count ?? 0;

        public JsonObject()
        {
            _members = new Dictionary<string, object>();
        }

        public object this[string key]
        {
            get { return _members[key]; }
        }

        public bool ContainsKey(string key)
        {
            return _members.ContainsKey(key);
        }

        public void Add(string key, object value)
        {
            _members.Add(key, value);
        }

        public void Clear()
        {
            _members.Clear();
        }
    }
}
