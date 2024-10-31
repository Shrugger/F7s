using Stride.Core.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace F7s.Utility {

    public struct Farbe {

        public Farbe (double r, double g, double b)
            : this(r: (float) r, g: (float) g, b: (float) b, a: 1) { }
        public Farbe (float r, float g, float b)
            : this(r: r, g: g, b: b, a: 1) { }

        public Farbe (double r, double g, double b, double a)
            : this(r: (float) r, g: (float) g, b: (float) b, a: (float) a) { }
        public Farbe (float r, float g, float b, float a) {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public readonly float R;
        public readonly float G;
        public readonly float B;
        public readonly float A;

        public static int SizeOfInBytes () {
            return sizeof(float) * 4;
        }

        public static implicit operator Color (Farbe color) {
            return new Color(
                                         color.R,
                                         color.G,
                                         color.B,
                                         color.A
                                        );
        }

        public static implicit operator Farbe (Color color) {
            return new Farbe(r: color.R, g: color.G, b: color.B, a: color.A);
        }

        public override bool Equals (object obj) {
            return base.Equals(obj: obj);
        }

        public override int GetHashCode () {
            return base.GetHashCode();
        }

        public override string ToString () {
            return "(r"
                 + MM.Round(value: R, decimals: 1)
                 + ", g"
                 + MM.Round(value: G, decimals: 1)
                 + ", b"
                 + MM.Round(value: B, decimals: 1)
                 + ", a"
                 + MM.Round(value: A, decimals: 1)
                 + ")";
        }

        public static float NormalizedDistance (Farbe a, Farbe b) {
            return Distance(a.Normalized(), b.Normalized());
        }

        public static float Distance (Farbe colorA, Farbe colorB) {
            return new Vector3(MathF.Abs(colorA.R - colorB.R), MathF.Abs(colorA.G - colorB.G), MathF.Abs(colorA.B - colorB.B)).Length();
        }

        public Farbe Normalized () {
            return this /= Intensity();
        }

        #region custom colours

        public static Farbe panzerGold => new Farbe(r: 1.0f, g: 0.9f, b: 0.8f);

        public static Farbe shinyGold => new Farbe(r: 1.0f, g: 0.8f, b: 0.6f);

        public static Farbe panzerBlau => new Farbe(r: 0.0f, g: 0.5f, b: 1.0f);

        public static Farbe darkBlue => new Farbe(r: 0.0f, g: 0.1f, b: 0.6f);

        public static Farbe goldenSun => new Farbe(r: 1.0f, g: 0.9f, b: 0.8f);

        public static Farbe plonatSky => new Farbe(r: 0.1f, g: 0.4f, b: 1.0f);

        public static Farbe sandBright => new Farbe(r: 1.0f, g: 0.75f, b: 0.5f);

        public static Farbe wastelandOrange => new Farbe(r: 1.0f, g: 0.25f, b: 0.0f);

        public static Farbe darkOrange => new Farbe(r: 1.0f, g: 0.25f, b: 0.0f);

        public static Farbe darkestOrange => new Farbe(r: 1.0f, g: 0.125f, b: 0.0f);

        public static Farbe darkestGrau => new Farbe(r: 0.125f, g: 0.125f, b: 0.125f);

        public static Farbe darkGrau => new Farbe(r: 0.25f, g: 0.25f, b: 0.25f);

        public static Farbe perfectGrau => new Farbe(r: 0.5f, g: 0.5f, b: 0.5f);

        public static Farbe lightGrau => new Farbe(r: 0.75f, g: 0.75f, b: 0.75f);

        public static Farbe lightestGrau => new Farbe(r: 0.875f, g: 0.875f, b: 0.875f);

        public static Farbe rust => new Farbe(r: 0.75f, g: 0.25f, b: 0.1f);

        public static Farbe agreeableGreen => new Farbe(r: 0.5f, g: 1.00f, b: 0.5f);

        public static Farbe faintGreen => new Farbe(r: 0.75f, g: 1.00f, b: 0.75f);

        public static Farbe goldenAndGreen => new Farbe(r: 0.75f, g: 1.00f, b: 0.5f);

        public static Farbe blueWaters => new Farbe(0.0f, 0.5f, 1.0f);

        public static Farbe black => new Farbe(0, 0, 0, 1);
        public static Farbe white => new Farbe(1, 1, 1, 1);
        public static Farbe magenta => new Farbe(1, 0, 1, 1);

        #endregion

        #region purpose-bound colours

        public static Farbe logDebugging => new Farbe(r: 0.25f, g: 0.25f, b: 0.25f);

        public static Farbe logUndesirable => new Farbe(r: 0.5f, g: 0.25f, b: 0.0f);

        public static Farbe logSuperfluous => new Farbe(r: 0.5f, g: 0.4f, b: 0.0f);

        public static Farbe logInacceptable => new Farbe(r: 0.5f, g: 0.1f, b: 0.0f);

        public static Farbe logFatal => new Farbe(r: 0.5f, g: 0.0f, b: 0.0f);

        public static Farbe logRoutine => new Farbe(r: 0.5f, g: 0.5f, b: 0.5f);

        public static Farbe logSuccess => new Farbe(r: 0.1f, g: 0.5f, b: 0.1f);

        public static Farbe logIncomplete => new Farbe(r: 0.6f, g: 0.4f, b: 0.0f);

        public static Farbe logInterruption => new Farbe(r: 0.5f, g: 0.25f, b: 0.5f);

        public static Farbe orbitPeriapsis // PanzerGold, MOAR COLORFULLSY.
            =>
                new Farbe(r: 1.0f, g: 0.5f, b: 0.0f);

        public static Farbe orbitApoapsis => Farbe.panzerBlau;

        public static Farbe orbitCircular => Farbe.panzerGold;

        public static Farbe BlackbodyRedHot => new Farbe(r: 1, g: 0.0337f, b: 0);

        public static Farbe BlackbodyWhiteHot => new Farbe(r: 1, g: 0.9445f, b: 0.9853f);

        public static Farbe BlackbodyBlueHot => new Farbe(r: 0.3277f, g: 0.5022f, b: 1);

        public static Farbe Transparent => new Farbe(r: 0, g: 0, b: 0, a: 0);

        #endregion

        #region procedural colours

        public static Farbe Clamp (Farbe color, float minBrightness, float maxBrighteness) {
            return new Farbe(
                             r: MM.Clamp(value: color.R, min: minBrightness, max: maxBrighteness),
                             g: MM.Clamp(value: color.G, min: minBrightness, max: maxBrighteness),
                             b: MM.Clamp(value: color.B, min: minBrightness, max: maxBrighteness),
                             a: color.A
                            );
        }

        public static Farbe Average (IEnumerable<Farbe> colours) {
            Farbe average = new Farbe(r: 0, g: 0, b: 0, a: 0);

            foreach (Farbe colour in colours) {
                average += colour;
            }

            average /= colours.Count();

            return average;
        }

        public float Luminance () {
            return (0.2126f * R) + (0.7152f * G) + (0.0722f * B);
        }

        public float Intensity () {
            return AverageValue();
        }

        public static Farbe SetIntensity (Farbe color, float brightness) {
            return color * (brightness / color.AverageValue());
        }

        public Farbe SetRed (float value) {
            return new Farbe(r: value, g: G, b: B, a: A);
        }
        public Farbe SetGreen (float value) {
            return new Farbe(r: R, g: value, b: B, a: A);
        }
        public Farbe SetBlue (float value) {
            return new Farbe(r: R, g: G, b: value, a: A);
        }
        public Farbe SetAlpha (double alpha) {
            return SetAlpha(alpha: (float) alpha);
        }
        public Farbe SetAlpha (float alpha) {
            return Farbe.SetAlpha(colour: this, alpha: alpha);
        }

        public Farbe AddToAlpha (float addition) {
            return SetAlpha(alpha: A + addition);
        }

        public static Farbe SetAlpha (Farbe colour, float alpha) {
            return new Farbe(r: colour.R, g: colour.G, b: colour.B, a: alpha);
        }

        public Farbe Invert () {
            return Farbe.Inverse(colour: this);
        }

        public static Farbe Inverse (Farbe colour) {
            return Farbe.Inverse(colour: colour, opacity: colour.A);
        }

        public static Farbe Inverse (Farbe colour, float opacity) {
            return new Farbe(r: 1f - colour.R, g: 1f - colour.G, b: 1f - colour.B, a: opacity);
        }

        public static Farbe ColorByCoordinates (Vector3 position, Vector3 scale) {
            Vector3 extents = scale / 2;

            return new Farbe(
                             r: (position.X / extents.X) + 0.5f,
                             g: (position.Y / extents.Y) + 0.5f,
                             b: (position.Z / extents.Z) + 0.5f
                            );
        }

        public static Farbe Lerp (Farbe lhs, Farbe rhs, float rightwardInterpolation) {

            return (lhs * (1.0f - rightwardInterpolation)) + (rhs * rightwardInterpolation);
        }

        #endregion

        #region randomised colors

        public static Farbe RandomNormalised (float brightness) {
            Farbe baseColor = new Farbe(
                                        r: Utility.Alea.Float(min: 0, max: 1),
                                        g: Utility.Alea.Float(min: 0, max: 1),
                                        b: Utility.Alea.Float(min: 0, max: 1),
                                        a: 1
                                       );
            baseColor = baseColor * (brightness / baseColor.MaxValue());

            return baseColor;
        }

        public static Farbe Random () {
            return Farbe.Random(minBright: 0f, maxBright: 1f, minOpacity: 1f, maxOpacity: 1f);
        }

        public static Farbe Random (float minBright, float maxBright) {
            return Farbe.Random(minBright: minBright, maxBright: maxBright, minOpacity: 1, maxOpacity: 1);
        }

        public static Farbe Random (float minBright, float maxBright, float minOpacity, float maxOpacity) {

            return new Farbe(
                             r: Utility.Alea.Float(min: minBright, max: maxBright),
                             g: Utility.Alea.Float(min: minBright, max: maxBright),
                             b: Utility.Alea.Float(min: minBright, max: maxBright),
                             a: Utility.Alea.Float(min: minOpacity, max: maxOpacity)
                            );
        }

        public static Farbe Randomise (Farbe basis, float randomisation) {
            return Farbe.Randomise(basis: basis, minBright: 0, maxBright: 1, randomisation: randomisation);
        }

        public static Farbe Randomise (Farbe basis, float minBright, float maxBright, float randomisation) {
            return new Farbe(
                             r: MM.Clamp(
                                            value: basis.R
                                                 + Utility.Alea.Float(min: -randomisation, max: randomisation),
                                            min: minBright,
                                            max: maxBright
                                           ),
                             g: MM.Clamp(
                                            value: basis.G
                                                 + Utility.Alea.Float(min: -randomisation, max: randomisation),
                                            min: minBright,
                                            max: maxBright
                                           ),
                             b: MM.Clamp(
                                            value: basis.B
                                                 + Utility.Alea.Float(min: -randomisation, max: randomisation),
                                            min: minBright,
                                            max: maxBright
                                           ),
                             a: basis.A
                            );
        }

        #endregion


        #region miscellaneous


        public static bool IsVisibile (Farbe? color) {
            return color.HasValue && color.Value.A > 0;
        }

        public static bool IsInvalid (Farbe colour) {
            return colour.R < 0
                || colour.G < 0
                || colour.B < 0
                || float.IsNaN(colour.R)
                || float.IsNaN(colour.G)
                || float.IsNaN(colour.B)
                || colour.R > 1
                || colour.G > 1
                || colour.B > 1;
        }

        public float TotalValue () {
            return R + G + B;
        }

        public float AverageValue () {
            return (R + G + B) / 3f;
        }

        public float MinValue () {
            return MathF.Min(MathF.Min(R, G), B);
        }

        public float MaxValue () {
            return MathF.Max(MathF.Max(R, G), B);
        }

        public Farbe Lerp (Farbe other, float factor) {
            return Lerp(this, other, factor);
        }


        public Color ToStrideColor () {
            return new Color(R, G, B, A);
        }

        #endregion

        public static Farbe operator - (Farbe a, Farbe b) {
            return new Farbe(r: a.R - b.R, g: a.G - b.G, b: a.B - b.B, a: a.A);
        }

        public static bool operator != (Farbe lhs, Farbe rhs) {
            if (lhs.A == rhs.A && lhs.B == rhs.B && lhs.G == rhs.G && lhs.A == rhs.A) {
                return false;
            }

            return true;
        }

        public static Farbe operator * (Farbe color, float brightness) {
            return new Farbe(r: color.R * brightness, g: color.G * brightness, b: color.B * brightness, a: color.A);
        }
        public static Farbe operator * (Farbe color, double brightness) {
            return new Farbe(r: color.R * brightness, g: color.G * brightness, b: color.B * brightness, a: color.A);
        }

        public static Farbe operator * (float brightness, Farbe color) {
            return new Farbe(r: color.R * brightness, g: color.G * brightness, b: color.B * brightness, a: color.A);
        }

        public static Farbe operator / (Farbe color, float brightness) {
            return new Farbe(r: color.R / brightness, g: color.G / brightness, b: color.B / brightness, a: color.A);
        }

        public static Farbe operator + (Farbe a, Farbe b) {
            return new Farbe(r: a.R + b.R, g: a.G + b.G, b: a.B + b.B, a: a.A + b.A);
        }

        public static Farbe operator + (Farbe a, float b) {
            return new Farbe(r: a.R + b, g: a.G + b, b: a.B + b, a: a.A);
        }

        public static bool operator == (Farbe lhs, Farbe rhs) {
            if (lhs.A == rhs.A && lhs.B == rhs.B && lhs.G == rhs.G && lhs.A == rhs.A) {
                return true;
            }

            return false;
        }

    }

}