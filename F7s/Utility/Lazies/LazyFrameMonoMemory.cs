using System;
using F7s.Engine;

namespace F7s.Utility.Lazies
{
    public class LazyFrameMonoMemory<inType, outType> : LazyMonoMemory<inType, outType> {
        private int lastFrame = 0;

        private int GetFrame() {
            return Zeit.GetCurrentFrame();
        }
        public LazyFrameMonoMemory(Func<inType, outType> initializer) : base(initializer) {
        }
        public override outType GetValue(inType key) {
            int lastFrame = this.lastFrame;
            int currentFrame = this.GetFrame();

            if (lastFrame < currentFrame) {
                this.MarkAsDirty();
            }

            this.lastFrame = currentFrame;

            return base.GetValue(key);
        }
    }
}
