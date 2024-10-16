using F7s.Modell.Abstract;

namespace F7s.Modell.Economics.Resources {
    /// <summary>
    /// Resources are abstract; they are NOT physical objects. Thus resources should not have any instance-specific information.
    /// </summary>
    public sealed class Resource : GameEntity {
        public readonly ResourceType Type;

        public Resource (ResourceType type, float mass) {
            Type = type;
            Quantity = mass;
        }

        public override string ToString () {
            return GenerateName(Type, Quantity);
        }

        public static string GenerateName (ResourceType type, float quantity) {
            return type + " x" + quantity;
        }

        // TODO: Replace mass with Quantity
        public float Quantity { get; private set; }

        public bool IsMatchedBy (Resource other) {
            return IsMatchedBy(other.Type, other.Quantity);
        }

        public bool Matches (Resource other) {
            return Matches(other.Type, other.Quantity);
        }

        public bool Matches (ResourceType type, float quantity) {
            return type == this.Type && this.Quantity >= quantity;
        }

        public bool IsMatchedBy (ResourceType type, float quantity) {
            return type == this.Type && this.Quantity <= quantity;
        }
    }
}
