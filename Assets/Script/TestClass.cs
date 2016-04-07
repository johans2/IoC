using UnityEngine;
using System.Collections;

public class TestClass : ITestClass {
    
    public string myString = "Not Constructed.";
    public ITestClass2 testClass2;

    public TestClass(ITestClass2 testClass2) {
        this.testClass2 = testClass2;
        myString = "Constructed";
    }
    
}
