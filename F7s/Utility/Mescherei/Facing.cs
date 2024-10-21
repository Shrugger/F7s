using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Utility.Mescherei {
    public struct Facing {

        public readonly Direction direction;
        public readonly Vector3 relativeTo;

        public Facing (Direction direction, Vector3 relativeTo) {
            this.direction = direction;
            this.relativeTo = relativeTo;
        }

        public enum Direction { Undefined, Outward, Inward, TwoSided }

    }

}
