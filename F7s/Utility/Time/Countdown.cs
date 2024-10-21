using Stride.Core.Mathematics; using F7s.Utility.Geometry.Double; using System; using F7s.Utility.Geometry.Double;

namespace F7s.Utility.Time {
    public class Countdown {
        public readonly float InitialTime;
        public float RemainingTime { get; private set; }
        public Countdown (float initialTime) {
            this.InitialTime = initialTime;
            this.RemainingTime = initialTime;
        }
        public void Update (double deltaTime) {
            this.RemainingTime = MathF.Max(0, RemainingTime - (float) deltaTime);
        }

        public bool IsOver () {
            return this.RemainingTime <= 0;
        }

        internal void Reset () {
            this.RemainingTime = InitialTime;
        }

        public static implicit operator bool (Countdown countdown) {
            return countdown.IsOver();
        }

        public override string ToString () {
            return Measurements.Measurement.MeasureTime(RemainingTime) + " / " + Measurements.Measurement.MeasureTime(InitialTime);
        }
    }
}
