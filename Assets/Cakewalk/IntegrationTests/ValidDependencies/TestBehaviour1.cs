using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CakewalkIoC.Injection;

[System.Serializable]
public class TestBehaviour1 : MonoBehaviour {

    [Dependency] public TestBehaviour3 dep3 { get; set; }

	void Awake () {
        this.InjectDependencies();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
