using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

public class CircularDependencyBehaviour3 : MonoBehaviour {

    [Dependency] CircularDependencyBehaviour1 dep1;
    
	void Awake () {
        this.InjectDependencies();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
