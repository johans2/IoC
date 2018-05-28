using System;

namespace Cakewalk.IoC {

    [AttributeUsage(AttributeTargets.Field)]
    public class Dependency : Attribute {
        public Dependency() { }
    }

}