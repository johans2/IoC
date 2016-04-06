using UnityEngine;
using System.Collections;

public class TestBehaviour : MonoBehaviour {

    [Dependency]
    private TestClass testClass { get; set; }

    [Dependency]
    private TestClass2 testClass2 { get; set; }

    public static int startCalls = 0;

    void Awake() {
        this.Inject();
    }

    void Start() {
        //    this.Inject();
        startCalls++;
        int a = 1;
    }

    void Update() {

    }
}
