using F7s.Mains;
using F7s.Utility.Measurements;
using Stride.Games;
using System;
namespace F7s.Engine {

    public static class Zeit {
        private static Func<int> currentFrameGetter;
        private static Func<double> simulationDateGetter;

        public static bool Paused { get; private set; } = false;
        public static string Stempel => Timestamp();

        private static double timePaused = 0;
        private static double realDateOfLastPausing = 0;

        public static string Timestamp () {
            return "F+" + GetCurrentFrame() + " T+" + Measurement.MeasureTime(GetEngineTimeSeconds()) + ": ";
        }

        public static void SetCurrentFrameGetter (Func<int> frameGetter) {
            currentFrameGetter = frameGetter;
        }

        public static int GetCurrentFrame () {
            if (currentFrameGetter == null) {
                return GameTime().FrameCount;
            } else {
                return currentFrameGetter();
            }
        }

        public static GameTime GameTime () {
            return MainSync.Game.UpdateTime;
        }

        public static double DeltaTimeSeconds () {
            return GameTime().Elapsed.TotalSeconds;
        }

        public static void SetSimulationDateGetter (Func<double> seconds) {
            simulationDateGetter = seconds;
        }


        public static double GetSimulationDateSeconds () {
            if (simulationDateGetter == null) {
                return GetEngineTimeSeconds();
            } else {
                return simulationDateGetter();
            }
        }

        public static double GetEngineTimeSeconds () {
            return GameTime().WarpElapsed.TotalSeconds;
        }

        public static void TogglePause () {
            Paused = !Paused;
            if (Paused) {
                double currentSimulationDate = GetSimulationDateSeconds();
                SetSimulationDateGetter(() => currentSimulationDate);
                realDateOfLastPausing = GetEngineTimeSeconds();
            } else {
                double currentRealDate = GetEngineTimeSeconds();
                timePaused += currentRealDate - realDateOfLastPausing;
                SetSimulationDateGetter(() => currentRealDate - timePaused);
            }
        }
    }
}
