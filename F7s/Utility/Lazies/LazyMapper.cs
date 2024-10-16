using System;
using System.Collections.Generic;

namespace F7s.Utility.Lazies {

    public class LazyMapper<KeyType, ValueType> {
        private readonly Dictionary<KeyType, ValueType> map = new Dictionary<KeyType, ValueType>();
        private readonly Func<KeyType, ValueType> populator;

        public LazyMapper(Func<KeyType, ValueType> populator) {
            this.populator = populator;
        }

        public ValueType Get(KeyType key) {
            if (this.map.ContainsKey(key)) {
                return this.map[key];
            } else {
                ValueType value = this.populator(key);
                this.map.Add(key, value);
                return value;
            }
        }
    }
}
