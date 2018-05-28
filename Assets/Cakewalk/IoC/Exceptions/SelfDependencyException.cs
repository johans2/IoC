using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cakewalk.IoC.Exceptions {

    /// <summary>
    /// Exception thrown when an object is declaring itself as a dependency.
    /// </summary>
    public class SelfDependencyException : RegistrationException {
        public SelfDependencyException(string message) : base(message) { }
    }
}
