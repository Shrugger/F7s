using F7s.Modell.Handling;

namespace F7s.Modell.Abstract {
    public class GameValue<T> : IGameValue {
        public T Value { get; private set; }

        public GameValue (T value) {
            this.Value = value;
        }

        public override string ToString () {
            return Value.ToString();
        }

        public static implicit operator GameValue<T> (T value) {
            return new GameValue<T>(value);
        }
        public static implicit operator T (GameValue<T> value) {
            return value.Value;
        }

        public void Set (T value) {
            this.Value = value;
        }

        public void ConfigureContextMenu (ContextMenu contextMenu) {
            return;
        }

        public void ConfigureInfoblock (Infoblock infoblock) {
            infoblock.Text = Value.ToString();
        }
    }

    public class GameFloat : GameValue<float> {
        public GameFloat (float value) : base(value) {
        }
    }

    public class GameBool : GameValue<bool> {
        public GameBool (bool value) : base(value) {
        }
    }

    public class GameInt : GameValue<int> {
        public GameInt (int value) : base(value) {
        }
    }
    public class GameLong : GameValue<long> {
        public GameLong (long value) : base(value) {
        }

        public static implicit operator GameLong (long value) {
            return new GameLong(value);
        }
    }

}
