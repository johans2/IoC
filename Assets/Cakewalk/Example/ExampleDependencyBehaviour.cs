using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CakewalkIoC.Injection;
using UnityEngine.Assertions;

public class ExampleDependencyBehaviour : MonoBehaviour {

    [Dependency]
    private IExampleClass exampleClass { get; set; }

    void Awake() {
        this.InjectDependencies();
        Debug.Log("Awake called in dependency behaviour!");
        Assert.IsNotNull(exampleClass); 
    }
    
}
