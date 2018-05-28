using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

public class CircularDependencyBehaviour2 : MonoBehaviour {

    [Dependency] CircularDependencyBehaviour3 dep3 { get; set; }

	void Awake () {
        this.InjectDependencies();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
