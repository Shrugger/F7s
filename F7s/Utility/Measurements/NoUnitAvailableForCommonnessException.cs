using System; using F7s.Utility.Geometry.Double;

namespace Assets.Utility.Measurements {

    public class NoUnitAvailableForCommonnessException : Exception {

        public NoUnitAvailableForCommonnessException(string message)
            : base(message: message) { }

    }

}