using System.Collections.Generic;
using System.Linq;

namespace F7s.Engine.InputHandling {

    public class InputSet {
        private readonly List<AbstractInputAction> actions;

        public bool Active { get; private set; }

        public override string ToString () {
            return GetType().Name;
        }

        public InputSet (params AbstractInputAction[] actions) {
            this.actions = actions.ToList();
        }
        public void Add (AbstractInputAction action) {
            actions.Add(action);
            if (Active) {
                action.Register();
            }
        }

        public void Activate () {
            Active = true;
            actions.ForEach(a => a.Register());
            OnActivation();
        }

        protected virtual void OnActivation () {

        }

        public void Deactivate () {
            Active = false;
            actions.ForEach(a => a.Deregister());
            OnDeactivation();
        }

        protected virtual void OnDeactivation () {

        }
    }

}
