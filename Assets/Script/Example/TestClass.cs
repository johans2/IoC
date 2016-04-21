using UnityEngine;
using System.Collections;

public class ExampleClass : IExampleClass {
    
    public string myString = "Not Constructed.";
    public ExampleClass2 testClass2;

    public ExampleClass(ExampleClass2 testClass2) {
        this.testClass2 = testClass2;
        myString = "Constructed";
    }
    
}
