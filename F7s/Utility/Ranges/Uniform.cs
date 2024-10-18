using System;

namespace F7s.Utility.Ranges
{

    public struct Uniform : Distribution {

        public readonly float min;
        public readonly float max;

        public Uniform (float min, float max) {
            this.min = min;
            this.max = max;
        }
        public Uniform (long min, long max) : this(min, (float) max) {
        }

        public float GetContinuousValue () {
            return Alea.Float(this.min, this.max);
        }

        public long GetDiscreteValue () {
            return Alea.Long((long) this.min, (long) this.max);
        }

    }

    public struct Normal : Distribution {
        private readonly float mean;
        private readonly float standardDeviation;
        public Normal (float mean = 0, float standardDeviation = 1) {
            this.mean = mean;
            this.standardDeviation = standardDeviation;
        }
        public float GetContinuousValue () {
            float randomUniformValue = Alea.Float();
            return MathF.Log(1 / ((this.mean - randomUniformValue) / (MathF.Pow(this.standardDeviation, 3) * MathF.Sqrt(2 * MathF.PI)) * MathF.Exp(-0.5f * MathF.Pow((randomUniformValue - this.mean) / this.standardDeviation, 2))));
        }

        public long GetDiscreteValue () {
            return 0;
        }

    }

    public struct Exponential : Distribution {
        private readonly float decayParameter;
        public Exponential (float decayParameter = 1) {
            if (decayParameter == 0) {
                throw new System.Exception();
            }
            this.decayParameter = decayParameter;
        }
        public float GetContinuousValue () {
            return -MathF.Log(1 - Alea.Float()) / this.decayParameter;
        }
        public long GetDiscreteValue () {
            return Mathematik.RoundToInt(this.GetContinuousValue());
        }
    }

}