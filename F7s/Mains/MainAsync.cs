using Stride.Engine;
using System.Threading.Tasks;

namespace F7s.Mains {
    public class MainAsync : AsyncScript {

        private static MainAsync instance;

        public MainAsync () {

            if (instance != null) {
                throw new System.Exception();
            } else {
                instance = this;
            }
        }

        public override async Task Execute () {
            while (Game.IsRunning) {
                // Do stuff every new frame
                await Script.NextFrame();
            }
        }
    }
}
