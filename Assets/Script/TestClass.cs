using UnityEngine;
using System.Collections;

public class TestClass {
    /*
    [Inject]
    public ITestClass2 testClass2 { get; set; }
    */
    public string myString = "Not Constructed.";
    public TestClass2 testClass2;

    public TestClass(TestClass2 testClass2) {
        this.testClass2 = testClass2;
        myString = "Constructed";
    }
    
}
