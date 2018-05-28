using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;

public class SelfReferenceBehaviour : MonoBehaviour {

    [Dependency] SelfReferenceBehaviour depSelf;

    private void Awake() {
        this.InjectDependencies();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
