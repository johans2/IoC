using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;
using Cakewalk.IoC.Core;

public class TestBootStrapper : BaseBootStrapper {
    
    public MonoBehaviour[] systems;

    public override void Configure(Container container) {
        for(int i = 0; i < systems.Length; i++) {
            container.RegisterPrefab(systems[i]);
        }
    }
    
}
