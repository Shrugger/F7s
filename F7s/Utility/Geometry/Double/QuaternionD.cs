﻿// Copyright (c) .NET Foundation and Contributors (https://dotnetfoundation.org/ & https://stride3d.net) and Silicon Studio Corp. (https://www.siliconstudio.co.jp)
// Distributed under the MIT license. See the LICENSE.md file in the project root for more information.
//
// -----------------------------------------------------------------------------
// Original code from SlimMath project. http://code.google.com/p/slimmath/
// Greetings to SlimDX Group. Original code published with the following license:
// -----------------------------------------------------------------------------
/*
* Copyright (c) 2007-2011 SlimDX Group
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/
using Stride.Core;
using System; using F7s.Utility.Geometry.Double;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.Math;

namespace F7s.Utility.Geometry.Double {
    /// <summary>
    /// Represents a four dimensional mathematical quaternion.
    /// </summary>
    [DataContract("quaternionD")]
    [DataStyle(DataStyle.Compact)]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct QuaternionD : IEquatable<QuaternionD>, IFormattable {
        /// <summary>
        /// The size of the <see cref="QuaternionD"/> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Unsafe.SizeOf<QuaternionD>();

        /// <summary>
        /// A <see cref="QuaternionD"/> with all of its components set to zero.
        /// </summary>
        public static readonly QuaternionD Zero = new QuaternionD();

        /// <summary>
        /// A <see cref="QuaternionD"/> with all of its components set to one.
        /// </summary>
        public static readonly QuaternionD One = new QuaternionD(1.0f, 1.0f, 1.0f, 1.0f);

        /// <summary>
        /// The identity <see cref="QuaternionD"/> (0, 0, 0, 1).
        /// </summary>
        public static readonly QuaternionD Identity = new QuaternionD(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        /// The X component of the quaternion.
        /// </summary>
        public double X;

        /// <summary>
        /// The Y component of the quaternion.
        /// </summary>
        public double Y;

        /// <summary>
        /// The Z component of the quaternion.
        /// </summary>
        public double Z;

        /// <summary>
        /// The W component of the quaternion.
        /// </summary>
        public double W;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionD"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public QuaternionD (double value) {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionD"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the components.</param>
        public QuaternionD (Vector4d value) {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = value.W;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionD"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X, Y, and Z components.</param>
        /// <param name="w">Initial value for the W component of the quaternion.</param>
        public QuaternionD (Vector3d value, double w) {
            X = value.X;
            Y = value.Y;
            Z = value.Z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionD"/> struct.
        /// </summary>
        /// <param name="value">A vector containing the values with which to initialize the X and Y components.</param>
        /// <param name="z">Initial value for the Z component of the quaternion.</param>
        /// <param name="w">Initial value for the W component of the quaternion.</param>
        public QuaternionD (Vector2d value, double z, double w) {
            X = value.X;
            Y = value.Y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionD"/> struct.
        /// </summary>
        /// <param name="x">Initial value for the X component of the quaternion.</param>
        /// <param name="y">Initial value for the Y component of the quaternion.</param>
        /// <param name="z">Initial value for the Z component of the quaternion.</param>
        /// <param name="w">Initial value for the W component of the quaternion.</param>
        public QuaternionD (double x, double y, double z, double w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QuaternionD"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the X, Y, Z, and W components of the quaternion. This must be an array with four elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than four elements.</exception>
        public QuaternionD (double[] values) {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 4)
                throw new ArgumentOutOfRangeException("values", "There must be four and only four input values for QuaternionD.");

            X = values[0];
            Y = values[1];
            Z = values[2];
            W = values[3];
        }

        /// <summary>
        /// Gets a value indicating whether this instance is equivalent to the identity quaternion.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is an identity quaternion; otherwise, <c>false</c>.
        /// </value>
        public bool IsIdentity {
            get { return Equals(Identity); }
        }

        /// <summary>
        /// Gets a value indicting whether this instance is normalized.
        /// </summary>
        public bool IsNormalized {
            get { return Abs((X * X) + (Y * Y) + (Z * Z) + (W * W) - 1f) < MathUtilD.ZeroTolerance; }
        }

        /// <summary>
        /// Gets the angle of the quaternion.
        /// </summary>
        /// <value>The quaternion's angle.</value>
        public double Angle {
            get {
                double length = (X * X) + (Y * Y) + (Z * Z);
                if (length < MathUtilD.ZeroTolerance)
                    return 0.0f;

                return 2.0f * Acos(W);
            }
        }

        /// <summary>
        /// Gets the axis components of the quaternion.
        /// </summary>
        /// <value>The axis components of the quaternion.</value>
        public Vector3d Axis {
            get {
                double length = (X * X) + (Y * Y) + (Z * Z);
                if (length < MathUtilD.ZeroTolerance)
                    return Vector3d.UnitX;

                double inv = 1.0f / length;
                return new Vector3d(X * inv, Y * inv, Z * inv);
            }
        }

        /// <summary>
        /// Gets yaw/pitch/roll equivalent of the quaternion
        /// </summary>
        public Vector3d YawPitchRoll {
            get {
                Vector3d yawPitchRoll;
                RotationYawPitchRoll(ref this, out yawPitchRoll.X, out yawPitchRoll.Y, out yawPitchRoll.Z);
                return yawPitchRoll;
            }
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the X, Y, Z, or W component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the X component, 1 for the Y component, 2 for the Z component, and 3 for the W component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 3].</exception>
        public double this[int index] {
            get {
                switch (index) {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    case 3:
                        return W;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for QuaternionD run from 0 to 3, inclusive.");
            }

            set {
                switch (index) {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    case 3:
                        W = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("index", "Indices for QuaternionD run from 0 to 3, inclusive.");
                }
            }
        }

        /// <summary>
        /// Conjugates the quaternion.
        /// </summary>
        public void Conjugate () {
            X = -X;
            Y = -Y;
            Z = -Z;
        }

        /// <summary>
        /// Conjugates and renormalizes the quaternion.
        /// </summary>
        public void Invert () {
            double lengthSq = LengthSquared();
            if (lengthSq > MathUtilD.ZeroTolerance) {
                lengthSq = 1.0f / lengthSq;

                X = -X * lengthSq;
                Y = -Y * lengthSq;
                Z = -Z * lengthSq;
                W = W * lengthSq;
            }
        }

        /// <summary>
        /// Calculates the length of the quaternion.
        /// </summary>
        /// <returns>The length of the quaternion.</returns>
        /// <remarks>
        /// <see cref="LengthSquared"/> may be preferred when only the relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public readonly double Length () {
            return Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));
        }

        /// <summary>
        /// Calculates the squared length of the quaternion.
        /// </summary>
        /// <returns>The squared length of the quaternion.</returns>
        /// <remarks>
        /// This method may be preferred to <see cref="Length"/> when only a relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        public readonly double LengthSquared () {
            return (X * X) + (Y * Y) + (Z * Z) + (W * W);
        }

        /// <summary>
        /// Converts the quaternion into a unit quaternion.
        /// </summary>
        public void Normalize () {
            double length = Length();
            if (length > MathUtilD.ZeroTolerance) {
                double inverse = 1.0f / length;
                X *= inverse;
                Y *= inverse;
                Z *= inverse;
                W *= inverse;
            }
        }

        /// <summary>
        /// Creates an array containing the elements of the quaternion.
        /// </summary>
        /// <returns>A four-element array containing the components of the quaternion.</returns>
        public double[] ToArray () {
            return new double[] { X, Y, Z, W };
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to add.</param>
        /// <param name="right">The second quaternion to add.</param>
        /// <param name="result">When the method completes, contains the sum of the two quaternions.</param>
        public static void Add (ref readonly QuaternionD left, ref readonly QuaternionD right, out QuaternionD result) {
            result.X = left.X + right.X;
            result.Y = left.Y + right.Y;
            result.Z = left.Z + right.Z;
            result.W = left.W + right.W;
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to add.</param>
        /// <param name="right">The second quaternion to add.</param>
        /// <returns>The sum of the two quaternions.</returns>
        public static QuaternionD Add (QuaternionD left, QuaternionD right) {
            QuaternionD result;
            Add(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Subtracts two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to subtract.</param>
        /// <param name="right">The second quaternion to subtract.</param>
        /// <param name="result">When the method completes, contains the difference of the two quaternions.</param>
        public static void Subtract (ref readonly QuaternionD left, ref readonly QuaternionD right, out QuaternionD result) {
            result.X = left.X - right.X;
            result.Y = left.Y - right.Y;
            result.Z = left.Z - right.Z;
            result.W = left.W - right.W;
        }

        /// <summary>
        /// Subtracts two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to subtract.</param>
        /// <param name="right">The second quaternion to subtract.</param>
        /// <returns>The difference of the two quaternions.</returns>
        public static QuaternionD Subtract (QuaternionD left, QuaternionD right) {
            QuaternionD result;
            Subtract(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Scales a quaternion by the given value.
        /// </summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scale">The amount by which to scale the quaternion.</param>
        /// <param name="result">When the method completes, contains the scaled quaternion.</param>
        public static void Multiply (ref readonly QuaternionD value, double scale, out QuaternionD result) {
            result.X = value.X * scale;
            result.Y = value.Y * scale;
            result.Z = value.Z * scale;
            result.W = value.W * scale;
        }

        /// <summary>
        /// Scales a quaternion by the given value.
        /// </summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scale">The amount by which to scale the quaternion.</param>
        /// <returns>The scaled quaternion.</returns>
        public static QuaternionD Multiply (QuaternionD value, double scale) {
            QuaternionD result;
            Multiply(ref value, scale, out result);
            return result;
        }

        /// <summary>
        /// Modulates a quaternion by another.
        /// </summary>
        /// <param name="left">The first quaternion to modulate.</param>
        /// <param name="right">The second quaternion to modulate.</param>
        /// <param name="result">When the moethod completes, contains the modulated quaternion.</param>
        public static void Multiply (ref readonly QuaternionD left, ref readonly QuaternionD right, out QuaternionD result) {
            double lx = left.X;
            double ly = left.Y;
            double lz = left.Z;
            double lw = left.W;
            double rx = right.X;
            double ry = right.Y;
            double rz = right.Z;
            double rw = right.W;

            result.X = (rx * lw) + (lx * rw) + (ry * lz) - (rz * ly);
            result.Y = (ry * lw) + (ly * rw) + (rz * lx) - (rx * lz);
            result.Z = (rz * lw) + (lz * rw) + (rx * ly) - (ry * lx);
            result.W = (rw * lw) - ((rx * lx) + (ry * ly) + (rz * lz));
        }

        /// <summary>
        /// Modulates a quaternion by another.
        /// </summary>
        /// <param name="left">The first quaternion to modulate.</param>
        /// <param name="right">The second quaternion to modulate.</param>
        /// <returns>The modulated quaternion.</returns>
        public static QuaternionD Multiply (in QuaternionD left, in QuaternionD right) {
            double lx = left.X;
            double ly = left.Y;
            double lz = left.Z;
            double lw = left.W;
            double rx = right.X;
            double ry = right.Y;
            double rz = right.Z;
            double rw = right.W;

            return new QuaternionD(
                (rx * lw) + (lx * rw) + (ry * lz) - (rz * ly),
                (ry * lw) + (ly * rw) + (rz * lx) - (rx * lz),
                (rz * lw) + (lz * rw) + (rx * ly) - (ry * lx),
                (rw * lw) - ((rx * lx) + (ry * ly) + (rz * lz)));
        }

        /// <summary>
        /// Reverses the direction of a given quaternion.
        /// </summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <param name="result">When the method completes, contains a quaternion facing in the opposite direction.</param>
        public static void Negate (ref readonly QuaternionD value, out QuaternionD result) {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            result.W = -value.W;
        }

        /// <summary>
        /// Reverses the direction of a given quaternion.
        /// </summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>A quaternion facing in the opposite direction.</returns>
        public static QuaternionD Negate (QuaternionD value) {
            QuaternionD result;
            Negate(ref value, out result);
            return result;
        }

        /// <summary>
        /// Returns a <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <param name="result">When the method completes, contains a new <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of the specified point.</param>
        public static void Barycentric (ref readonly QuaternionD value1, ref readonly QuaternionD value2, ref readonly QuaternionD value3, double amount1, double amount2, out QuaternionD result) {
            QuaternionD start, end;
            Slerp(in value1, in value2, amount1 + amount2, out start);
            Slerp(in value1, in value3, amount1 + amount2, out end);
            Slerp(ref start, ref end, amount2 / (amount1 + amount2), out result);
        }

        /// <summary>
        /// Returns a <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <returns>A new <see cref="QuaternionD"/> containing the 4D Cartesian coordinates of the specified point.</returns>
        public static QuaternionD Barycentric (QuaternionD value1, QuaternionD value2, QuaternionD value3, double amount1, double amount2) {
            QuaternionD result;
            Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out result);
            return result;
        }

        /// <summary>
        /// Conjugates a quaternion.
        /// </summary>
        /// <param name="value">The quaternion to conjugate.</param>
        /// <param name="result">When the method completes, contains the conjugated quaternion.</param>
        public static void Conjugate (ref readonly QuaternionD value, out QuaternionD result) {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
            result.W = value.W;
        }

        /// <summary>
        /// Conjugates a quaternion.
        /// </summary>
        /// <param name="value">The quaternion to conjugate.</param>
        /// <returns>The conjugated quaternion.</returns>
        public static QuaternionD Conjugate (in QuaternionD value) {
            return new QuaternionD(-value.X, -value.Y, -value.Z, value.W);
        }

        /// <summary>
        /// Calculates the dot product of two quaternions.
        /// </summary>
        /// <param name="left">First source quaternion.</param>
        /// <param name="right">Second source quaternion.</param>
        /// <param name="result">When the method completes, contains the dot product of the two quaternions.</param>
        public static void Dot (ref readonly QuaternionD left, ref readonly QuaternionD right, out double result) {
            result = (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        /// <summary>
        /// Calculates the dot product of two quaternions.
        /// </summary>
        /// <param name="left">First source quaternion.</param>
        /// <param name="right">Second source quaternion.</param>
        /// <returns>The dot product of the two quaternions.</returns>
        public static double Dot (in QuaternionD left, in QuaternionD right) {
            return (left.X * right.X) + (left.Y * right.Y) + (left.Z * right.Z) + (left.W * right.W);
        }

        /// <summary>
        /// Returns the absolute angle in radians between <paramref name="a"/> and <paramref name="b"/>
        /// </summary>
        public static double AngleBetween (in QuaternionD a, in QuaternionD b) {
            return Acos(Min(Abs(Dot(a, b)), 1f)) * 2f;
        }

        /// <summary>
        /// Exponentiates a quaternion.
        /// </summary>
        /// <param name="value">The quaternion to exponentiate.</param>
        /// <param name="result">When the method completes, contains the exponentiated quaternion.</param>
        public static void Exponential (ref readonly QuaternionD value, out QuaternionD result) {
            double angle = Sqrt((value.X * value.X) + (value.Y * value.Y) + (value.Z * value.Z));
            double sin = Sin(angle);

            if (Abs(sin) >= MathUtilD.ZeroTolerance) {
                double coeff = sin / angle;
                result.X = coeff * value.X;
                result.Y = coeff * value.Y;
                result.Z = coeff * value.Z;
            } else {
                result = value;
            }

            result.W = Cos(angle);
        }

        /// <summary>
        /// Exponentiates a quaternion.
        /// </summary>
        /// <param name="value">The quaternion to exponentiate.</param>
        /// <returns>The exponentiated quaternion.</returns>
        public static QuaternionD Exponential (QuaternionD value) {
            QuaternionD result;
            Exponential(ref value, out result);
            return result;
        }

        /// <summary>
        /// Conjugates and renormalizes the quaternion.
        /// </summary>
        /// <param name="value">The quaternion to conjugate and renormalize.</param>
        /// <param name="result">When the method completes, contains the conjugated and renormalized quaternion.</param>
        public static void Invert (ref readonly QuaternionD value, out QuaternionD result) {
            result = value;
            result.Invert();
        }

        /// <summary>
        /// Conjugates and renormalizes the quaternion.
        /// </summary>
        /// <param name="value">The quaternion to conjugate and renormalize.</param>
        /// <returns>The conjugated and renormalized quaternion.</returns>
        public static QuaternionD Invert (QuaternionD value) {
            QuaternionD result;
            Invert(ref value, out result);
            return result;
        }

        /// <summary>
        /// Performs a linear interpolation between two quaternions.
        /// </summary>
        /// <param name="start">Start quaternion.</param>
        /// <param name="end">End quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the linear interpolation of the two quaternions.</param>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned.
        /// </remarks>
        public static void Lerp (ref readonly QuaternionD start, ref readonly QuaternionD end, double amount, out QuaternionD result) {
            double inverse = 1.0f - amount;

            if (Dot(start, end) >= 0.0f) {
                result.X = (inverse * start.X) + (amount * end.X);
                result.Y = (inverse * start.Y) + (amount * end.Y);
                result.Z = (inverse * start.Z) + (amount * end.Z);
                result.W = (inverse * start.W) + (amount * end.W);
            } else {
                result.X = (inverse * start.X) - (amount * end.X);
                result.Y = (inverse * start.Y) - (amount * end.Y);
                result.Z = (inverse * start.Z) - (amount * end.Z);
                result.W = (inverse * start.W) - (amount * end.W);
            }

            result.Normalize();
        }

        /// <summary>
        /// Performs a linear interpolation between two quaternion.
        /// </summary>
        /// <param name="start">Start quaternion.</param>
        /// <param name="end">End quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two quaternions.</returns>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned.
        /// </remarks>
        public static QuaternionD Lerp (QuaternionD start, QuaternionD end, double amount) {
            QuaternionD result;
            Lerp(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Returns a rotation whose facing direction points towards <paramref name="forward"/>
        /// and whose up direction points as close as possible to <paramref name="up"/>.
        /// </summary>
        public static QuaternionD LookRotation (in Vector3d forward, in Vector3d up) {
            var right = Vector3d.Normalize(Vector3d.Cross(up, forward));
            var orthoUp = Vector3d.Cross(forward, right);
            var m = new MatrixD {
                M11 = right.X,
                M12 = right.Y,
                M13 = right.Z,
                M21 = orthoUp.X,
                M22 = orthoUp.Y,
                M23 = orthoUp.Z,
                M31 = forward.X,
                M32 = forward.Y,
                M33 = forward.Z,
            };
            RotationMatrixD(ref m, out var lQuaternionD);
            return lQuaternionD;
        }

        /// <summary>
        /// Calculates the natural logarithm of the specified quaternion.
        /// </summary>
        /// <param name="value">The quaternion whose logarithm will be calculated.</param>
        /// <param name="result">When the method completes, contains the natural logarithm of the quaternion.</param>
        public static void Logarithm (ref readonly QuaternionD value, out QuaternionD result) {
            if (Abs(value.W) < 1.0f) {
                double angle = Acos(value.W);
                double sin = Sin(angle);

                if (Abs(sin) >= MathUtilD.ZeroTolerance) {
                    double coeff = angle / sin;
                    result.X = value.X * coeff;
                    result.Y = value.Y * coeff;
                    result.Z = value.Z * coeff;
                } else {
                    result = value;
                }
            } else {
                result = value;
            }

            result.W = 0.0f;
        }

        /// <summary>
        /// Calculates the natural logarithm of the specified quaternion.
        /// </summary>
        /// <param name="value">The quaternion whose logarithm will be calculated.</param>
        /// <returns>The natural logarithm of the quaternion.</returns>
        public static QuaternionD Logarithm (QuaternionD value) {
            QuaternionD result;
            Logarithm(ref value, out result);
            return result;
        }

        /// <summary>
        /// Converts the quaternion into a unit quaternion.
        /// </summary>
        /// <param name="value">The quaternion to normalize.</param>
        /// <param name="result">When the method completes, contains the normalized quaternion.</param>
        public static void Normalize (ref readonly QuaternionD value, out QuaternionD result) {
            QuaternionD temp = value;
            result = temp;
            result.Normalize();
        }

        /// <summary>
        /// Converts the quaternion into a unit quaternion.
        /// </summary>
        /// <param name="value">The quaternion to normalize.</param>
        /// <returns>The normalized quaternion.</returns>
        public static QuaternionD Normalize (QuaternionD value) {
            value.Normalize();
            return value;
        }

        /// <summary>
        /// Rotates a Vector3d by the specified quaternion rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        public readonly void Rotate (ref Vector3d vector) {
            var pureQuaternionD = new QuaternionD(vector, 0);
            pureQuaternionD = Conjugate(this) * pureQuaternionD * this;

            vector.X = pureQuaternionD.X;
            vector.Y = pureQuaternionD.Y;
            vector.Z = pureQuaternionD.Z;
        }

        /// <summary>
        /// Creates a quaternion given a rotation and an axis.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        public static void RotationAxis (ref readonly Vector3d axis, double angle, out QuaternionD result) {
            Vector3d normalized;
            Vector3d.Normalize(in axis, out normalized);

            double half = angle * 0.5f;
            double sin = Sin(half);
            double cos = Cos(half);

            result.X = normalized.X * sin;
            result.Y = normalized.Y * sin;
            result.Z = normalized.Z * sin;
            result.W = cos;
        }

        /// <summary>
        /// Creates a quaternion given a rotation and an axis.
        /// </summary>
        /// <param name="axis">The axis of rotation.</param>
        /// <param name="angle">The angle of rotation.</param>
        /// <returns>The newly created quaternion.</returns>
        public static QuaternionD RotationAxis (Vector3d axis, double angle) {
            QuaternionD result;
            RotationAxis(ref axis, angle, out result);
            return result;
        }

        /// <summary>
        /// Creates a quaternion given a rotation matrix.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        public static void RotationMatrixD (ref readonly MatrixD matrix, out QuaternionD result) {
            double sqrt;
            double half;
            double scale = matrix.M11 + matrix.M22 + matrix.M33;

            if (scale > 0.0f) {
                sqrt = Sqrt(scale + 1.0f);
                result.W = sqrt * 0.5f;
                sqrt = 0.5f / sqrt;

                result.X = (matrix.M23 - matrix.M32) * sqrt;
                result.Y = (matrix.M31 - matrix.M13) * sqrt;
                result.Z = (matrix.M12 - matrix.M21) * sqrt;
            } else if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33) {
                sqrt = Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                half = 0.5f / sqrt;

                result.X = 0.5f * sqrt;
                result.Y = (matrix.M12 + matrix.M21) * half;
                result.Z = (matrix.M13 + matrix.M31) * half;
                result.W = (matrix.M23 - matrix.M32) * half;
            } else if (matrix.M22 > matrix.M33) {
                sqrt = Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                half = 0.5f / sqrt;

                result.X = (matrix.M21 + matrix.M12) * half;
                result.Y = 0.5f * sqrt;
                result.Z = (matrix.M32 + matrix.M23) * half;
                result.W = (matrix.M31 - matrix.M13) * half;
            } else {
                sqrt = Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                half = 0.5f / sqrt;

                result.X = (matrix.M31 + matrix.M13) * half;
                result.Y = (matrix.M32 + matrix.M23) * half;
                result.Z = 0.5f * sqrt;
                result.W = (matrix.M12 - matrix.M21) * half;
            }
        }

        /// <summary>
        /// Creates a quaternion given a rotation matrix.
        /// </summary>
        /// <param name="matrix">The rotation matrix.</param>
        /// <returns>The newly created quaternion.</returns>
        public static QuaternionD RotationMatrixD (MatrixD matrix) {
            QuaternionD result;
            RotationMatrixD(ref matrix, out result);
            return result;
        }

        /// <summary>
        /// Creates a quaternion that rotates around the x-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        public static void RotationX (double angle, out QuaternionD result) {
            double halfAngle = angle * 0.5f;
            result = new QuaternionD(Sin(halfAngle), 0.0f, 0.0f, Cos(halfAngle));
        }

        /// <summary>
        /// Creates a quaternion that rotates around the x-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians.</param>
        /// <returns>The created rotation quaternion.</returns>
        public static QuaternionD RotationX (double angle) {
            QuaternionD result;
            RotationX(angle, out result);
            return result;
        }

        /// <summary>
        /// Creates a quaternion that rotates around the y-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        public static void RotationY (double angle, out QuaternionD result) {
            double halfAngle = angle * 0.5f;
            result = new QuaternionD(0.0f, Sin(halfAngle), 0.0f, Cos(halfAngle));
        }

        /// <summary>
        /// Creates a quaternion that rotates around the y-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians.</param>
        /// <returns>The created rotation quaternion.</returns>
        public static QuaternionD RotationY (double angle) {
            QuaternionD result;
            RotationY(angle, out result);
            return result;
        }

        /// <summary>
        /// Creates a quaternion that rotates around the z-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        public static void RotationZ (double angle, out QuaternionD result) {
            double halfAngle = angle * 0.5f;
            result = new QuaternionD(0.0f, 0.0f, Sin(halfAngle), Cos(halfAngle));
        }

        /// <summary>
        /// Creates a quaternion that rotates around the z-axis.
        /// </summary>
        /// <param name="angle">Angle of rotation in radians.</param>
        /// <returns>The created rotation quaternion.</returns>
        public static QuaternionD RotationZ (double angle) {
            QuaternionD result;
            RotationZ(angle, out result);
            return result;
        }

        /// <summary>
        /// Calculate the yaw/pitch/roll rotation equivalent to the provided quaternion.
        /// </summary>
        /// <param name="rotation">The input quaternion</param>
        /// <param name="yaw">The yaw component in radians.</param>
        /// <param name="pitch">The pitch component in radians.</param>
        /// <param name="roll">The roll component in radians.</param>
        public static void RotationYawPitchRoll (ref readonly QuaternionD rotation, out double yaw, out double pitch, out double roll) {
            // Equivalent to:
            //  MatrixD rotationMatrixD;
            //  MatrixD.Rotation(ref cachedRotation, out rotationMatrixD);
            //  rotationMatrixD.Decompose(out double yaw, out double pitch, out double roll);

            var xx = rotation.X * rotation.X;
            var yy = rotation.Y * rotation.Y;
            var zz = rotation.Z * rotation.Z;
            var xy = rotation.X * rotation.Y;
            var zw = rotation.Z * rotation.W;
            var zx = rotation.Z * rotation.X;
            var yw = rotation.Y * rotation.W;
            var yz = rotation.Y * rotation.Z;
            var xw = rotation.X * rotation.W;

            var M11 = 1.0f - (2.0f * (yy + zz));
            var M12 = 2.0f * (xy + zw);
            //var M13 = 2.0f * (zx - yw);
            var M21 = 2.0f * (xy - zw);
            var M22 = 1.0f - (2.0f * (zz + xx));
            //var M23 = 2.0f * (yz + xw);
            var M31 = 2.0f * (zx + yw);
            var M32 = 2.0f * (yz - xw);
            var M33 = 1.0f - (2.0f * (yy + xx));

            /*** Refer to MatrixD.Decompose(out double yaw, out double pitch, out double roll) for code and license ***/
            if (MathUtilD.IsOne(Math.Abs(M32))) {
                if (M32 >= 0) {
                    // Edge case where M32 == +1
                    pitch = -MathUtilD.PiOverTwo;
                    yaw = Atan2(-M21, M11);
                    roll = 0;
                } else {
                    // Edge case where M32 == -1
                    pitch = MathUtilD.PiOverTwo;
                    yaw = -Atan2(-M21, M11);
                    roll = 0;
                }
            } else {
                // Common case
                pitch = Asin(-M32);
                yaw = Atan2(M31, M33);
                roll = Atan2(M12, M22);
            }
        }

        /// <summary>
        /// Creates a quaternion given a yaw, pitch, and roll value (angles in radians).
        /// </summary>
        /// <param name="yaw">The yaw of rotation in radians.</param>
        /// <param name="pitch">The pitch of rotation in radians.</param>
        /// <param name="roll">The roll of rotation in radians.</param>
        /// <param name="result">When the method completes, contains the newly created quaternion.</param>
        public static void RotationYawPitchRoll (double yaw, double pitch, double roll, out QuaternionD result) {
            var halfRoll = roll * 0.5f;
            var halfPitch = pitch * 0.5f;
            var halfYaw = yaw * 0.5f;

            var sinRoll = Sin(halfRoll);
            var cosRoll = Cos(halfRoll);
            var sinPitch = Sin(halfPitch);
            var cosPitch = Cos(halfPitch);
            var sinYaw = Sin(halfYaw);
            var cosYaw = Cos(halfYaw);

            var cosYawPitch = cosYaw * cosPitch;
            var sinYawPitch = sinYaw * sinPitch;

            result.X = (cosYaw * sinPitch * cosRoll) + (sinYaw * cosPitch * sinRoll);
            result.Y = (sinYaw * cosPitch * cosRoll) - (cosYaw * sinPitch * sinRoll);
            result.Z = (cosYawPitch * sinRoll) - (sinYawPitch * cosRoll);
            result.W = (cosYawPitch * cosRoll) + (sinYawPitch * sinRoll);
        }

        /// <summary>
        /// Creates a quaternion given a yaw, pitch, and roll value (angles in radians).
        /// </summary>
        /// <param name="yaw">The yaw of rotation in radians.</param>
        /// <param name="pitch">The pitch of rotation in radians.</param>
        /// <param name="roll">The roll of rotation in radians.</param>
        /// <returns>The newly created quaternion.</returns>
        public static QuaternionD RotationYawPitchRoll (double yaw, double pitch, double roll) {
            QuaternionD result;
            RotationYawPitchRoll(yaw, pitch, roll, out result);
            return result;
        }

        /// <summary>
        /// Computes a quaternion corresponding to the rotation transforming the vector <paramref name="source"/> to the vector <paramref name="target"/>.
        /// </summary>
        /// <param name="source">The source vector of the transformation.</param>
        /// <param name="target">The target vector of the transformation.</param>
        /// <returns>The resulting quaternion corresponding to the transformation of the source vector to the target vector.</returns>
        public static QuaternionD BetweenDirections (Vector3d source, Vector3d target) {
            QuaternionD result;
            BetweenDirections(ref source, ref target, out result);
            return result;
        }

        /// <summary>
        /// Computes a quaternion corresponding to the rotation transforming the vector <paramref name="source"/> to the vector <paramref name="target"/>.
        /// </summary>
        /// <param name="source">The source vector of the transformation.</param>
        /// <param name="target">The target vector of the transformation.</param>
        /// <param name="result">The resulting quaternion corresponding to the transformation of the source vector to the target vector.</param>
        public static void BetweenDirections (ref readonly Vector3d source, ref readonly Vector3d target, out QuaternionD result) {
            var norms = Math.Sqrt(source.LengthSquared() * target.LengthSquared());
            var real = norms + Vector3d.Dot(source, target);
            if (real < MathUtilD.ZeroTolerance * norms) {
                // If source and target are exactly opposite, rotate 180 degrees around an arbitrary orthogonal axis.
                // Axis normalisation can happen later, when we normalise the quaternion.
                result = Math.Abs(source.X) > Math.Abs(source.Z)
                    ? new QuaternionD(-source.Y, source.X, 0.0f, 0.0f)
                    : new QuaternionD(0.0f, -source.Z, source.Y, 0.0f);
            } else {
                // Otherwise, build quaternion the standard way.
                var axis = Vector3d.Cross(source, target);
                result = new QuaternionD(axis, real);
            }
            result.Normalize();
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation.
        /// </summary>
        /// <param name="start">Start quaternion.</param>
        /// <param name="end">End quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the spherical linear interpolation of the two quaternions.</param>
        public static void Slerp (ref readonly QuaternionD start, ref readonly QuaternionD end, double amount, out QuaternionD result) {
            double opposite;
            double inverse;
            double dot = Dot(start, end);

            if (Abs(dot) > 1.0f - MathUtilD.ZeroTolerance) {
                inverse = 1.0f - amount;
                opposite = amount * Sign(dot);
            } else {
                double acos = Acos(Abs(dot));
                double invSin = 1.0f / Sin(acos);

                inverse = Sin((1.0f - amount) * acos) * invSin;
                opposite = Sin(amount * acos) * invSin * Sign(dot);
            }

            result.X = (inverse * start.X) + (opposite * end.X);
            result.Y = (inverse * start.Y) + (opposite * end.Y);
            result.Z = (inverse * start.Z) + (opposite * end.Z);
            result.W = (inverse * start.W) + (opposite * end.W);
        }

        /// <summary>
        /// Interpolates between two quaternions, using spherical linear interpolation.
        /// </summary>
        /// <param name="start">Start quaternion.</param>
        /// <param name="end">End quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The spherical linear interpolation of the two quaternions.</returns>
        public static QuaternionD Slerp (in QuaternionD start, in QuaternionD end, double amount) {
            double opposite;
            double inverse;
            double dot = Dot(start, end);

            if (Abs(dot) > 1.0f - MathUtilD.ZeroTolerance) {
                inverse = 1.0f - amount;
                opposite = amount * Sign(dot);
            } else {
                double acos = Acos(Abs(dot));
                double invSin = 1.0f / Sin(acos);

                inverse = Sin((1.0f - amount) * acos) * invSin;
                opposite = Sin(amount * acos) * invSin * Sign(dot);
            }

            QuaternionD result;
            result.X = (inverse * start.X) + (opposite * end.X);
            result.Y = (inverse * start.Y) + (opposite * end.Y);
            result.Z = (inverse * start.Z) + (opposite * end.Z);
            result.W = (inverse * start.W) + (opposite * end.W);
            return result;
        }

        /// <summary>
        /// Rotate <paramref name="current"/> towards <paramref name="target"/> by <paramref name="angle"/>.
        /// </summary>
        /// <remarks>
        /// When the angle difference between <paramref name="current"/> and <paramref name="target"/> is less than
        /// the given <paramref name="angle"/>, returns <paramref name="target"/> instead of overshooting past it.
        /// </remarks>
        public static QuaternionD RotateTowards (in QuaternionD current, in QuaternionD target, double angle) {
            var maxAngle = AngleBetween(current, target);
            return maxAngle == 0f ? target : Slerp(current, target, Min(1f, angle / maxAngle));
        }

        /// <summary>
        /// Interpolates between quaternions, using spherical quadrangle interpolation.
        /// </summary>
        /// <param name="value1">First source quaternion.</param>
        /// <param name="value2">Second source quaternion.</param>
        /// <param name="value3">Thrid source quaternion.</param>
        /// <param name="value4">Fourth source quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of interpolation.</param>
        /// <param name="result">When the method completes, contains the spherical quadrangle interpolation of the quaternions.</param>
        public static void Squad (ref readonly QuaternionD value1, ref readonly QuaternionD value2, ref readonly QuaternionD value3, ref readonly QuaternionD value4, double amount, out QuaternionD result) {
            QuaternionD start, end;
            Slerp(in value1, in value4, amount, out start);
            Slerp(in value2, in value3, amount, out end);
            Slerp(ref start, ref end, 2.0f * amount * (1.0f - amount), out result);
        }

        /// <summary>
        /// Interpolates between quaternions, using spherical quadrangle interpolation.
        /// </summary>
        /// <param name="value1">First source quaternion.</param>
        /// <param name="value2">Second source quaternion.</param>
        /// <param name="value3">Thrid source quaternion.</param>
        /// <param name="value4">Fourth source quaternion.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of interpolation.</param>
        /// <returns>The spherical quadrangle interpolation of the quaternions.</returns>
        public static QuaternionD Squad (QuaternionD value1, QuaternionD value2, QuaternionD value3, QuaternionD value4, double amount) {
            QuaternionD result;
            Squad(ref value1, ref value2, ref value3, ref value4, amount, out result);
            return result;
        }

        /// <summary>
        /// Sets up control points for spherical quadrangle interpolation.
        /// </summary>
        /// <param name="value1">First source quaternion.</param>
        /// <param name="value2">Second source quaternion.</param>
        /// <param name="value3">Third source quaternion.</param>
        /// <param name="value4">Fourth source quaternion.</param>
        /// <returns>An array of three quaternions that represent control points for spherical quadrangle interpolation.</returns>
        public static QuaternionD[] SquadSetup (QuaternionD value1, QuaternionD value2, QuaternionD value3, QuaternionD value4) {
            QuaternionD q0 = (value1 + value2).LengthSquared() < (value1 - value2).LengthSquared() ? -value1 : value1;
            QuaternionD q2 = (value2 + value3).LengthSquared() < (value2 - value3).LengthSquared() ? -value3 : value3;
            QuaternionD q3 = (value3 + value4).LengthSquared() < (value3 - value4).LengthSquared() ? -value4 : value4;
            QuaternionD q1 = value2;

            QuaternionD q1Exp, q2Exp;
            Exponential(ref q1, out q1Exp);
            Exponential(ref q2, out q2Exp);

            QuaternionD[] results = new QuaternionD[3];
            results[0] = q1 * Exponential(-0.25f * (Logarithm(q1Exp * q2) + Logarithm(q1Exp * q0)));
            results[1] = q2 * Exponential(-0.25f * (Logarithm(q2Exp * q3) + Logarithm(q2Exp * q1)));
            results[2] = q2;

            return results;
        }

        /// <summary>
        /// Adds two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to add.</param>
        /// <param name="right">The second quaternion to add.</param>
        /// <returns>The sum of the two quaternions.</returns>
        public static QuaternionD operator + (in QuaternionD left, in QuaternionD right) {
            QuaternionD result;
            Add(in left, in right, out result);
            return result;
        }

        /// <summary>
        /// Subtracts two quaternions.
        /// </summary>
        /// <param name="left">The first quaternion to subtract.</param>
        /// <param name="right">The second quaternion to subtract.</param>
        /// <returns>The difference of the two quaternions.</returns>
        public static QuaternionD operator - (in QuaternionD left, in QuaternionD right) {
            QuaternionD result;
            Subtract(in left, in right, out result);
            return result;
        }

        /// <summary>
        /// Reverses the direction of a given quaternion.
        /// </summary>
        /// <param name="value">The quaternion to negate.</param>
        /// <returns>A quaternion facing in the opposite direction.</returns>
        public static QuaternionD operator - (in QuaternionD value) {
            QuaternionD result;
            Negate(in value, out result);
            return result;
        }

        /// <summary>
        /// Scales a quaternion by the given value.
        /// </summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scale">The amount by which to scale the quaternion.</param>
        /// <returns>The scaled quaternion.</returns>
        public static QuaternionD operator * (double scale, in QuaternionD value) {
            QuaternionD result;
            Multiply(in value, scale, out result);
            return result;
        }

        /// <summary>
        /// Scales a quaternion by the given value.
        /// </summary>
        /// <param name="value">The quaternion to scale.</param>
        /// <param name="scale">The amount by which to scale the quaternion.</param>
        /// <returns>The scaled quaternion.</returns>
        public static QuaternionD operator * (in QuaternionD value, double scale) {
            QuaternionD result;
            Multiply(in value, scale, out result);
            return result;
        }

        /// <summary>
        /// Multiplies a quaternion by another.
        /// </summary>
        /// <param name="left">The first quaternion to multiply.</param>
        /// <param name="right">The second quaternion to multiply.</param>
        /// <returns>The multiplied quaternion.</returns>
        public static QuaternionD operator * (in QuaternionD left, in QuaternionD right) {
            QuaternionD result;
            Multiply(in left, in right, out result);
            return result;
        }

        /// <summary>
        /// Return the vector rotated by the quaternion.
        /// </summary>
        /// <remarks>
        /// Shorthand for <see cref="Rotate"/>
        /// </remarks>
        public static Vector3d operator * (in QuaternionD left, in Vector3d right) {
            var pureQuaternionD = new QuaternionD(right, 0);
            pureQuaternionD = Conjugate(left) * pureQuaternionD * left;
            return new Vector3d(pureQuaternionD.X, pureQuaternionD.Y, pureQuaternionD.Z);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator == (in QuaternionD left, in QuaternionD right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator != (in QuaternionD left, in QuaternionD right) {
            return !left.Equals(right);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override readonly string ToString () {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", X, Y, Z, W);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public readonly string ToString (string format) {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1} Z:{2} W:{3}", X.ToString(format, CultureInfo.CurrentCulture),
                Y.ToString(format, CultureInfo.CurrentCulture), Z.ToString(format, CultureInfo.CurrentCulture), W.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public readonly string ToString (IFormatProvider formatProvider) {
            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", X, Y, Z, W);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public readonly string ToString (string format, IFormatProvider formatProvider) {
            if (format == null)
                return ToString(formatProvider);

            return string.Format(formatProvider, "X:{0} Y:{1} Z:{2} W:{3}", X.ToString(format, formatProvider),
                Y.ToString(format, formatProvider), Z.ToString(format, formatProvider), W.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override readonly int GetHashCode () {
            return X.GetHashCode() + Y.GetHashCode() + Z.GetHashCode() + W.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="QuaternionD"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="QuaternionD"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="QuaternionD"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public readonly bool Equals (QuaternionD other) {
            return Abs(other.X - X) < MathUtilD.ZeroTolerance &&
                Abs(other.Y - Y) < MathUtilD.ZeroTolerance &&
                Abs(other.Z - Z) < MathUtilD.ZeroTolerance &&
                Abs(other.W - W) < MathUtilD.ZeroTolerance;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override readonly bool Equals (object value) {
            return value is QuaternionD q && Equals(q);
        }

#if SlimDX1xInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="Stride.Core.Mathematics.QuaternionD"/> to <see cref="SlimDX.QuaternionD"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator SlimDX.QuaternionD(QuaternionD value)
        {
            return new SlimDX.QuaternionD(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SlimDX.QuaternionD"/> to <see cref="Stride.Core.Mathematics.QuaternionD"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator QuaternionD(SlimDX.QuaternionD value)
        {
            return new QuaternionD(value.X, value.Y, value.Z, value.W);
        }
#endif

#if WPFInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="Stride.Core.Mathematics.QuaternionD"/> to <see cref="System.Windows.Media.Media3D.QuaternionD"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator System.Windows.Media.Media3D.QuaternionD(QuaternionD value)
        {
            return new System.Windows.Media.Media3D.QuaternionD(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Windows.Media.Media3D.QuaternionD"/> to <see cref="Stride.Core.Mathematics.QuaternionD"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator QuaternionD(System.Windows.Media.Media3D.QuaternionD value)
        {
            return new QuaternionD((double)value.X, (double)value.Y, (double)value.Z, (double)value.W);
        }
#endif

#if XnaInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="Stride.Core.Mathematics.QuaternionD"/> to <see cref="Microsoft.Xna.Framework.QuaternionD"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Microsoft.Xna.Framework.QuaternionD(QuaternionD value)
        {
            return new Microsoft.Xna.Framework.QuaternionD(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Microsoft.Xna.Framework.QuaternionD"/> to <see cref="Stride.Core.Mathematics.QuaternionD"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator QuaternionD(Microsoft.Xna.Framework.QuaternionD value)
        {
            return new QuaternionD(value.X, value.Y, value.Z, value.W);
        }
#endif
    }
}