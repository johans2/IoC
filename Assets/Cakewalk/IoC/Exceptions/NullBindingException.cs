using System;

namespace Cakewalk.Exceptions {
    
    /// <summary>
    /// Exception thrown when trying to inject or resolve a type that has not been registered in the container.
    /// </summary>
    public class NullBindingException : Exception {
        public NullBindingException(string message) : base(message) { }
    }
    
}



