using F7s.Utility.Geometry;
using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility
{

    /// <summary>
    ///     Provides some random values meant to supplement System.Random and UnityEngine.Random.
    ///     Does not offer seed or other determinism functionality.
    /// </summary>
    public static class Alea {

        private static readonly Random randomizer;

        static Alea () {
            randomizer = new Random();
        }

        public static Random SeededRandomizer (int seed) {
            return new Random(seed);
        }

        #region limited values

        public static int Int (int max) {
            return randomizer.Next(maxValue: max);
        }

        #endregion

        public static Quaternion AQuaternion () {
            return Mathematik.DegreesToQuaternion(Vector3() * 180f);
        }

        public static T Item<T> (List<T> items) {
            if (items == null || items.Count == 0) {
                throw new Exception();
            }

            return items[index: Int(inclusiveMin: 0, exclusiveMax: items.Count)];
        }

        public static T Item<T> (Type enumerator) {
            return (T) Item<T>(Enum.GetValues(enumerator));
        }

        public static object Item<T> (Array items) {
            return items.GetValue(Int(inclusiveMin: 0, exclusiveMax: items.Length));
        }

        public static double Range (double min, double max) {
            return Double(min, max);
        }

        public static T Item<T> (params T[] items) {
            return items[Int(inclusiveMin: 0, exclusiveMax: items.Length)];
        }


        public static T Item<T> () where T : Enum {
            Array values = Enum.GetValues(typeof(T));
            T item = (T) values.GetValue(Int(values.Length));
            return item;
        }

        public static T Item<T> (Dictionary<T, float> weightedItems) {
            float sumTotalWeight = weightedItems.Values.Sum();
            float index = Float(0.0f, sumTotalWeight);

            float currentStepWeight = index;

            foreach (T item in weightedItems.Keys) {
                float weight = weightedItems[item];
                currentStepWeight -= weight;

                if (currentStepWeight <= 0) {
                    return item;
                }
            }
            throw new Exception();
        }
        public static T Item<T> (IEnumerable<T> items, Func<T, float> weighter) {
            float sumTotalWeight = items.Sum(i => weighter.Invoke(i));
            float index = Float(0.0f, sumTotalWeight);

            float currentStepWeight = index;

            foreach (T item in items) {
                float weight = weighter.Invoke(item);
                currentStepWeight -= weight;

                if (currentStepWeight <= 0) {
                    return item;
                }
            }
            throw new Exception();
        }

        public static T Item<T> (HashSet<T> items) {
            return Item(items.ToArray());
        }

        #region probabilities

        public static bool Probability (float probability) {
            return Float() <= probability;
        }
        public static bool Probability (double probability) {
            return Double() <= probability;
        }

        #endregion

        #region values

        public static double Double () {
            return randomizer.NextDouble();
        }

        public static double Double (double range) {
            return Double() * range;
        }
        public static float Float () {
            return randomizer.NextSingle();
        }
        public static float Float (float range) {
            return Float() * range;
        }

        public static bool Bool () {
            return Float() < 0.5f;
        }

        public static float SignFloat => Bool() ? -1 : 1;
        public static double SignDouble => Bool() ? -1 : 1;
        public static int Sign => Bool() ? -1 : 1;

        public static int Int () {
            return randomizer.Next();
        }

        #endregion

        #region ranges

        public static int Long (long inclusiveMin, long exclusiveMax) {
            if (inclusiveMin < int.MinValue || exclusiveMax > int.MaxValue) {
                throw new NotImplementedException(
                                                  message: "Cannot handle longs larger than corresponding ints: "
                                                         + inclusiveMin
                                                         + " / "
                                                         + exclusiveMax
                                                         + " exceeds "
                                                         + int.MinValue
                                                         + " / "
                                                         + int.MaxValue
                                                         + "."
                                                 );
            }

            return Int(inclusiveMin: (int) inclusiveMin, exclusiveMax: (int) exclusiveMax);
        }

        public static int Int (int inclusiveMin, int exclusiveMax) {
            return randomizer.Next(minValue: inclusiveMin, maxValue: exclusiveMax);
        }

        public static float Float (float min, float max) {
            return min + (max - min) * randomizer.NextSingle();
        }

        public static double Double (double min, double max) {
            return min + (max - min) * randomizer.NextDouble();
        }

        #endregion

        #region Vectors


        public static Vector3 Vector3 (float x, float y, float z) {
            return new Vector3(
                                x: Float(min: -x, max: x),
                                y: Float(min: -y, max: y),
                                z: Float(min: -z, max: z)
                               );
        }

        public static Vector3 RotationInDegrees () {
            return Vector3Abs(360);
        }


        public static Vector3 Vector3Abs (float sizeLimit) {
            return Vector3Abs(sizeLimit, sizeLimit, sizeLimit);
        }
        public static Vector3 Vector3Abs (float x, float y, float z) {
            return new Vector3(
                                x: Float(min: 0, max: x),
                                y: Float(min: 0, max: y),
                                z: Float(min: 0, max: z)
                               );
        }

        /// <summary>
        ///     Returns a Vector3 with each component ranging from -1 to 1.
        /// </summary>
        public static Vector3 Vector3 (float scale = 1.0f) {
            return new Vector3(
                               x: Float(min: -1, max: 1),
                               y: Float(min: -1, max: 1),
                               z: Float(min: -1, max: 1)
                              ) * scale;
        }
        public static Vector3 Vector3 (Vector3 limits) {
            return new Vector3(
                               x: Alea.Float(min: -limits.X, max: limits.X),
                               y: Alea.Float(min: -limits.Y, max: limits.Y),
                               z: Alea.Float(min: -limits.Z, max: limits.Z)
                              );
        }


        #endregion

        #region Colors

        public static Farbe Color (float brightness, int seed) {
            Random random = SeededRandomizer(seed);
            Farbe baseFarbe = Color(seed);
            baseFarbe = baseFarbe * (brightness / Math.Max(Math.Max(baseFarbe.R, baseFarbe.G), baseFarbe.B));
            return baseFarbe;
        }

        public static Farbe Color (int seed) {
            Random random = SeededRandomizer(seed);
            Farbe baseFarbe = new Farbe(
                                        r: random.NextSingle(),
                                        g: random.NextSingle(),
                                        b: random.NextSingle(),
                                        a: 1
                                       );
            return baseFarbe;
        }
        public static Farbe Color (float brightness) {
            Farbe baseFarbe = new Farbe(
                                        r: Float(min: 0, max: 1),
                                        g: Float(min: 0, max: 1),
                                        b: Float(min: 0, max: 1),
                                        a: 1
                                       );
            baseFarbe = baseFarbe * (brightness / Math.Max(Math.Max(baseFarbe.R, baseFarbe.G), baseFarbe.B));

            return baseFarbe;
        }

        public static Farbe Color () {
            return Color(minBright: 0f, maxBright: 1f, minOpacity: 1f, maxOpacity: 1f);
        }

        public static Farbe Color (double minBright, double maxBright) {
            return Color(minBright: minBright, maxBright: maxBright, minOpacity: 1, maxOpacity: 1);
        }

        public static Farbe Color (double minBright, double maxBright, double minOpacity, double maxOpacity) {
            return new Farbe(
                             r: (float) Double(min: minBright, max: maxBright),
                             g: (float) Double(min: minBright, max: maxBright),
                             b: (float) Double(min: minBright, max: maxBright),
                             a: (float) Double(min: minOpacity, max: maxOpacity)
                            );
        }

        public static Farbe Color (Farbe basis, float randomisation) {
            return new Farbe(
                             r: Math.Clamp(
                                            value: basis.R + Float(min: -randomisation, max: randomisation),
                                            min: 0f,
                                            max: 1f
                                           ),
                             g: Math.Clamp(
                                            value: basis.G + Float(min: -randomisation, max: randomisation),
                                            min: 0f,
                                            max: 1f
                                           ),
                             b: Math.Clamp(
                                            value: basis.B + Float(min: -randomisation, max: randomisation),
                                            min: 0f,
                                            max: 1f
                                           ),
                             a: basis.A
                            );
        }

        public static double AngleAbsDouble () {
            return Double(0, 360);
        }
        public static double AngleDouble () {
            return Double(-180, 180);
        }
        public static float AngleAbs () {
            return Float(0, 360);
        }
        public static float Angle () {
            return Float(-180, 180);
        }

        public static Vector3 InUnitSphere () {
            return OnUnitSphere() * Float();
        }
        public static Vector3 OnUnitSphere () {
            return Mathematik.Normalize(Alea.Vector3());
        }

        public static PolarCoordinates Coordinates (float radius = 1) {
            return new PolarCoordinates(Alea.Float(-180, 180), Alea.Float(-90, 90), radius);
        }

        public static Vector2 Vector2 (float min, float max) {
            return new Vector2(Alea.Float(min, max), Alea.Float(min, max));
        }

        public static float Maybe (float probability) {
            return Probability(probability) ? 1 : 0;
        }



        #endregion

    }

}