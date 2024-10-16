using F7s.Modell.Handling;

namespace F7s.Modell.Abstract {
    public abstract class AbstractGameValue : IGameValue {
        public static implicit operator bool (AbstractGameValue value) {
            return value != null;
        }


        public virtual void ConfigureContextMenu (ContextMenu contextMenu) {
            return;
        }

        public virtual void ConfigureInfoblock (Infoblock infoblock) {
            infoblock.AddInformation("AGV " + this.GetType().Name);
        }


    }
}
