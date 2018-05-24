using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseClass {

}

public class Inheritor : BaseClass {

}

public class TestScript : MonoBehaviour {

    


    Inheritor myInher = new Inheritor();


	// Use this for initialization
	void Start () {
        Debug.Log(myInher.GetType().ToString());
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
