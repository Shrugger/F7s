using F7s.Modell.Handling.PlayerControllers;

namespace F7s.Engine;
internal static class RepresentationSettings {
    public static float GlobalScaleFactor { get; private set; } = 1.0f;

    public static void SetGlobalScaleFactor (float value) {
        FloatingOrigin.ForceSnap();
        GlobalScaleFactor = value;
    }
}
