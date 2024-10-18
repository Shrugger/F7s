using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;

namespace F7s.Geometry;

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

    public Matrix ToStrideMatrix () {
        // TODO: This is ridiulous.
        return Matrix.AffineTransformation(1, this.Basis.GetRotationQuaternion(), this.Origin.ToVector3());
    }

    public Vector3 Transform (Vector3 v) {
        Matrix translation;
        Matrix.Translation(in v, out translation);
        Matrix transformation = ToStrideMatrix();
        return (transformation * translation).TranslationVector;
    }
}
