namespace F7s.Engine.InputHandling {
    public abstract class ButtonInput {

        public enum ButtonStates { Pressed, HeldDown, Released }

        public readonly ButtonStates State;

        protected ButtonInput (ButtonStates state) {
            State = state;
        }

        public abstract bool Matched ();
    }

}
