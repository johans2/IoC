﻿using System;

namespace Cakewalk.IoC.Exceptions {
    
    /// <summary>
    /// Exception thrown when something goes wrong with object registration.
    /// </summary>
    public class RegistrationException : Exception {
        public RegistrationException(string message) : base(message) { }
    }

}

