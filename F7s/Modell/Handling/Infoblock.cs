using F7s.Modell.Abstract;
using System; using F7s.Utility.Geometry.Double;
using System.Collections.Generic;

namespace F7s.Modell.Handling;

public class Infoblock {

    public readonly IGameValue entity;

    public string Text { get; internal set; }
    public string TooltipText { get; internal set; }

    public override string ToString () {
        return "Infoblock of " + entity.ToString();
    }

    public void AddInformation (List<IGameValue> values, string prefix = null, string tooltip = null) {
        foreach (IGameValue value in values) {
            AddInformation(value, prefix, tooltip);
        }
    }

    public void AddInformation (object value, string prefix = null, string tooltip = null) {
        AddInformation(value.ToString(), prefix, tooltip);
    }

    public void AddInformation (IGameValue value, string prefix = null, string tooltip = null) {
        throw new NotImplementedException();
    }

    public void AddInformation (string information, string tooltip = null) {
        throw new NotImplementedException();
    }
}
