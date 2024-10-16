using F7s.Modell.Abstract;
using System;

namespace F7s.Modell.Economics.Industry {
    public class Work : GameEntity {
        public float Workforce { get; private set; }

        public void AddForce (float workforce) {
            this.Workforce += workforce;
        }

        public float ExtractLabor (float desiredQuantity) {
            float resultingQuantity = Math.Clamp(desiredQuantity, 0, this.Workforce);
            this.Workforce -= resultingQuantity;
            return resultingQuantity;
        }
    }

}
