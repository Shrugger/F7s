using System;

namespace Assets.Utility.Measurements {

    public class NoUnitAvailableForCommonnessException : Exception {

        public NoUnitAvailableForCommonnessException(string message)
            : base(message: message) { }

    }

}