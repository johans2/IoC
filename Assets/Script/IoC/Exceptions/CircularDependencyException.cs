using System;

namespace CakewalkIoC.Exceptions {

    /// <summary>
    /// Excetion thrown when trying to resolve a type that has circular dependencies.
    /// </summary>
    public class CircularDependencyException : Exception {
        public CircularDependencyException(string message) : base(message) { }
    }
     
}

