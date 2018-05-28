using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

public class TestBehaviour2 : MonoBehaviour {
    
    [Dependency] public TestBehaviour3 dep3 { get; set; }

	void Awake () {
        this.InjectDependencies();
	}
	
	void Update () {
		
	}
}
