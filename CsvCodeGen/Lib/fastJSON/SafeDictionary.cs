﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace fastJSON
{
    internal class SafeDictionary<TKey, TValue>
    {
        private readonly object _Padlock = new object();
        private readonly Dictionary<TKey, TValue> _Dictionary = null;

        public SafeDictionary(int capacity)
        {
            _Dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public SafeDictionary()
        {
            _Dictionary = new Dictionary<TKey, TValue>();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_Padlock)
                return _Dictionary.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_Padlock)
                    return _Dictionary[key];
            }
            set
            {
                lock (_Padlock)
                    _Dictionary[key] = value;
            }
        }

        public int Count
        {
            get { lock (_Padlock) return _Dictionary.Count; }
        }

        public ICollection<KeyValuePair<TKey, TValue>> GetList()
        {
            return (ICollection<KeyValuePair<TKey, TValue>>)_Dictionary;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)_Dictionary).GetEnumerator();
        }

        public void Add(TKey key, TValue value)
        {
            lock (_Padlock)
            {
                if (_Dictionary.ContainsKey(key) == false)
                    _Dictionary.Add(key, value);
            }
        }

        public TKey[] Keys()
        {
            lock (_Padlock)
            {
                TKey[] keys = new TKey[_Dictionary.Keys.Count];
                _Dictionary.Keys.CopyTo(keys, 0);
                return keys;
            }
        }

        public bool Remove(TKey key)
        {
            lock (_Padlock)
                return _Dictionary.Remove(key);
        }
    }

}
