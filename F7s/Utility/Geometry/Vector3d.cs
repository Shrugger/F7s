using F7s.Utility.Mathematics;
using System;
using System.Collections.Generic;
using Stride.Core.Mathematics;

namespace F7s.Utility.Geometry {


    public struct QuaternionD {
        public double w;
        public double x;
        public double y;
        public double z;

        public QuaternionD (double w, double x, double y, double z) {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public enum Direction { Undefined, Left, Right, Up, Down, Forward, Back }
    [Serializable]
    public struct Vector3d : Coordinates {

        public const float kEpsilon = 1E-05f;
        public double X;
        public double Y;
        public double Z;

        public double this[int index] {
            get {
                switch (index) {
                    case 0:
                        return this.X;
                    case 1:
                        return this.Y;
                    case 2:
                        return this.Z;
                    default:
                        throw new IndexOutOfRangeException(message: "Invalid index!");
                }
            }
            set {
                switch (index) {
                    case 0:
                        this.X = value;

                        break;
                    case 1:
                        this.Y = value;

                        break;
                    case 2:
                        this.Z = value;

                        break;
                    default:
                        throw new IndexOutOfRangeException(message: "Invalid Vector3d index!");
                }
            }
        }

        public static double DegreesToFractions => 1.0 / 360.0;

        public static double FractionsToDegrees => 360.0;

        public static Vector3d Zero => new Vector3d(x: 0d, y: 0d, z: 0d);

        public static Vector3d one => new Vector3d(x: 1d, y: 1d, z: 1d);

        public static Vector3d forward => new Vector3d(x: 0d, y: 0d, z: 1d);

        public static Vector3d back => new Vector3d(x: 0d, y: 0d, z: -1d);

        public static Vector3d up => new Vector3d(x: 0d, y: 1d, z: 0d);

        public static Vector3d down => new Vector3d(x: 0d, y: -1d, z: 0d);

        public static Vector3d left => new Vector3d(x: -1d, y: 0d, z: 0d);

        public static Vector3d right => new Vector3d(x: 1d, y: 0d, z: 0d);

        public static Vector3d Forward (double scaleFactor) {
            return forward * scaleFactor;
        }

        public static Vector3d Back (double scaleFactor) {
            return back * scaleFactor;
        }

        public static Vector3d Right (double scaleFactor) {
            return right * scaleFactor;
        }

        public static Vector3d Left (double scaleFactor) {
            return left * scaleFactor;
        }

        public static Vector3d Up (double scaleFactor) {
            return up * scaleFactor;
        }

        public static Vector3d Down (double scaleFactor) {
            return down * scaleFactor;
        }

        public Vector3d normalized => Normalize(value: this);

        public double magnitude => Math.Sqrt(this.X * this.X + this.Y * this.Y + this.Z * this.Z);

        public double sqrMagnitude => this.X * this.X + this.Y * this.Y + this.Z * this.Z;

        public static Vector3d Lerp (Vector3d from, Vector3d to, double t) {
            return new Vector3d(
                                x: from.X + (to.X - from.X) * t,
                                y: from.Y + (to.Y - from.Y) * t,
                                z: from.Z + (to.Z - from.Z) * t
                               );
        }

        public static Vector3d LerpClamped (Vector3d from, Vector3d to, double t) {
            return Lerp(from, to, Math.Clamp(t, 0, 1));
        }

        public AxesLengthsOrdered AxesLengthsOrdered () {
            return new AxesLengthsOrdered(this.ToVector3());
        }



        public static Vector3d MoveTowards (Vector3d current, Vector3d target, double maxDistanceDelta) {
            Vector3d direction = target - current;
            double magnitude = direction.MagnitudeDouble();

            if (magnitude <= maxDistanceDelta || magnitude == 0.0d) {
                return target;
            }

            return current + direction / magnitude * maxDistanceDelta;
        }

        internal Vector3d Absolute () {
            return new Vector3d(Math.Abs(this.X), Math.Abs(this.Y), Math.Abs(this.Z));
        }


        public static Vector3d SmoothDamp (
            Vector3d current,
            Vector3d target,
            ref Vector3d currentVelocity,
            double smoothTime,
            double maxSpeed,
            double deltaTime
        ) {
            smoothTime = Math.Max(0.0001d, smoothTime);
            double num1 = 2d / smoothTime;
            double num2 = num1 * deltaTime;

            double num3 = 1.0d
                        / (1.0d + num2 + 0.479999989271164d * num2 * num2 + 0.234999999403954d * num2 * num2 * num2);
            Vector3d vector = current - target;
            Vector3d vector3_1 = target;
            double maxLength = maxSpeed * smoothTime;
            Vector3d vector3_2 = ClampMagnitude(vector: vector, maxLength: maxLength);
            target = current - vector3_2;
            Vector3d vector3_3 = (currentVelocity + num1 * vector3_2) * deltaTime;
            currentVelocity = (currentVelocity - num1 * vector3_3) * num3;
            Vector3d vector3_4 = target + (vector3_2 + vector3_3) * num3;

            if (Dot(lhs: vector3_1 - current, rhs: vector3_4 - vector3_1) > 0.0) {
                vector3_4 = vector3_1;
                currentVelocity = (vector3_4 - vector3_1) / deltaTime;
            }

            return vector3_4;
        }

        public void Set (double new_x, double new_y, double new_z) {
            this.X = new_x;
            this.Y = new_y;
            this.Z = new_z;
        }

        public static Vector3d Scale (Vector3d a, Vector3d b) {
            return new Vector3d(x: a.X * b.X, y: a.Y * b.Y, z: a.Z * b.Z);
        }

        public static Vector3d Average (IEnumerable<Vector3d> values) {
            Vector3d sum = Vector3d.Zero;
            int count = 0;
            foreach (Vector3d value in values) {
                sum += value;
                count++;
            }

            return sum / count;
        }
        public void Scale (Vector3d scale) {
            this.X *= scale.X;
            this.Y *= scale.Y;
            this.Z *= scale.Z;
        }

        public static Vector3d Cross (Vector3d lhs, Vector3d rhs) {
            return new Vector3d(
                                x: lhs.Y * rhs.Z - lhs.Z * rhs.Y,
                                y: lhs.Z * rhs.X - lhs.X * rhs.Z,
                                z: lhs.X * rhs.Y - lhs.Y * rhs.X
                               );
        }

        public static Vector3d Reflect (Vector3d inDirection, Vector3d inNormal) {
            return -2d * Dot(lhs: inNormal, rhs: inDirection) * inNormal + inDirection;
        }

        public static Vector3d Normalize (Vector3d value) {
            double num = Length(value);

            if (num > 9.99999974737875E-06) {
                return value / num;
            }

            return Zero;
        }

        public Vector3d Normalized () {
            double num = Length(this);

            if (num > 9.99999974737875E-06) {
                return this / num;
            } else {
                return Zero;
            }
        }

        public static double Dot (Vector3d lhs, Vector3d rhs) {
            return lhs.X * rhs.X + lhs.Y * rhs.Y + lhs.Z * rhs.Z;
        }

        public static Vector3d Project (Vector3d vector, Vector3d onNormal) {
            double num = Dot(lhs: onNormal, rhs: onNormal);

            if (num < 1.40129846432482E-45d) {
                return Zero;
            }

            return onNormal * Dot(lhs: vector, rhs: onNormal) / num;
        }

        public static Vector3d Exclude (Vector3d excludeThis, Vector3d fromThat) {
            return fromThat - Project(vector: fromThat, onNormal: excludeThis);
        }

        public static double Angle (Vector3d from, Vector3d to) {
            return Math.Acos(
                              d: Math.Clamp(
                                             value: Dot(lhs: from.normalized, rhs: to.normalized),
                                             min: -1d,
                                             max: 1d
                                            )
                             )
                 * 57.29578d;
        }

        public static double Distance (Vector3d a, Vector3d b) {
            Vector3d vector3d = new Vector3d(x: a.X - b.X, y: a.Y - b.Y, z: a.Z - b.Z);

            return Math.Sqrt(vector3d.X * vector3d.X + vector3d.Y * vector3d.Y + vector3d.Z * vector3d.Z);
        }

        public static Vector3d ClampMagnitude (Vector3d vector, double maxLength) {
            if (vector.sqrMagnitude > maxLength * maxLength) {
                return vector.normalized * maxLength;
            }

            return vector;
        }

        public double Length () {
            return this.magnitude;
        }

        public static double Length (Vector3d a) {
            return Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z);
        }

        public static double SqrMagnitude (Vector3d a) {
            return a.X * a.X + a.Y * a.Y + a.Z * a.Z;
        }

        public static Vector3d Min (Vector3d lhs, Vector3d rhs) {
            return new Vector3d(
                                x: Math.Min(lhs.X, rhs.X),
                                y: Math.Min(lhs.Y, rhs.Y),
                                z: Math.Min(lhs.Z, rhs.Z)
                               );
        }

        public static Vector3d Max (Vector3d lhs, Vector3d rhs) {
            return new Vector3d(
                                x: Math.Max(lhs.X, rhs.X),
                                y: Math.Max(lhs.Y, rhs.Y),
                                z: Math.Max(lhs.Z, rhs.Z)
                               );
        }

        public static implicit operator Vector3d (Direction direction) {
            switch (direction) {
                case Direction.Up:
                    return up;
                case Direction.Down:
                    return down;
                case Direction.Forward:
                    return forward;
                case Direction.Back:
                    return back;
                case Direction.Right:
                    return right;
                case Direction.Left:
                    return left;
                default:
                    throw new ArgumentException(direction.ToString());
            }
        }
        public static Vector3d operator * (Vector3d a, Vector3d b) {
            a.Scale(scale: b);

            return a;
        }


        public static Vector3d operator + (Vector3d a, Vector3d b) {
            return new Vector3d(x: a.X + b.X, y: a.Y + b.Y, z: a.Z + b.Z);
        }

        public static Vector3d operator - (Vector3d a, Vector3d b) {
            return new Vector3d(x: a.X - b.X, y: a.Y - b.Y, z: a.Z - b.Z);
        }

        public static Vector3d operator - (Vector3d a) {
            return new Vector3d(x: -a.X, y: -a.Y, z: -a.Z);
        }

        public static Vector3d operator * (Vector3d a, double d) {
            return new Vector3d(x: a.X * d, y: a.Y * d, z: a.Z * d);
        }

        public static Vector3d operator * (double d, Vector3d a) {
            return new Vector3d(x: a.X * d, y: a.Y * d, z: a.Z * d);
        }

        public static Vector3d operator / (Vector3d a, double d) {
            return new Vector3d(x: a.X / d, y: a.Y / d, z: a.Z / d);
        }

        public static bool operator == (Vector3d lhs, Vector3d rhs) {
            return SqrMagnitude(lhs - rhs) <= 0.0 / 1.0; // Used to be < instead of <=
        }

        public static bool operator != (Vector3d lhs, Vector3d rhs) {
            return SqrMagnitude(lhs - rhs) >= 0.0 / 1.0;
        }

        public static explicit operator Vector3 (Vector3d v3d) {
            return v3d.ToVector3();
        }

        public static implicit operator Vector3d (Vector3 v3) {
            return new Vector3d(x: v3.X, y: v3.Y, z: v3.Z);
        }

        public static implicit operator Vector3d (Vector2 v2) {
            return new Vector3d(x: v2.X, y: v2.Y, z: 0);
        }

        public Vector3d (double x, double y, double z) {
            if (double.IsNaN(x) || double.IsNaN(y) || double.IsNaN(z)) {
                throw new ArgumentException(message: "Cannot handle parameters " + x + ", " + y + ", " + z + ".");
            }

            this.X = x;
            this.Y = y;
            this.Z = z;

        }

        public Vector3d (float x, float y, float z)
            : this(x: x, y: y, z: (double) z) { }

        public Vector3d (Vector3 v3)
            : this(x: v3.X, y: v3.Y, z: v3.Z) { }

        [Obsolete(message: "This is really down to convention, and convention is bad.")]
        public Vector3d (double x, double z)
            : this(x: x, y: 0, z: z) { }

        public Vector3d (double valueForEachComponent)
            : this(x: valueForEachComponent, y: valueForEachComponent, z: valueForEachComponent) { }

        public override int GetHashCode () {
            return this.X.GetHashCode() ^ this.Y.GetHashCode() << 2 ^ this.Z.GetHashCode() >> 2;
        }

        public override bool Equals (object other) {
            if (other is Vector3d == false && other is Vector3 == false) {
                return false;
            }

            Vector3d vector3d;
            try {
                vector3d = (Vector3d) other;
            } catch (InvalidCastException ice) {
                throw new InvalidCastException(other + " is a " + other.GetType(), ice);
            }

            if (this.X.Equals(obj: vector3d.X) && this.Y.Equals(obj: vector3d.Y) && this.Z.Equals(obj: vector3d.Z)) {
                return true;
            }

            return false;
        }

        public string ToString (int decimals) {
            return "("
                 + Rounding.Round(value: this.X, decimals: decimals)
                 + ", "
                 + Rounding.Round(value: this.Y, decimals: decimals)
                 + ", "
                 + Rounding.Round(value: this.Z, decimals: decimals)
                 + ")";
        }

        public string ToStringRounded () {
            return "("
                 + Rounding.Round(value: this.X, decimals: 3)
                 + ", "
                 + Rounding.Round(value: this.Y, decimals: 3)
                 + ", "
                 + Rounding.Round(value: this.Z, decimals: 3)
                 + ")";
        }

        public string ToString (bool inParantheses = true) {
            return (inParantheses ? "(" : "")
                 + Rounding.RoundToFirstInterestingDigit(value: this.X, 1)
                 + ", "
                 + Rounding.RoundToFirstInterestingDigit(value: this.Y, 1)
                 + ", "
                 + Rounding.RoundToFirstInterestingDigit(value: this.Z, 1)
                 + (inParantheses ? ")" : "");
        }

        public override string ToString () {
            return this.ToString(inParantheses: true);
        }

        public static bool ApproximatelyEquals (Vector3d lhs, Vector3d rhs) {
            return Mathematik.IsEqualApprox(lhs.X, rhs.X)
            && Mathematik.IsEqualApprox(lhs.Y, rhs.Y)
                && Mathematik.IsEqualApprox(lhs.Z, rhs.Z);
        }

        public static bool ApproximatelyEquals (Vector3d lhs, Vector3d rhs, double threshold) {
            return Math.Abs(lhs.X - rhs.X) < threshold
                && Math.Abs(lhs.Y - rhs.Y) < threshold
                && Math.Abs(lhs.Z - rhs.Z) < threshold;
        }

        public bool ContainsZero () {
            return this.X == 0 || this.Y == 0 || this.Z == 0;
        }

        public bool ContainsNonPositive () {
            return this.X <= 0 || this.Y <= 0 || this.Z <= 0;
        }

        public bool ContainsNegative () {
            return this.X < 0 || this.Y < 0 || this.Z < 0;
        }

        public bool ContainsNaN () {
            return double.IsNaN(this.X) || double.IsNaN(this.Y) || double.IsNaN(this.Z);
        }

        public Vector3 ToVector3 () {
            return new Vector3(x: (float) this.X, y: (float) this.Y, z: (float) this.Z);
        }


        public PolarCoordinates Polar () {
            return PolarCoordinates.FromCartesian(cartesian: this);
        }


        public double CartesianDistanceDouble (Coordinates other) {
            return Distance(this, other.ToVector3d());
        }


        public static QuaternionD EulerAnglesToQuaternion (Vector3d euler) {
            return EulerAnglesToQuaternion(euler.Y, euler.X, euler.Z);
        }
        public static QuaternionD EulerAnglesToQuaternion (double yaw, double pitch, double roll) {
            double cy = Math.Cos(yaw * 0.5);
            double sy = Math.Sin(yaw * 0.5);
            double cp = Math.Cos(pitch * 0.5);
            double sp = Math.Sin(pitch * 0.5);
            double cr = Math.Cos(roll * 0.5);
            double sr = Math.Sin(roll * 0.5);

            double W = cr * cp * cy + sr * sp * sy;
            double X = sr * cp * cy - cr * sp * sy;
            double Y = cr * sp * cy + sr * cp * sy;
            double Z = cr * cp * sy - sr * sp * cy;
            QuaternionD q = new QuaternionD(W, X, Y, Z);

            return q;
        }

        public Direction AsDirection () {
            if (ApproximatelyEquals(this, Direction.Right)) {
                return Direction.Right;
            } else if (ApproximatelyEquals(this, Direction.Left)) {
                return Direction.Left;
            } else
              if (ApproximatelyEquals(this, Direction.Up)) {
                return Direction.Up;
            } else
              if (ApproximatelyEquals(this, Direction.Down)) {
                return Direction.Down;
            } else
              if (ApproximatelyEquals(this, Direction.Forward)) {
                return Direction.Forward;
            } else
              if (ApproximatelyEquals(this, Direction.Back)) {
                return Direction.Back;
            }
            throw new Exception(this + " does not correspond to a known direction.");
        }

        public double MagnitudeDouble () {
            return this.magnitude;
        }

        public double PolarDistanceDouble (Coordinates other, double projectionRadius = 1.0) {
            return this.Polar().PolarDistanceDouble(other, projectionRadius);
        }

        Vector3d Coordinates.ToVector3d () {
            return this;
        }

        public static bool operator < (Vector3d lhs, Vector3d rhs) {
            return lhs.X < rhs.X && lhs.Y < rhs.Y && lhs.Z < rhs.Z;
        }
        public static bool operator > (Vector3d lhs, Vector3d rhs) {
            return lhs.X > rhs.X && lhs.Y > rhs.Y && lhs.Z > rhs.Z;
        }
        public static bool operator <= (Vector3d lhs, Vector3d rhs) {
            return lhs.X <= rhs.X && lhs.Y <= rhs.Y && lhs.Z <= rhs.Z;
        }
        public static bool operator >= (Vector3d lhs, Vector3d rhs) {
            return lhs.X >= rhs.X && lhs.Y >= rhs.Y && lhs.Z >= rhs.Z;
        }
    }

}