using Stride.Core.Mathematics;
using System;

namespace F7s.Utility.Geometry;

[Obsolete("Replace with Stride transformation Matrix.")]
public class Transform3D {
    public Matrix3x3d Basis;
    public Vector3d Origin;

    private Matrix matrix;

    public static Transform3D Identity;

    public Transform3D (Vector3d translation, Matrix3x3d rotation) {
        Basis = rotation;
        Origin = translation;

        Quaternion rotation;
        Vector3 translation;
        matrix = Matrix.AffineTransformation(1, rotation, translation);
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
        return Matrix.AffineTransformation(1, Basis.GetRotationQuaternion(), Origin.ToVector3());
    }

    public Vector3 Transform (Vector3 v) {
        Matrix translation;
        Matrix.Translation(in v, out translation);
        Matrix transformation = ToStrideMatrix();
        return (transformation * translation).TranslationVector;
    }
}
