using System.Collections.Generic;
using UnityEngine;
using CakewalkIoC.Injection;


namespace CakewalkIoC.Core {

    [System.Serializable]
    public struct GameObjectDependency {
        public MonoBehaviour script;
        public GameObject prefab;
    }
    
    public abstract class BaseBootStrapper : MonoBehaviour {

        public GameObjectDependency[] gameObjectDependencies;
        
        BaseBootStrapper Instance;
        
        void Awake() {
            if(Instance == null) {
                Instance = this;
                Container container = new Container();
                IoCExtentions.Container = container;
                container.CreateDependencyGameObjects(gameObjectDependencies);
                Configure(container);
            }
        }


        public virtual void Configure(Container container) { }

    } 
}
