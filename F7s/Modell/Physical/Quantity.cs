using F7s.Modell.Abstract;
using F7s.Utility;
using F7s.Utility.Measurements;
using System; using F7s.Utility.Geometry.Double; using Stride.Core.Mathematics;

namespace F7s.Modell.Physical {
    public class Quantity : AbstractGameValue {
        public double Mass { get; private set; }
        public double Volume { get; private set; }
        public double Density { get; private set; }
        public int Amount { get; private set; }

        public override string ToString () {
            return
                Measurement.MeasureMass(Mass) +
                ", " + Measurement.MeasureVolume(Volume) +
                ", D " + Mathematik.RoundToFirstInterestingDigit(Density) +
                (Amount > 1 ? ", " + "x " + Amount : "");
        }

        public Quantity (double mass) : this(1, mass, 0, 0) {

        }

        public Quantity (double volume, double density) : this(1, volume * density, volume, density) {

        }

        private Quantity (int amount, double mass, double volume, double density) {

            if (amount < 0 || mass < 0 || volume < 0 || density < 0) {
                throw new Exception(message: "Negative values cannot work. int " + amount + ", mass per unit " + mass + ", volume per unit " + volume + ", density " + density + ".");
            }

            if (double.IsInfinity(mass)) {
                throw new Exception("Mass per unit is infinite.");
            }
            if (double.IsInfinity(amount)) {
                throw new Exception("Amount is infinite.");
            }
            if (double.IsInfinity(volume)) {
                throw new Exception("Volume is infinite.");
            }
            if (double.IsInfinity(density)) {
                throw new Exception("Density is infinite.");
            }

            this.Amount = amount;
            this.Mass = mass;
            this.Volume = volume;
            this.Density = density;

        }

    }
}
