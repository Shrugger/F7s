using System; using F7s.Utility.Geometry.Double;

namespace F7s.Utility.Lazies {

    public class Faul<T> {
        private T valueBackingField;
        public bool Initialized { get; private set; }
        private readonly Func<T> initializer;

        public Faul(Func<T> initializer) {
            this.initializer = initializer;
        }

        protected virtual T GetValue() {
            if (!this.Initialized) {
                this.Initialize();
            }
            return this.valueBackingField;
        }

        public T Value {
            get {
                return this.GetValue();
            }
        }

        public void Initialize() {
            this.valueBackingField = this.initializer.Invoke();
            this.Initialized = true;
        }

        public void MarkAsDirty() {
            this.valueBackingField = default;
            this.Initialized = false;
        }

        public bool HasValue() {
            return this.Initialized;
        }

        public static implicit operator T(Faul<T> faul) { return faul.Value; }

        public static implicit operator bool(Faul<T> faul) {
            return faul != null && faul.HasValue();
        }

        public override bool Equals(object obj) {
            if (this.HasValue() && obj is T) {
                return this.Value.Equals((T)obj);
            } else {
                return base.Equals(obj);
            }
        }

        public override string ToString() {
            if (this.HasValue()) {
                return this.Value.ToString();
            } else {
                return (this.Initialized ? "Initialized " : "Uninitialized ") + this.GetType().Name + " of " + typeof(T).Name;
            }
        }

        public override int GetHashCode() {
            return this.Value.GetHashCode();
        }
    }
}
