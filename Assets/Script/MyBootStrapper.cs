using UnityEngine;
using System.Collections;
using System;

public class MyBootStrapper : BaseBootStrapper {

    public override void Configure(Container container) {

        // container.Register<TestClass>();
        // container.Register<TestClass2>();
        container.Register<ITestClass, TestClass>();
        container.Register<ITestClass2, TestClass2>();

    }
    
}
