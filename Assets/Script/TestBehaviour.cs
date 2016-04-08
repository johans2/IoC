using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class TestBehaviour : MonoBehaviour {

    [Dependency]
    private ITestClass testClass { get; set; }
    
    
    [Dependency]
    private TestClass2 testClass2 { get; set; }
    
    void Awake() {
        this.Inject();
    }

    void Start() {
        Assert.IsNotNull(testClass);
        Assert.IsNotNull((testClass as TestClass).testClass2);
        Assert.IsNotNull(testClass2);
    }

    void Update() {

    }
}
