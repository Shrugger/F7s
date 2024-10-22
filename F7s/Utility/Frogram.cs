using System.Collections.Generic;

namespace F7s.Utility;
public abstract class Frogram {
    private static readonly List<Frogram> frograms = new List<Frogram>();

    protected Frogram () {
        frograms.Add(this);
    }

    public void Quit () {
        frograms.Remove(this);
    }

    public static void UpdateAll () {
        frograms.ForEach(f => f.Update());
    }
    public static void PrePhysicsUpdateAll (Stride.Physics.Simulation sender, float tick) {
        frograms.ForEach(f => f.PrePhysicsUpdate(sender, tick));
    }
    public static void PostPhysicsUpdateAll (Stride.Physics.Simulation sender, float tick) {
        frograms.ForEach(f => f.PostPhysicsUpdate(sender, tick));
    }

    protected virtual void Update () {

    }
    protected virtual void PrePhysicsUpdate (Stride.Physics.Simulation sender, float tick) {

    }
    protected virtual void PostPhysicsUpdate (Stride.Physics.Simulation sender, float tick) {

    }
}
