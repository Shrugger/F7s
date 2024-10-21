using F7s.Utility.Geometry.Double;

namespace F7s.Utility.Geometry {

    public struct Matrix3x3d {

        public double m00;
        public double m01;
        public double m02;
        public double m10;
        public double m11;
        public double m12;
        public double m20;
        public double m21;
        public double m22;

        public Matrix3x3d (
            double i00,
            double i01,
            double i02,
            double i10,
            double i11,
            double i12,
            double i20,
            double i21,
            double i22
        ) {
            m00 = i00;
            m01 = i01;
            m02 = i02;
            m10 = i10;
            m11 = i11;
            m12 = i12;
            m20 = i20;
            m21 = i21;
            m22 = i22;
        }

        public static Matrix3x3d operator * (Matrix3x3d m1, Matrix3x3d m2) {
            return new Matrix3x3d(
                                  i00: m1.m00 * m2.m00,
                                  i01: m1.m01 * m2.m10,
                                  i02: m1.m02 * m2.m20,
                                  i10: m1.m10 * m2.m02,
                                  i11: m1.m11 * m2.m11,
                                  i12: m1.m12 * m2.m21,
                                  i20: m1.m20 * m2.m02,
                                  i21: m1.m21 * m2.m12,
                                  i22: m1.m22 * m2.m22
                                 );
        }

        public static Vector3d operator * (Matrix3x3d m, Vector3d v) {
            return (new Vector3d(x: m.m00, y: m.m10, z: m.m20) * v.X)
                 + (new Vector3d(x: m.m01, y: m.m11, z: m.m21) * v.Y)
                 + (new Vector3d(x: m.m02, y: m.m12, z: m.m22) * v.Z);
        }

        public static string ToString (Matrix3x3d m) {
            return m.m00
                 + ", "
                 + m.m01
                 + ", "
                 + m.m02
                 + "\n"
                 + m.m10
                 + ", "
                 + m.m11
                 + ", "
                 + m.m12
                 + "\n"
                 + m.m20
                 + ", "
                 + m.m21
                 + ", "
                 + m.m22;
        }

    }

}