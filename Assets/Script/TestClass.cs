using UnityEngine;
using System.Collections;

public class TestClass : ITestClass {
    
    public string myString = "Not Constructed.";
    public TestClass2 testClass2;

    public TestClass(TestClass2 testClass2) {
        this.testClass2 = testClass2;
        myString = "Constructed";
    }
    
}
