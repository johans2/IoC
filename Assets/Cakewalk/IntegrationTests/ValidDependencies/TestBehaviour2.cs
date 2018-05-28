using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

public class TestBehaviour2 : MonoBehaviour {

    public float prefabValue1 = 4f;
    public int prefabValue2 = 2;
    [Dependency] TestBehaviour3 dep3;

    public TestBehaviour3 GetDepValue() {
        return dep3;
    }

	void Awake () {
        this.InjectDependencies();
	}
	
	void Update () {
		
	}
}
