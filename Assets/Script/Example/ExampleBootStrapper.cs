using UnityEngine;
using System.Collections;
using System;
using CakewalkIoC.Core;

public class ExampleBootStrapper : BaseBootStrapper {

    public override void Configure(Container container) {

        // container.Register<TestClass>();
        // container.Register<TestClass2>();
        container.Register<IExampleClass, ExampleClass>();
        container.Register<ExampleClass2>();

    }
    
}
