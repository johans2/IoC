using System;

namespace Cakewalk.IoC {

    [AttributeUsage(AttributeTargets.Property)]
    public class Dependency : Attribute {
        public Dependency() { }
    }

}