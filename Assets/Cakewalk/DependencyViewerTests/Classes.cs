﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

public abstract class TestClass { }

// Simple dependency tree
public class A : TestClass {
    [Dependency] B b;
}

public class B : TestClass {
    [Dependency] C c;

}

public class C : TestClass {
    [Dependency] D d;
    [Dependency] E e;
}

public class D : TestClass {
    [Dependency] E b;
}

public class E : TestClass {


}

public class F : TestClass {


}