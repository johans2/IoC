using System;

namespace CakewalkIoC.Injection {

    [AttributeUsage(AttributeTargets.Property)]
    public class Dependency : Attribute {
        public Dependency() { }
    }

}