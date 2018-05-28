using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

public class TestBehaviourWithDependencies : MonoBehaviour {

    [Dependency] TestBehaviour1 dep1;
    [Dependency] TestBehaviour2 dep2;

    void Awake () {
        this.InjectDependencies();
	}
	
	void Update () {
		
	}
}
