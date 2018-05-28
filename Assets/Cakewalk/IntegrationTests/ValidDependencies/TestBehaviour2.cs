using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

public class TestBehaviour2 : MonoBehaviour {
    
    [Dependency] public TestBehaviour3 dep3 { get; set; }

    public float prefabValue1 = 4f;
    public int prefabValue2 = 2;

	void Awake () {
        this.InjectDependencies();
	}
	
	void Update () {
		
	}
}
