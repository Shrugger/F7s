using System;
using System.Collections.Generic;

namespace F7s.Modell.Conceptual.Agents.Activities;

public abstract class Activity {

    private Agent agent;

    public abstract Followup RunOneCycle ();

    public void SetAgent (Agent agent) {
        this.agent = agent;
    }

    public enum Followup { Undefined, Repeat, Prioritize, Emergency, Deprioritize, Cancel }
}

public class PrioritizeNeeds : Activity {
    // Take note of personal needs and pick one to prioritize.
    public override Followup RunOneCycle () {
        throw new NotImplementedException();
    }
}

public class ObserveSurroundings : Activity {
    // Take note of any entities in the environment, as well as of the environment itself.
    public override Followup RunOneCycle () {
        throw new NotImplementedException();
    }
}

public class AssessThreats : Activity {
    // Go over observed entities and mark the ones that pose any danger.
    public override Followup RunOneCycle () {
        throw new NotImplementedException();
    }
}

public class Orient : Activity {
    // Produce mental model of geographic location and surroundings.
    public override Followup RunOneCycle () {
        throw new NotImplementedException();
    }
}

public class ActivityLoop {

    public List<Activity> activities = new List<Activity>();

    public void RunSequentially (double timeBudgetOverall, double cyclicRate) {
        throw new NotImplementedException();
    }

    public void RunInParallel (double timeBudget) {
        throw new NotImplementedException();
    }
}