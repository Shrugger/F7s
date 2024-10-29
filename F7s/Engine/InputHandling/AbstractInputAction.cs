namespace F7s.Engine.InputHandling {

    public abstract class AbstractInputAction {

        public readonly string name;

        public AbstractInputAction (string name) {
            this.name = name;
        }
        public override string ToString () {
            return name;
        }

        protected void Triggered () {

        }

        public abstract void Register ();
        public abstract void Deregister ();
    }

}
