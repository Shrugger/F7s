using System; using F7s.Utility.Geometry.Double;
using F7s.Engine;

namespace F7s.Utility.Time
{

    public class Stoppuhr {
        private double startDate;

        public Stoppuhr () {
            Reset();
        }

        public double TimePassed () {
            return GetTime() - startDate;
        }

        private double GetTime () {
            return Zeit.GetEngineTimeSeconds();
        }

        public void PrintAndReset (string message, double minimumTimeElapsed = 0) {
            Print(message, minimumTimeElapsed);
            Reset();
        }

        public void Print (string message, double minimumTimeElapsed = 0) {
            double timePassed = TimePassed();
            if (minimumTimeElapsed > 0 && timePassed < minimumTimeElapsed) {
                return;
            }
            string measuredTime = Measurements.Measurement.MeasureTime(timePassed, preferredMagnitude: Assets.Utility.Measurements.Magnitude.milli);
            if (measuredTime == null || measuredTime.Length == 0) {
                throw new Exception();
            }
            Console.WriteLine(message + " " + measuredTime);
        }

        public void Reset () {
            startDate = GetTime();
        }
    }
}
