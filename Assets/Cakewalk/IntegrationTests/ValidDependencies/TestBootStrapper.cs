using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CakewalkIoC.Core;

public class TestBootStrapper : BaseBootStrapper {
    
    public TestBehaviour1 testprefab1;
    public GameObject testprefab2;
    public GameObject testprefab3;
    public MonoBehaviour janne;



    public MonoBehaviour[] systems;

    public override void Configure(Container container) {
        //container.RegisterPrefab<TestBehaviour1>(testprefab1);
        /*
        var prefabNameType = System.Reflection.Assembly.GetAssembly(typeof(TestBootStrapper)).GetType(janne.name);

        var method = container.GetType().GetMethod("RegisterPrefab");
        var generic = method.MakeGenericMethod(prefabNameType);
        generic.Invoke(container, new object[] { janne.gameObject });

        //container.RegisterPrefab <janne.GetType() > (janne);

        container.RegisterPrefab<TestBehaviour2>(testprefab2);
        container.RegisterPrefab<TestBehaviour3>(testprefab3);
        */

        for(int i = 0; i < systems.Length; i++) {
            container.RegisterPrefab(systems[i]);
        }
    }
    /*
    private void OnValidate() {

        Debug.Log("janne type=" + janne.GetType());
        var prefabNameType = System.Reflection.Assembly.GetAssembly(typeof(TestBootStrapper)).GetType(janne.name);
        Debug.Log("name=" + prefabNameType);
        MonoBehaviour mb = (MonoBehaviour)(object)janne;
        var c = mb.GetComponent(prefabNameType);
        Debug.Log("Compontn=" + c);
    }*/
}
