using System.Collections.Generic;
using UnityEngine;
using CakewalkIoC.Injection;
using System;

namespace CakewalkIoC.Core {
    
    [System.Serializable]
    public struct PrefabDependency {
        public UnityEngine.Object script;
        public GameObject prefab;
    }

    public abstract class BaseBootStrapper : MonoBehaviour {

        public PrefabDependency[] derp;

        BaseBootStrapper Instance;
        
        void Awake() {
            if(Instance == null) {
                Instance = this;
                Container container = new Container();
                IoCExtentions.Container = container;
                Configure(container);
            }
        }

        public abstract void Configure(Container container);
    } 
}
