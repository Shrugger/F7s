using System;

namespace F7s.Utility.Geometry;

public class Transform3D {
    public Matrix3x3d Basis;
    public Vector3d Origin;

    public static Transform3D Identity;

    public Transform3D (Matrix3x3d basis, Vector3d origin) {
        this.Basis = basis;
        this.Origin = origin;
    }

    public static Transform3D operator * (Transform3D a, Transform3D b) {
        throw new NotImplementedException();
    }

    internal Transform3D Inverse () {
        throw new NotImplementedException();
    }

    internal Transform3D Translated (Vector3d origin) {
        throw new NotImplementedException();
    }
}
