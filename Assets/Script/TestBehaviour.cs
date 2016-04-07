using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class TestBehaviour : MonoBehaviour {

    [Dependency]
    private ITestClass testClass { get; set; }

    [Dependency]
    private ITestClass2 testClass2 { get; set; }

    public static int startCalls = 0;

    void Awake() {
        this.Inject();
    }

    void Start() {
        startCalls++;
        Assert.IsNotNull(testClass);
        Assert.IsNotNull(testClass2);
    }

    void Update() {

    }
}
