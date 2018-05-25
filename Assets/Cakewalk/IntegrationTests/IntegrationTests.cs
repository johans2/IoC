using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using CakewalkIoC.Exceptions;
using CakewalkIoC.Core;
using CakewalkIoC.Injection;

public class IntegrationTests {
    
	[UnityTest]
    public IEnumerator CreateDependencyGameObejcts() {
        GameObject bootStrapperPrefab = Resources.Load("ValidTestBootStrapper") as GameObject;

        GameObject bootStrapper = GameObject.Instantiate(bootStrapperPrefab);


        GameObject goWithDependencies = Resources.Load("GOWithDependencies") as GameObject;

        GameObject.Instantiate(goWithDependencies);

        GameObject GO1 = GameObject.Find("Dependency1(Clone)");
        GameObject GO2 = GameObject.Find("Dependency2(Clone)");
        GameObject GO3 = GameObject.Find("Dependency3(Clone)");
        
        // Check that the dependencies has been created.
        Assert.IsNotNull(GO1);
        Assert.IsNotNull(GO2);
        Assert.IsNotNull(GO3);

        // Check that the references are correct
        Assert.IsTrue(Object.ReferenceEquals(GO1.GetComponent<TestBehaviour1>().dep3, GO2.GetComponent<TestBehaviour2>().dep3));

        GameObject.DestroyImmediate(bootStrapper);
        GameObject.DestroyImmediate(GameObject.Find("Dependency1(Clone)"));
        GameObject.DestroyImmediate(GameObject.Find("Dependency2(Clone)"));
        GameObject.DestroyImmediate(GameObject.Find("Dependency3(Clone)"));

        yield return null;
	}

    [UnityTest]
    public IEnumerator CircularDependencyCheck() {
        
        Container c = new Container();
        
        Assert.Throws<CakewalkIoC.Exceptions.CircularDependencyException>(() => {
            c.CheckCircularDependencies(typeof(CircularDependencyBehaviour1));
        });
        
        yield return null;
    }



}
