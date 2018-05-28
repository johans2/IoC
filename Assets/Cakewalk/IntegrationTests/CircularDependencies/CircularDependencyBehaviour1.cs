using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

public class CircularDependencyBehaviour1 : MonoBehaviour {

    [Dependency] CircularDependencyBehaviour2 dep2;

	void Awake () {
        this.InjectDependencies();    	
	}
	
	void Update () {
		
	}
}
