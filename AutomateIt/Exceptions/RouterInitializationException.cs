using System;

namespace AutomateIt.Framework.Exceptions {
    public class RouterInitializationException : Exception {
        public RouterInitializationException(Exception cause)
            : base("Error in router initialization", cause) {
        }
    }
}