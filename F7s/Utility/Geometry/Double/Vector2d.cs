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
using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double;
using System; using F7s.Utility.Geometry.Double;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace F7s.Utility.Geometry.Double {
    /// <summary>
    /// Represents a two dimensional mathematical vector.
    /// </summary>
    [DataContract("double2")]
    [DataStyle(DataStyle.Compact)]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Vector2d : IEquatable<Vector2d>, IFormattable {
        /// <summary>
        /// The size of the <see cref="Vector2d"/> type, in bytes.
        /// </summary>
        public static readonly int SizeInBytes = Unsafe.SizeOf<Vector2d>();

        /// <summary>
        /// A <see cref="Vector2d"/> with all of its components set to zero.
        /// </summary>
        public static readonly Vector2d Zero = new Vector2d();

        /// <summary>
        /// The X unit <see cref="Vector2d"/> (1, 0).
        /// </summary>
        public static readonly Vector2d UnitX = new Vector2d(1.0f, 0.0f);

        /// <summary>
        /// The Y unit <see cref="Vector2d"/> (0, 1).
        /// </summary>
        public static readonly Vector2d UnitY = new Vector2d(0.0f, 1.0f);

        /// <summary>
        /// A <see cref="Vector2d"/> with all of its components set to one.
        /// </summary>
        public static readonly Vector2d One = new Vector2d(1.0f, 1.0f);

        /// <summary>
        /// The X component of the vector.
        /// </summary>
        [DataMember(0)]
        public double X;

        /// <summary>
        /// The Y component of the vector.
        /// </summary>
        [DataMember(1)]
        public double Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2d"/> struct.
        /// </summary>
        /// <param name="value">The value that will be assigned to all components.</param>
        public Vector2d (double value) {
            X = value;
            Y = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2d"/> struct.
        /// </summary>
        /// <param name="x">Initial value for the X component of the vector.</param>
        /// <param name="y">Initial value for the Y component of the vector.</param>
        public Vector2d (double x, double y) {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector2d"/> struct.
        /// </summary>
        /// <param name="values">The values to assign to the X and Y components of the vector. This must be an array with two elements.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="values"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="values"/> contains more or less than two elements.</exception>
        public Vector2d (double[] values) {
            if (values == null)
                throw new ArgumentNullException("values");
            if (values.Length != 2)
                throw new ArgumentOutOfRangeException("values", "There must be two and only two input values for Vector2d.");

            X = values[0];
            Y = values[1];
        }

        /// <summary>
        /// Gets a value indicting whether this instance is normalized.
        /// </summary>
        public bool IsNormalized {
            get { return Math.Abs((X * X) + (Y * Y) - 1f) < MathUtil.ZeroTolerance; }
        }

        /// <summary>
        /// Gets or sets the component at the specified index.
        /// </summary>
        /// <value>The value of the X or Y component, depending on the index.</value>
        /// <param name="index">The index of the component to access. Use 0 for the X component and 1 for the Y component.</param>
        /// <returns>The value of the component at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="index"/> is out of the range [0, 1].</exception>
        public double this[int index] {
            get {
                switch (index) {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                }

                throw new ArgumentOutOfRangeException("index", "Indices for Vector2d run from 0 to 1, inclusive.");
            }

            set {
                switch (index) {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("index", "Indices for Vector2d run from 0 to 1, inclusive.");
                }
            }
        }

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        /// <returns>The length of the vector.</returns>
        /// <remarks>
        /// <see cref="LengthSquared"/> may be preferred when only the relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly double Length () {
            return Math.Sqrt((X * X) + (Y * Y));
        }

        /// <summary>
        /// Calculates the squared length of the vector.
        /// </summary>
        /// <returns>The squared length of the vector.</returns>
        /// <remarks>
        /// This method may be preferred to <see cref="Length"/> when only a relative length is needed
        /// and speed is of the essence.
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double LengthSquared () {
            return (X * X) + (Y * Y);
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Normalize () {
            double length = Length();
            if (length > MathUtil.ZeroTolerance) {
                double inv = 1.0f / length;
                X *= inv;
                Y *= inv;
            }
        }

        /// <summary>
        /// Creates an array containing the elements of the vector.
        /// </summary>
        /// <returns>A two-element array containing the components of the vector.</returns>
        public double[] ToArray () {
            return new double[] { X, Y };
        }

        /// <summary>
        /// Moves the first vector2 to the second one in a straight line.
        /// </summary>
        /// <param name="from">The first point.</param>
        /// <param name="to">The second point.</param>
        /// <param name="maxTravelDistance">The rate at which the first point is going to move towards the second point.</param>
        public static Vector2d MoveTo (in Vector2d from, in Vector2d to, double maxTravelDistance) {
            Vector2d distance = Subtract(to, from);

            double length = distance.Length();

            if (maxTravelDistance >= length || length == 0)
                return to;
            else
                return new Vector2d(from.X + (distance.X / length * maxTravelDistance), from.Y + (distance.Y / length * maxTravelDistance));
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <param name="result">When the method completes, contains the sum of the two vectors.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add (ref readonly Vector2d left, ref readonly Vector2d right, out Vector2d result) {
            result = new Vector2d(left.X + right.X, left.Y + right.Y);
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Add (Vector2d left, Vector2d right) {
            return new Vector2d(left.X + right.X, left.Y + right.Y);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <param name="result">When the method completes, contains the difference of the two vectors.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Subtract (ref readonly Vector2d left, ref readonly Vector2d right, out Vector2d result) {
            result = new Vector2d(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Subtract (in Vector2d left, in Vector2d right) {
            return new Vector2d(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Multiply (ref readonly Vector2d value, double scale, out Vector2d result) {
            result = new Vector2d(value.X * scale, value.Y * scale);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Multiply (Vector2d value, double scale) {
            return new Vector2d(value.X * scale, value.Y * scale);
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <param name="result">When the method completes, contains the modulated vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Modulate (ref readonly Vector2d left, ref readonly Vector2d right, out Vector2d result) {
            result = new Vector2d(left.X * right.X, left.Y * right.Y);
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to modulate.</param>
        /// <param name="right">The second vector to modulate.</param>
        /// <returns>The modulated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Modulate (Vector2d left, Vector2d right) {
            return new Vector2d(left.X * right.X, left.Y * right.Y);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <param name="result">When the method completes, contains the scaled vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Divide (ref readonly Vector2d value, double scale, out Vector2d result) {
            result = new Vector2d(value.X / scale, value.Y / scale);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Divide (Vector2d value, double scale) {
            return new Vector2d(value.X / scale, value.Y / scale);
        }

        /// <summary>
        /// Demodulates a vector with another by performing component-wise division.
        /// </summary>
        /// <param name="left">The first vector to demodulate.</param>
        /// <param name="right">The second vector to demodulate.</param>
        /// <param name="result">When the method completes, contains the demodulated vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Demodulate (ref readonly Vector2d left, ref readonly Vector2d right, out Vector2d result) {
            result = new Vector2d(left.X / right.X, left.Y / right.Y);
        }

        /// <summary>
        /// Demodulates a vector with another by performing component-wise division.
        /// </summary>
        /// <param name="left">The first vector to demodulate.</param>
        /// <param name="right">The second vector to demodulate.</param>
        /// <returns>The demodulated vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Demodulate (Vector2d left, Vector2d right) {
            return new Vector2d(left.X / right.X, left.Y / right.Y);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <param name="result">When the method completes, contains a vector facing in the opposite direction.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Negate (ref readonly Vector2d value, out Vector2d result) {
            result = new Vector2d(-value.X, -value.Y);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Negate (Vector2d value) {
            return new Vector2d(-value.X, -value.Y);
        }

        /// <summary>
        /// Returns a <see cref="Vector2d"/> containing the 2D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="Vector2d"/> containing the 2D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="Vector2d"/> containing the 2D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="Vector2d"/> containing the 2D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <param name="result">When the method completes, contains the 2D Cartesian coordinates of the specified point.</param>
        public static void Barycentric (ref readonly Vector2d value1, ref readonly Vector2d value2, ref readonly Vector2d value3, double amount1, double amount2, out Vector2d result) {
            result = new Vector2d(
                value1.X + (amount1 * (value2.X - value1.X)) + (amount2 * (value3.X - value1.X)),
                value1.Y + (amount1 * (value2.Y - value1.Y)) + (amount2 * (value3.Y - value1.Y)));
        }

        /// <summary>
        /// Returns a <see cref="Vector2d"/> containing the 2D Cartesian coordinates of a point specified in Barycentric coordinates relative to a 2D triangle.
        /// </summary>
        /// <param name="value1">A <see cref="Vector2d"/> containing the 2D Cartesian coordinates of vertex 1 of the triangle.</param>
        /// <param name="value2">A <see cref="Vector2d"/> containing the 2D Cartesian coordinates of vertex 2 of the triangle.</param>
        /// <param name="value3">A <see cref="Vector2d"/> containing the 2D Cartesian coordinates of vertex 3 of the triangle.</param>
        /// <param name="amount1">Barycentric coordinate b2, which expresses the weighting factor toward vertex 2 (specified in <paramref name="value2"/>).</param>
        /// <param name="amount2">Barycentric coordinate b3, which expresses the weighting factor toward vertex 3 (specified in <paramref name="value3"/>).</param>
        /// <returns>A new <see cref="Vector2d"/> containing the 2D Cartesian coordinates of the specified point.</returns>
        public static Vector2d Barycentric (Vector2d value1, Vector2d value2, Vector2d value3, double amount1, double amount2) {
            Vector2d result;
            Barycentric(ref value1, ref value2, ref value3, amount1, amount2, out result);
            return result;
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <param name="result">When the method completes, contains the clamped value.</param>
        public static void Clamp (ref readonly Vector2d value, ref readonly Vector2d min, ref readonly Vector2d max, out Vector2d result) {
            double x = value.X;
            x = x > max.X ? max.X : x;
            x = x < min.X ? min.X : x;

            double y = value.Y;
            y = y > max.Y ? max.Y : y;
            y = y < min.Y ? min.Y : y;

            result = new Vector2d(x, y);
        }

        /// <summary>
        /// Restricts a value to be within a specified range.
        /// </summary>
        /// <param name="value">The value to clamp.</param>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        /// <returns>The clamped value.</returns>
        public static Vector2d Clamp (Vector2d value, Vector2d min, Vector2d max) {
            Vector2d result;
            Clamp(ref value, ref min, ref max, out result);
            return result;
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <param name="result">When the method completes, contains the distance between the two vectors.</param>
        /// <remarks>
        /// <see cref="DistanceSquared(ref readonly Vector2d, ref readonly Vector2d, out double)"/> may be preferred when only the relative distance is needed
        /// and speed is of the essence.
        /// </remarks>
        public static void Distance (ref readonly Vector2d value1, ref readonly Vector2d value2, out double result) {
            double x = value1.X - value2.X;
            double y = value1.Y - value2.Y;

            result = Math.Sqrt((x * x) + (y * y));
        }

        /// <summary>
        /// Calculates the distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The distance between the two vectors.</returns>
        /// <remarks>
        /// <see cref="DistanceSquared(Vector2d, Vector2d)"/> may be preferred when only the relative distance is needed
        /// and speed is of the essence.
        /// </remarks>
        public static double Distance (Vector2d value1, Vector2d value2) {
            double x = value1.X - value2.X;
            double y = value1.Y - value2.Y;

            return Math.Sqrt((x * x) + (y * y));
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector</param>
        /// <param name="result">When the method completes, contains the squared distance between the two vectors.</param>
        /// <remarks>Distance squared is the value before taking the square root. 
        /// Distance squared can often be used in place of distance if relative comparisons are being made. 
        /// For example, consider three points A, B, and C. To determine whether B or C is further from A, 
        /// compare the distance between A and B to the distance between A and C. Calculating the two distances 
        /// involves two square roots, which are computationally expensive. However, using distance squared 
        /// provides the same information and avoids calculating two square roots.
        /// </remarks>
        public static void DistanceSquared (ref readonly Vector2d value1, ref readonly Vector2d value2, out double result) {
            double x = value1.X - value2.X;
            double y = value1.Y - value2.Y;

            result = (x * x) + (y * y);
        }

        /// <summary>
        /// Calculates the squared distance between two vectors.
        /// </summary>
        /// <param name="value1">The first vector.</param>
        /// <param name="value2">The second vector.</param>
        /// <returns>The squared distance between the two vectors.</returns>
        /// <remarks>Distance squared is the value before taking the square root. 
        /// Distance squared can often be used in place of distance if relative comparisons are being made. 
        /// For example, consider three points A, B, and C. To determine whether B or C is further from A, 
        /// compare the distance between A and B to the distance between A and C. Calculating the two distances 
        /// involves two square roots, which are computationally expensive. However, using distance squared 
        /// provides the same information and avoids calculating two square roots.
        /// </remarks>
        public static double DistanceSquared (Vector2d value1, Vector2d value2) {
            double x = value1.X - value2.X;
            double y = value1.Y - value2.Y;

            return (x * x) + (y * y);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <param name="result">When the method completes, contains the dot product of the two vectors.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Dot (ref readonly Vector2d left, ref readonly Vector2d right, out double result) {
            result = (left.X * right.X) + (left.Y * right.Y);
        }

        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="left">First source vector.</param>
        /// <param name="right">Second source vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Dot (Vector2d left, Vector2d right) {
            return (left.X * right.X) + (left.Y * right.Y);
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <param name="result">When the method completes, contains the normalized vector.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize (ref readonly Vector2d value, out Vector2d result) {
            result = value;
            result.Normalize();
        }

        /// <summary>
        /// Converts the vector into a unit vector.
        /// </summary>
        /// <param name="value">The vector to normalize.</param>
        /// <returns>The normalized vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Normalize (Vector2d value) {
            value.Normalize();
            return value;
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the linear interpolation of the two vectors.</param>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static void Lerp (ref readonly Vector2d start, ref readonly Vector2d end, double amount, out Vector2d result) {
            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
        }

        /// <summary>
        /// Performs a linear interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The linear interpolation of the two vectors.</returns>
        /// <remarks>
        /// This method performs the linear interpolation based on the following formula.
        /// <code>start + (end - start) * amount</code>
        /// Passing <paramref name="amount"/> a value of 0 will cause <paramref name="start"/> to be returned; a value of 1 will cause <paramref name="end"/> to be returned. 
        /// </remarks>
        public static Vector2d Lerp (Vector2d start, Vector2d end, double amount) {
            Vector2d result;
            Lerp(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Performs a cubic interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <param name="result">When the method completes, contains the cubic interpolation of the two vectors.</param>
        public static void SmoothStep (ref readonly Vector2d start, ref readonly Vector2d end, double amount, out Vector2d result) {
            amount = amount > 1.0f ? 1.0f : amount < 0.0f ? 0.0f : amount;
            amount = amount * amount * (3.0f - (2.0f * amount));

            result.X = start.X + ((end.X - start.X) * amount);
            result.Y = start.Y + ((end.Y - start.Y) * amount);
        }

        /// <summary>
        /// Performs a cubic interpolation between two vectors.
        /// </summary>
        /// <param name="start">Start vector.</param>
        /// <param name="end">End vector.</param>
        /// <param name="amount">Value between 0 and 1 indicating the weight of <paramref name="end"/>.</param>
        /// <returns>The cubic interpolation of the two vectors.</returns>
        public static Vector2d SmoothStep (Vector2d start, Vector2d end, double amount) {
            Vector2d result;
            SmoothStep(ref start, ref end, amount, out result);
            return result;
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">First source position vector.</param>
        /// <param name="tangent1">First source tangent vector.</param>
        /// <param name="value2">Second source position vector.</param>
        /// <param name="tangent2">Second source tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">When the method completes, contains the result of the Hermite spline interpolation.</param>
        public static void Hermite (ref readonly Vector2d value1, ref readonly Vector2d tangent1, ref readonly Vector2d value2, ref readonly Vector2d tangent2, double amount, out Vector2d result) {
            double squared = amount * amount;
            double cubed = amount * squared;
            double part1 = (2.0f * cubed) - (3.0f * squared) + 1.0f;
            double part2 = (-2.0f * cubed) + (3.0f * squared);
            double part3 = cubed - (2.0f * squared) + amount;
            double part4 = cubed - squared;

            result.X = (value1.X * part1) + (value2.X * part2) + (tangent1.X * part3) + (tangent2.X * part4);
            result.Y = (value1.Y * part1) + (value2.Y * part2) + (tangent1.Y * part3) + (tangent2.Y * part4);
        }

        /// <summary>
        /// Performs a Hermite spline interpolation.
        /// </summary>
        /// <param name="value1">First source position vector.</param>
        /// <param name="tangent1">First source tangent vector.</param>
        /// <param name="value2">Second source position vector.</param>
        /// <param name="tangent2">Second source tangent vector.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>The result of the Hermite spline interpolation.</returns>
        public static Vector2d Hermite (Vector2d value1, Vector2d tangent1, Vector2d value2, Vector2d tangent2, double amount) {
            Vector2d result;
            Hermite(ref value1, ref tangent1, ref value2, ref tangent2, amount, out result);
            return result;
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <param name="result">When the method completes, contains the result of the Catmull-Rom interpolation.</param>
        public static void CatmullRom (ref readonly Vector2d value1, ref readonly Vector2d value2, ref readonly Vector2d value3, ref readonly Vector2d value4, double amount, out Vector2d result) {
            double squared = amount * amount;
            double cubed = amount * squared;

            result.X = 0.5f * ((2.0f * value2.X) + ((-value1.X + value3.X) * amount) +
            (((2.0f * value1.X) - (5.0f * value2.X) + (4.0f * value3.X) - value4.X) * squared) +
            ((-value1.X + (3.0f * value2.X) - (3.0f * value3.X) + value4.X) * cubed));

            result.Y = 0.5f * ((2.0f * value2.Y) + ((-value1.Y + value3.Y) * amount) +
                (((2.0f * value1.Y) - (5.0f * value2.Y) + (4.0f * value3.Y) - value4.Y) * squared) +
                ((-value1.Y + (3.0f * value2.Y) - (3.0f * value3.Y) + value4.Y) * cubed));
        }

        /// <summary>
        /// Performs a Catmull-Rom interpolation using the specified positions.
        /// </summary>
        /// <param name="value1">The first position in the interpolation.</param>
        /// <param name="value2">The second position in the interpolation.</param>
        /// <param name="value3">The third position in the interpolation.</param>
        /// <param name="value4">The fourth position in the interpolation.</param>
        /// <param name="amount">Weighting factor.</param>
        /// <returns>A vector that is the result of the Catmull-Rom interpolation.</returns>
        public static Vector2d CatmullRom (Vector2d value1, Vector2d value2, Vector2d value3, Vector2d value4, double amount) {
            Vector2d result;
            CatmullRom(ref value1, ref value2, ref value3, ref value4, amount, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <param name="result">When the method completes, contains an new vector composed of the largest components of the source vectors.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Max (ref readonly Vector2d left, ref readonly Vector2d right, out Vector2d result) {
            result.X = left.X > right.X ? left.X : right.X;
            result.Y = left.Y > right.Y ? left.Y : right.Y;
        }

        /// <summary>
        /// Returns a vector containing the largest components of the specified vectors.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>A vector containing the largest components of the source vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Max (Vector2d left, Vector2d right) {
            Vector2d result;
            Max(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <param name="result">When the method completes, contains an new vector composed of the smallest components of the source vectors.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Min (ref readonly Vector2d left, ref readonly Vector2d right, out Vector2d result) {
            result.X = left.X < right.X ? left.X : right.X;
            result.Y = left.Y < right.Y ? left.Y : right.Y;
        }

        /// <summary>
        /// Returns a vector containing the smallest components of the specified vectors.
        /// </summary>
        /// <param name="left">The first source vector.</param>
        /// <param name="right">The second source vector.</param>
        /// <returns>A vector containing the smallest components of the source vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d Min (Vector2d left, Vector2d right) {
            Vector2d result;
            Min(ref left, ref right, out result);
            return result;
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal. 
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">Normal of the surface.</param>
        /// <param name="result">When the method completes, contains the reflected vector.</param>
        /// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine 
        /// whether the original vector was close enough to the surface to hit it.</remarks>
        public static void Reflect (ref readonly Vector2d vector, ref readonly Vector2d normal, out Vector2d result) {
            double dot = (vector.X * normal.X) + (vector.Y * normal.Y);

            result.X = vector.X - (2.0f * dot * normal.X);
            result.Y = vector.Y - (2.0f * dot * normal.Y);
        }

        /// <summary>
        /// Returns the reflection of a vector off a surface that has the specified normal. 
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="normal">Normal of the surface.</param>
        /// <returns>The reflected vector.</returns>
        /// <remarks>Reflect only gives the direction of a reflection off a surface, it does not determine 
        /// whether the original vector was close enough to the surface to hit it.</remarks>
        public static Vector2d Reflect (Vector2d vector, Vector2d normal) {
            Vector2d result;
            Reflect(ref vector, ref normal, out result);
            return result;
        }

        /// <summary>
        /// Orthogonalizes a list of vectors.
        /// </summary>
        /// <param name="destination">The list of orthogonalized vectors.</param>
        /// <param name="source">The list of vectors to orthogonalize.</param>
        /// <remarks>
        /// <para>Orthogonalization is the process of making all vectors orthogonal to each other. This
        /// means that any given vector in the list will be orthogonal to any other given vector in the
        /// list.</para>
        /// <para>Because this method uses the modified Gram-Schmidt process, the resulting vectors
        /// tend to be numerically unstable. The numeric stability decreases according to the vectors
        /// position in the list so that the first vector is the most stable and the last vector is the
        /// least stable.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Orthogonalize (Vector2d[] destination, params Vector2d[] source) {
            //Uses the modified Gram-Schmidt process.
            //q1 = m1
            //q2 = m2 - ((q1 ⋅ m2) / (q1 ⋅ q1)) * q1
            //q3 = m3 - ((q1 ⋅ m3) / (q1 ⋅ q1)) * q1 - ((q2 ⋅ m3) / (q2 ⋅ q2)) * q2
            //q4 = m4 - ((q1 ⋅ m4) / (q1 ⋅ q1)) * q1 - ((q2 ⋅ m4) / (q2 ⋅ q2)) * q2 - ((q3 ⋅ m4) / (q3 ⋅ q3)) * q3
            //q5 = ...

            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i) {
                Vector2d newvector = source[i];

                for (int r = 0; r < i; ++r) {
                    newvector -= Dot(destination[r], newvector) / Dot(destination[r], destination[r]) * destination[r];
                }

                destination[i] = newvector;
            }
        }

        /// <summary>
        /// Orthonormalizes a list of vectors.
        /// </summary>
        /// <param name="destination">The list of orthonormalized vectors.</param>
        /// <param name="source">The list of vectors to orthonormalize.</param>
        /// <remarks>
        /// <para>Orthonormalization is the process of making all vectors orthogonal to each
        /// other and making all vectors of unit length. This means that any given vector will
        /// be orthogonal to any other given vector in the list.</para>
        /// <para>Because this method uses the modified Gram-Schmidt process, the resulting vectors
        /// tend to be numerically unstable. The numeric stability decreases according to the vectors
        /// position in the list so that the first vector is the most stable and the last vector is the
        /// least stable.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Orthonormalize (Vector2d[] destination, params Vector2d[] source) {
            //Uses the modified Gram-Schmidt process.
            //Because we are making unit vectors, we can optimize the math for orthogonalization
            //and simplify the projection operation to remove the division.
            //q1 = m1 / |m1|
            //q2 = (m2 - (q1 ⋅ m2) * q1) / |m2 - (q1 ⋅ m2) * q1|
            //q3 = (m3 - (q1 ⋅ m3) * q1 - (q2 ⋅ m3) * q2) / |m3 - (q1 ⋅ m3) * q1 - (q2 ⋅ m3) * q2|
            //q4 = (m4 - (q1 ⋅ m4) * q1 - (q2 ⋅ m4) * q2 - (q3 ⋅ m4) * q3) / |m4 - (q1 ⋅ m4) * q1 - (q2 ⋅ m4) * q2 - (q3 ⋅ m4) * q3|
            //q5 = ...

            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i) {
                Vector2d newvector = source[i];

                for (int r = 0; r < i; ++r) {
                    newvector -= Dot(destination[r], newvector) * destination[r];
                }

                newvector.Normalize();
                destination[i] = newvector;
            }
        }

        /// <summary>
        /// Transforms a 2D vector by the given <see cref="Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> rotation to apply.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="Vector4d"/>.</param>
        public static void Transform (ref readonly Vector2d vector, ref readonly Quaternion rotation, out Vector2d result) {
            double x = rotation.X + rotation.X;
            double y = rotation.Y + rotation.Y;
            double z = rotation.Z + rotation.Z;
            double wz = rotation.W * z;
            double xx = rotation.X * x;
            double xy = rotation.X * y;
            double yy = rotation.Y * y;
            double zz = rotation.Z * z;

            result = new Vector2d((vector.X * (1.0f - yy - zz)) + (vector.Y * (xy - wz)), (vector.X * (xy + wz)) + (vector.Y * (1.0f - xx - zz)));
        }

        /// <summary>
        /// Transforms a 2D vector by the given <see cref="Quaternion"/> rotation.
        /// </summary>
        /// <param name="vector">The vector to rotate.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> rotation to apply.</param>
        /// <returns>The transformed <see cref="Vector4d"/>.</returns>
        public static Vector2d Transform (Vector2d vector, Quaternion rotation) {
            Vector2d result;
            Transform(ref vector, ref rotation, out result);
            return result;
        }

        /// <summary>
        /// Transforms an array of vectors by the given <see cref="Quaternion"/> rotation.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="rotation">The <see cref="Quaternion"/> rotation to apply.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.
        /// This array may be the same array as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Transform (Vector2d[] source, ref readonly Quaternion rotation, Vector2d[] destination) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            double x = rotation.X + rotation.X;
            double y = rotation.Y + rotation.Y;
            double z = rotation.Z + rotation.Z;
            double wz = rotation.W * z;
            double xx = rotation.X * x;
            double xy = rotation.X * y;
            double yy = rotation.Y * y;
            double zz = rotation.Z * z;

            double num1 = 1.0f - yy - zz;
            double num2 = xy - wz;
            double num3 = xy + wz;
            double num4 = 1.0f - xx - zz;

            for (int i = 0; i < source.Length; ++i) {
                destination[i] = new Vector2d(
                    (source[i].X * num1) + (source[i].Y * num2),
                    (source[i].X * num3) + (source[i].Y * num4));
            }
        }

        /// <summary>
        /// Transforms a 2D vector by the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">When the method completes, contains the transformed <see cref="Vector4d"/>.</param>
        public static void Transform (ref readonly Vector2d vector, ref readonly Matrix transform, out Vector4d result) {
            result = new Vector4d(
                (vector.X * transform.M11) + (vector.Y * transform.M21) + transform.M41,
                (vector.X * transform.M12) + (vector.Y * transform.M22) + transform.M42,
                (vector.X * transform.M13) + (vector.Y * transform.M23) + transform.M43,
                (vector.X * transform.M14) + (vector.Y * transform.M24) + transform.M44);
        }

        /// <summary>
        /// Transforms a 2D vector by the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="vector">The source vector.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <returns>The transformed <see cref="Vector4d"/>.</returns>
        public static Vector4d Transform (Vector2d vector, Matrix transform) {
            Vector4d result;
            Transform(ref vector, ref transform, out result);
            return result;
        }

        /// <summary>
        /// Transforms an array of 2D vectors by the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="source">The array of vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        public static void Transform (Vector2d[] source, ref readonly Matrix transform, Vector4d[] destination) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i) {
                Transform(ref source[i], in transform, out destination[i]);
            }
        }

        /// <summary>
        /// Performs a coordinate transformation using the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate vector to transform.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">When the method completes, contains the transformed coordinates.</param>
        /// <remarks>
        /// A coordinate transform performs the transformation with the assumption that the w component
        /// is one. The four dimensional vector obtained from the transformation operation has each
        /// component in the vector divided by the w component. This forces the wcomponent to be one and
        /// therefore makes the vector homogeneous. The homogeneous vector is often prefered when working
        /// with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static void TransformCoordinate (ref readonly Vector2d coordinate, ref readonly Matrix transform, out Vector2d result) {
            Vector4d vector = new Vector4d();
            vector.X = (coordinate.X * transform.M11) + (coordinate.Y * transform.M21) + transform.M41;
            vector.Y = (coordinate.X * transform.M12) + (coordinate.Y * transform.M22) + transform.M42;
            vector.Z = (coordinate.X * transform.M13) + (coordinate.Y * transform.M23) + transform.M43;
            vector.W = 1f / ((coordinate.X * transform.M14) + (coordinate.Y * transform.M24) + transform.M44);

            result = new Vector2d(vector.X * vector.W, vector.Y * vector.W);
        }

        /// <summary>
        /// Performs a coordinate transformation using the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="coordinate">The coordinate vector to transform.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <returns>The transformed coordinates.</returns>
        /// <remarks>
        /// A coordinate transform performs the transformation with the assumption that the w component
        /// is one. The four dimensional vector obtained from the transformation operation has each
        /// component in the vector divided by the w component. This forces the wcomponent to be one and
        /// therefore makes the vector homogeneous. The homogeneous vector is often prefered when working
        /// with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static Vector2d TransformCoordinate (Vector2d coordinate, Matrix transform) {
            Vector2d result;
            TransformCoordinate(ref coordinate, ref transform, out result);
            return result;
        }

        /// <summary>
        /// Performs a coordinate transformation on an array of vectors using the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="source">The array of coordinate vectors to trasnform.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.
        /// This array may be the same array as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        /// <remarks>
        /// A coordinate transform performs the transformation with the assumption that the w component
        /// is one. The four dimensional vector obtained from the transformation operation has each
        /// component in the vector divided by the w component. This forces the wcomponent to be one and
        /// therefore makes the vector homogeneous. The homogeneous vector is often prefered when working
        /// with coordinates as the w component can safely be ignored.
        /// </remarks>
        public static void TransformCoordinate (Vector2d[] source, ref readonly Matrix transform, Vector2d[] destination) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i) {
                TransformCoordinate(ref source[i], in transform, out destination[i]);
            }
        }

        /// <summary>
        /// Performs a normal transformation using the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">The normal vector to transform.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <param name="result">When the method completes, contains the transformed normal.</param>
        /// <remarks>
        /// A normal transform performs the transformation with the assumption that the w component
        /// is zero. This causes the fourth row and fourth collumn of the matrix to be unused. The
        /// end result is a vector that is not translated, but all other transformation properties
        /// apply. This is often prefered for normal vectors as normals purely represent direction
        /// rather than location because normal vectors should not be translated.
        /// </remarks>
        public static void TransformNormal (ref readonly Vector2d normal, ref readonly Matrix transform, out Vector2d result) {
            result = new Vector2d(
                (normal.X * transform.M11) + (normal.Y * transform.M21),
                (normal.X * transform.M12) + (normal.Y * transform.M22));
        }

        /// <summary>
        /// Performs a normal transformation using the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="normal">The normal vector to transform.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <returns>The transformed normal.</returns>
        /// <remarks>
        /// A normal transform performs the transformation with the assumption that the w component
        /// is zero. This causes the fourth row and fourth collumn of the matrix to be unused. The
        /// end result is a vector that is not translated, but all other transformation properties
        /// apply. This is often prefered for normal vectors as normals purely represent direction
        /// rather than location because normal vectors should not be translated.
        /// </remarks>
        public static Vector2d TransformNormal (Vector2d normal, Matrix transform) {
            Vector2d result;
            TransformNormal(ref normal, ref transform, out result);
            return result;
        }

        /// <summary>
        /// Performs a normal transformation on an array of vectors using the given <see cref="Matrix"/>.
        /// </summary>
        /// <param name="source">The array of normal vectors to transform.</param>
        /// <param name="transform">The transformation <see cref="Matrix"/>.</param>
        /// <param name="destination">The array for which the transformed vectors are stored.
        /// This array may be the same array as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="source"/> or <paramref name="destination"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="destination"/> is shorter in length than <paramref name="source"/>.</exception>
        /// <remarks>
        /// A normal transform performs the transformation with the assumption that the w component
        /// is zero. This causes the fourth row and fourth collumn of the matrix to be unused. The
        /// end result is a vector that is not translated, but all other transformation properties
        /// apply. This is often prefered for normal vectors as normals purely represent direction
        /// rather than location because normal vectors should not be translated.
        /// </remarks>
        public static void TransformNormal (Vector2d[] source, ref readonly Matrix transform, Vector2d[] destination) {
            if (source == null)
                throw new ArgumentNullException("source");
            if (destination == null)
                throw new ArgumentNullException("destination");
            if (destination.Length < source.Length)
                throw new ArgumentOutOfRangeException("destination", "The destination array must be of same length or larger length than the source array.");

            for (int i = 0; i < source.Length; ++i) {
                TransformNormal(ref source[i], in transform, out destination[i]);
            }
        }

        /// <summary>
        /// Adds two vectors.
        /// </summary>
        /// <param name="left">The first vector to add.</param>
        /// <param name="right">The second vector to add.</param>
        /// <returns>The sum of the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator + (Vector2d left, Vector2d right) {
            return new Vector2d(left.X + right.X, left.Y + right.Y);
        }

        /// <summary>
        /// Assert a vector (return it unchanged).
        /// </summary>
        /// <param name="value">The vector to assert (unchange).</param>
        /// <returns>The asserted (unchanged) vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator + (Vector2d value) {
            return value;
        }

        /// <summary>
        /// Subtracts two vectors.
        /// </summary>
        /// <param name="left">The first vector to subtract.</param>
        /// <param name="right">The second vector to subtract.</param>
        /// <returns>The difference of the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator - (Vector2d left, Vector2d right) {
            return new Vector2d(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>
        /// Reverses the direction of a given vector.
        /// </summary>
        /// <param name="value">The vector to negate.</param>
        /// <returns>A vector facing in the opposite direction.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator - (Vector2d value) {
            return new Vector2d(-value.X, -value.Y);
        }

        /// <summary>
        /// Modulates a vector with another by performing component-wise multiplication.
        /// </summary>
        /// <param name="left">The first vector to multiply.</param>
        /// <param name="right">The second vector to multiply.</param>
        /// <returns>The multiplication of the two vectors.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator * (Vector2d left, Vector2d right) {
            return new Vector2d(left.X * right.X, left.Y * right.Y);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator * (double scale, Vector2d value) {
            return new Vector2d(value.X * scale, value.Y * scale);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator * (Vector2d value, double scale) {
            return new Vector2d(value.X * scale, value.Y * scale);
        }

        /// <summary>
        /// Scales a vector by the given value.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="scale">The amount by which to scale the vector.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator / (Vector2d value, double scale) {
            return new Vector2d(value.X / scale, value.Y / scale);
        }

        /// <summary>
        /// Divides a numerator by a vector.
        /// </summary>
        /// <param name="numerator">The numerator.</param>
        /// <param name="value">The value.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator / (double numerator, Vector2d value) {
            return new Vector2d(numerator / value.X, numerator / value.Y);
        }

        /// <summary>
        /// Divides a vector by the given vector, component-wise.
        /// </summary>
        /// <param name="value">The vector to scale.</param>
        /// <param name="by">The by.</param>
        /// <returns>The scaled vector.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2d operator / (Vector2d value, Vector2d by) {
            return new Vector2d(value.X / by.X, value.Y / by.Y);
        }

        /// <summary>
        /// Tests for equality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has the same value as <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator == (Vector2d left, Vector2d right) {
            return left.Equals(right);
        }

        /// <summary>
        /// Tests for inequality between two objects.
        /// </summary>
        /// <param name="left">The first value to compare.</param>
        /// <param name="right">The second value to compare.</param>
        /// <returns><c>true</c> if <paramref name="left"/> has a different value than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator != (Vector2d left, Vector2d right) {
            return !left.Equals(right);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Vector2d"/> to <see cref="Vector3d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector3d (Vector2d value) {
            return new Vector3d(value, 0.0f);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="Vector2d"/> to <see cref="Vector4d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector4d (Vector2d value) {
            return new Vector4d(value, 0.0f, 0.0f);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString () {
            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", X, Y);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public string ToString (string format) {
            if (format == null)
                return ToString();

            return string.Format(CultureInfo.CurrentCulture, "X:{0} Y:{1}", X.ToString(format, CultureInfo.CurrentCulture), Y.ToString(format, CultureInfo.CurrentCulture));
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public string ToString (IFormatProvider formatProvider) {
            return string.Format(formatProvider, "X:{0} Y:{1}", X, Y);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="formatProvider">The format provider.</param>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public string ToString (string format, IFormatProvider formatProvider) {
            if (format == null)
                ToString(formatProvider);

            return string.Format(formatProvider, "X:{0} Y:{1}", X.ToString(format, formatProvider), Y.ToString(format, formatProvider));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode () {
            return X.GetHashCode() + Y.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="Vector2d"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="Vector2d"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Vector2d"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals (Vector2d other) {
            return Math.Abs(other.X - X) < MathUtil.ZeroTolerance &&
                Math.Abs(other.Y - Y) < MathUtil.ZeroTolerance;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="value">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals (object value) {
            if (value == null)
                return false;

            if (value.GetType() != GetType())
                return false;

            return Equals((Vector2d) value);
        }

        /// <summary>
        /// Deconstructs the vector's components into named variables.
        /// </summary>
        /// <param name="x">The X component</param>
        /// <param name="y">The Y component</param>
        public void Deconstruct (out double x, out double y) {
            x = X;
            y = Y;
        }

#if WPFInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="Vector2d"/> to <see cref="System.Windows.Point"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator System.Windows.Point(Vector2d value)
        {
            return new System.Windows.Point(value.X, value.Y);
        }

        /// <summary>
        /// Performs an explicit conversion from <see cref="System.Windows.Point"/> to <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Vector2d(System.Windows.Point value)
        {
            return new Vector2d((double)value.X, (double)value.Y);
        }
#endif

#if XnaInterop
        /// <summary>
        /// Performs an implicit conversion from <see cref="Vector2d"/> to <see cref="Microsoft.Xna.Framework.Vector2d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Microsoft.Xna.Framework.Vector2d(Vector2d value)
        {
            return new Microsoft.Xna.Framework.Vector2d(value.X, value.Y);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Microsoft.Xna.Framework.Vector2d"/> to <see cref="Vector2d"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Vector2d(Microsoft.Xna.Framework.Vector2d value)
        {
            return new Vector2d(value.X, value.Y);
        }
#endif
    }
}