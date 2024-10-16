using System;
using F7s.Engine;

namespace F7s.Utility.Lazies
{
    public class FrameFaul<T> : Faul<T> {
        private int lastFrame = -1;
        public FrameFaul(Func<T> initializer) : base(initializer) {
        }

        public override string ToString() {
            return base.ToString() + " @F+" + this.lastFrame;
        }

        private int GetFrame() {
            return Zeit.GetCurrentFrame();
        }
        protected override T GetValue() {
            int lastFrame = this.lastFrame;
            int currentFrame = this.GetFrame();

            if (lastFrame < currentFrame) {
                this.MarkAsDirty();
            }

            this.lastFrame = currentFrame;

            return base.GetValue();
        }
    }
}
