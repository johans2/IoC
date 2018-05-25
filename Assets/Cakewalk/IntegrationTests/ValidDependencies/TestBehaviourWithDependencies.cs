using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CakewalkIoC.Injection;

public class TestBehaviourWithDependencies : MonoBehaviour {

    [Dependency] TestBehaviour1 dep1 { get; set; }
    [Dependency] TestBehaviour2 dep2 { get; set; }

    void Awake () {
        this.InjectDependencies();
	}
	
	void Update () {
		
	}
}
