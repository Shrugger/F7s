using Stride.Core.Mathematics;
using System;

namespace F7s.Utility.Geometry;

public struct Basis {
    Matrix3x3d matrix;

    public Basis (Quaternion rotation) : this() {
        throw new NotImplementedException();
    }

    public static Matrix3x3d Identity => Matrix3x3d.Identity;

    public Vector3 Scale { get { throw new NotImplementedException(); } }

    internal static Matrix3x3d FromEuler (Vector3 rotation) {
        throw new NotImplementedException();
    }

    public static implicit operator Matrix3x3d (Basis basis) {
        return basis.matrix;
    }
}
