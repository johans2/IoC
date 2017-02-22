using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class ExampleClass2 : IExampleClass2 {
    
    public string myString2 = "Not constructed.";

    public static int constructorCalls = 0;
    
    public ExampleClass2() {
        myString2 = "Constructed.";
        constructorCalls++;
    }
}
