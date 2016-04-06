using System;

public class CircularDependencyException : Exception {

    public CircularDependencyException(string message) : base(message) {}

}

