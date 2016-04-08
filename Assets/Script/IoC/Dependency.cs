using System;

[AttributeUsage(AttributeTargets.Property)]
public class Dependency : Attribute {

    public Dependency() { }
    
}
