using System;

namespace F7s.Utility.Lazies {

    /// <summary>
    /// Remembers the last parameter and result, and returns it from cache on repeated queries.
    /// </summary>
    public class LazyMonoMemory<inType, outType> {
        private outType valueBackingField;
        public bool Initialized { get; private set; }
        private readonly Func<inType, outType> initializer;
        private inType lastIn;

        public LazyMonoMemory(Func<inType, outType> initializer) {
            this.initializer = initializer;
        }
        public virtual outType GetValue(inType key) {
            inType lastIn = this.lastIn;
            inType currentIn = key;

            if (!Equals(lastIn, currentIn)) {
                this.MarkAsDirty();
            }

            this.lastIn = currentIn;

            if (!this.Initialized) {
                this.valueBackingField = this.initializer.Invoke(key);
                this.Initialized = true;
            }

            return this.valueBackingField;
        }

        public void MarkAsDirty() {
            this.valueBackingField = default;
            this.Initialized = false;
        }

        protected virtual outType GetValue() {
            return this.valueBackingField;
        }
    }
}
