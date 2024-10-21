using F7s.Utility.Measurements;
using Stride.Games;
using System; using F7s.Utility.Geometry.Double;

namespace F7s.Engine {

    public static class Zeit {
        private static Func<int> currentFrameGetter;
        private static Func<double> simulationDateGetter;

        public static bool Paused { get; private set; } = false;
        public static string Stempel => Timestamp();

        private static double timePaused = 0;
        private static double realDateOfLastPausing = 0;

        private static GameTime StrideGameTime = new GameTime();

        public static string Timestamp () {
            return "F+" + GetCurrentFrame() + " T+" + Measurement.MeasureTime(GetEngineTimeSeconds()) + ": ";
        }

        public static void SetCurrentFrameGetter (Func<int> frameGetter) {
            currentFrameGetter = frameGetter;
        }

        public static int GetCurrentFrame () {
            if (currentFrameGetter == null) {
                return StrideGameTime.FrameCount;
            } else {
                return currentFrameGetter();
            }
        }

        public static double DeltaTimeSeconds () {
            return StrideGameTime.Elapsed.TotalSeconds;
        }

        public static void SetSimulationDateGetter (Func<double> ticksMsec) {
            simulationDateGetter = ticksMsec;
        }

        public static double GetSimulationDateSeconds () {
            return GetSimulationDateMilliseconds() / 1000.0;
        }

        public static double GetSimulationDateMilliseconds () {
            if (simulationDateGetter == null) {
                return GetEngineTimeMilliseconds();
            } else {
                return simulationDateGetter();
            }
        }

        public static double GetEngineTimeMilliseconds () {
            return StrideGameTime.WarpElapsed.TotalMilliseconds;
        }
        public static double GetEngineTimeSeconds () {
            return StrideGameTime.WarpElapsed.TotalSeconds;
        }

        public static void TogglePause () {
            Paused = !Paused;
            if (Paused) {
                double currentSimulationDate = GetSimulationDateMilliseconds();
                SetSimulationDateGetter(() => currentSimulationDate);
                realDateOfLastPausing = GetEngineTimeMilliseconds();
            } else {
                double currentRealDate = GetEngineTimeMilliseconds();
                timePaused += currentRealDate - realDateOfLastPausing;
                SetSimulationDateGetter(() => GetEngineTimeMilliseconds() - timePaused);
            }
        }

        public static ulong GetTicksMsec () {
            throw new NotImplementedException();
        }
    }
}
