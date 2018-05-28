using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using Cakewalk.IoC.Exceptions;
using Cakewalk.IoC.Core;
using Cakewalk.IoC;

public class IntegrationTests {
    
	[UnityTest]
    public IEnumerator CreateDependencyGameObejcts() {
        GameObject bootStrapperPrefab = Resources.Load("ValidTestBootStrapper") as GameObject;
        bootStrapperPrefab.GetComponent<BootStrapper>().autoInstantiate = false;
        
        GameObject bootStrapper = GameObject.Instantiate(bootStrapperPrefab);
        
        GameObject goWithDependencies = Resources.Load("GOWithDependencies") as GameObject;

        GameObject.Instantiate(goWithDependencies);

        GameObject GO1 = GameObject.Find("TestBehaviour1(Clone)");
        GameObject GO2 = GameObject.Find("TestBehaviour2(Clone)");
        GameObject GO3 = GameObject.Find("TestBehaviour3(Clone)");
        
        // Check that the dependencies has been created.
        Assert.IsNotNull(GO1);
        Assert.IsNotNull(GO2);
        Assert.IsNotNull(GO3);

        // Check that the references are correct
        Assert.IsTrue(Object.ReferenceEquals(GO1.GetComponent<TestBehaviour1>().dep3, GO2.GetComponent<TestBehaviour2>().dep3));

        GameObject.DestroyImmediate(bootStrapper);
        GameObject.DestroyImmediate(GO1);
        GameObject.DestroyImmediate(GO2);
        GameObject.DestroyImmediate(GO3);

        yield return null;
	}

    [UnityTest]
    public IEnumerator CircularDependencyCheck() {
        
        Container c = new Container();
        
        Assert.Throws<CircularDependencyException>(() => {
            c.CheckCircularDependencies(typeof(CircularDependencyBehaviour1));
        });
        
        yield return null;
    }

    [UnityTest]
    public IEnumerator AutoInstantiateTRUE() {
        GameObject bootStrapperPrefab = Resources.Load("ValidTestBootStrapper") as GameObject;
        
        BootStrapper bs = bootStrapperPrefab.GetComponent<BootStrapper>();
        bs.autoInstantiate = true;

        GameObject bootStrapper = GameObject.Instantiate(bootStrapperPrefab);
        
        GameObject GO1 = GameObject.Find("TestBehaviour1(Clone)");
        GameObject GO2 = GameObject.Find("TestBehaviour2(Clone)");
        GameObject GO3 = GameObject.Find("TestBehaviour3(Clone)");

        // Check that the dependencies has been created.
        Assert.IsNotNull(GO1);
        Assert.IsNotNull(GO2);
        Assert.IsNotNull(GO3);

        // Check that the references are correct
        Assert.IsTrue(Object.ReferenceEquals(GO1.GetComponent<TestBehaviour1>().dep3, GO2.GetComponent<TestBehaviour2>().dep3));

        GameObject.DestroyImmediate(bootStrapper);
        GameObject.DestroyImmediate(GO1);
        GameObject.DestroyImmediate(GO2);
        GameObject.DestroyImmediate(GO3);

        yield return null;
    }

    [UnityTest]
    public IEnumerator AutoInstantiateFALSE() {
        GameObject bootStrapperPrefab = Resources.Load("ValidTestBootStrapper") as GameObject;

        BootStrapper bs = bootStrapperPrefab.GetComponent<BootStrapper>();
        bs.autoInstantiate = false;

        GameObject bootStrapper = GameObject.Instantiate(bootStrapperPrefab);

        GameObject GO1 = GameObject.Find("TestBehaviour1(Clone)");
        GameObject GO2 = GameObject.Find("TestBehaviour2(Clone)");
        GameObject GO3 = GameObject.Find("TestBehaviour3(Clone)");

        // Check that the dependencies has been created.
        Assert.IsNull(GO1);
        Assert.IsNull(GO2);
        Assert.IsNull(GO3);
        
        GameObject.DestroyImmediate(bootStrapper);

        yield return null;
    }

}
