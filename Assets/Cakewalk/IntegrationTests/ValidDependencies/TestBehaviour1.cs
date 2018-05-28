using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

[System.Serializable]
public class TestBehaviour1 : MonoBehaviour {

    [Dependency] private TestBehaviour3 dep3;

    public TestBehaviour3 GetDepValue() {
        return dep3;
    }

	void Awake () {
        this.InjectDependencies();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
