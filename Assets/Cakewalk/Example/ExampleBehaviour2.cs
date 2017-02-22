using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CakewalkIoC.Injection;

public class ExampleBehaviour2 : MonoBehaviour {

    [Dependency]
    public ExampleClass3 classWithDependencyToGameObject { get; set; }

    void Awake() {
        this.Inject();
    }

}
