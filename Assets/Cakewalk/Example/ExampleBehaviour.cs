using UnityEngine;
using UnityEngine.Assertions;
using CakewalkIoC.Injection;

public class ExampleBehaviour : MonoBehaviour {

    [Dependency]
    private IExampleClass exampleClass { get; set; }
    
    
    [Dependency]
    private ExampleClass2 exampleClass2 { get; set; }
    
    void Awake() {
        this.Inject();
    }

    void Start() {
        Assert.IsNotNull(exampleClass);
        Assert.IsNotNull((exampleClass as ExampleClass).testClass2);
        Assert.IsNotNull(exampleClass2);
    }

    void Update() {

    }
}
