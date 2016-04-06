using UnityEngine;
using System.Collections;

public class TestClass2 {

    // Use this for initialization
    public string myString2 = "Not constructed.";

    public static int constructorCalls = 0;

    public TestClass2() {
        myString2 = "Constructed.";
        constructorCalls++;
    }
}
