using Stride.Engine;
using System.Threading.Tasks;

namespace F7s {
    public class MainAsync : AsyncScript {
        // Declared public member fields and properties will show in the game studio
        public bool thisIsAPublicBooleanField;

        public override async Task Execute () {
            while (Game.IsRunning) {
                // Do stuff every new frame
                await Script.NextFrame();
            }
        }
    }
}
