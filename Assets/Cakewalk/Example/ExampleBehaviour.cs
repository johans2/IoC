using UnityEngine;
using UnityEngine.Assertions;
using Cakewalk.IoC;

public class ExampleBehaviour : MonoBehaviour {

    [Dependency] IExampleClass exampleClass;
    [Dependency] ExampleClass2 exampleClass2;
    
    void Awake() {
        this.InjectDependencies();
    }

    void Start() {
        Assert.IsNotNull(exampleClass);
        Assert.IsNotNull((exampleClass as ExampleClass).testClass2);
        Assert.IsNotNull(exampleClass2);
    }

    void Update() {
        // Do stuff..
    }
}
