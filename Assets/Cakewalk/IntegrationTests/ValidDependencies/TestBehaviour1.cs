using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

[System.Serializable]
public class TestBehaviour1 : MonoBehaviour {

    [Dependency] private TestBehaviour3 dep3;

    private float f = 2f;

    public TestBehaviour3 GetDepValue() {
        return dep3;
    }

	void Awake () {
        this.InjectDependencies();
	}

    public float ReturnFloat() {
        return f;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
