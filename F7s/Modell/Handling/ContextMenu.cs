using F7s.Modell.Abstract;
using F7s.Resources.Interfaces;
using System;

namespace F7s.Modell.Handling;

public partial class ContextMenu {
    private IGameValue entity;

    public void RegisterAction (Action action, string text, Texture texture, string tooltip = null) {
        throw new NotImplementedException();
    }

    public void RegisterAction (Action action, string text, string tooltip = null) {
        throw new NotImplementedException();
    }

    public void RegisterAction (Action action, Texture texture, string tooltip = null) {
        throw new NotImplementedException();
    }
}
