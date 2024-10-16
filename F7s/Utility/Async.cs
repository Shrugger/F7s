using F7s.Engine;
using System;
using System.Threading.Tasks;

namespace F7s.Utility
{
    public static class Async {

        private static bool GameHasQuit () {
            throw new NotImplementedException();
        }

        public static async Task WaitFrame () {
            await Task.Run(UntilFrame);
        }
        public static async Task WaitFrames (int frames) {
            await Task.Run(() => UntilFrames(frames));
        }

        public static async Task WaitSeconds (float seconds) {
            await Task.Run(() => UntilSeconds(seconds));
        }

        public static void UntilSeconds (float seconds) {
            double date = Zeit.GetSimulationDateSeconds() + seconds;
            while (!GameHasQuit() && Zeit.GetSimulationDateSeconds() < date) {

            }
            return;
        }

        public static void UntilFrame () {
            UntilFrames(1);
        }

        public static void UntilFrames (int frames) {
            int frame = Zeit.GetCurrentFrame() + frames;
            while (!GameHasQuit() && Zeit.GetCurrentFrame() < frame) {

            }
            return;
        }

        public static async void Schedule (float seconds, Action action = null) {
            await Task.Run(() => UntilSeconds(seconds));
            if (action != null) {
                action.Invoke();
            }
        }
    }
}
