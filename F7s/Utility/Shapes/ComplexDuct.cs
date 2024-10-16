using Stride.Core.Mathematics;
using System;
namespace F7s.Utility.Shapes {
    [Serializable]
    public class ComplexDuct : ComplexHollowBox {
        public ComplexDuct (Vector3 fullExtents, float wallThickness) : this(fullExtents.X, fullExtents.Y, fullExtents.Z, wallThickness) { }

        public ComplexDuct (float width, float height, float length, float wallThickness) : base(new Vector3(width, height, length), wallThickness, true, true, true, true, false, false) {
        }

    }

}