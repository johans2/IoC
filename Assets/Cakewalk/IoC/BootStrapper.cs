using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC.Core;

namespace Cakewalk.IoC {
    
    public class BootStrapper : MonoBehaviour {

        public MonoBehaviour[] systems;
        public bool autoInstantiate = true;

        static BootStrapper Instance;

        void Awake() {
            if(Instance == null) {
                Instance = this;
                Container container = new Container();
                IoCExtentions.Container = container;

                for(int i = 0; i < systems.Length; i++) {
                    container.RegisterPrefab(systems[i]);
                }

                if(autoInstantiate) {
                    container.InstantiateAllRegistrations();
                }
            }
            else {
                DestroyImmediate(gameObject);
            }
        }
        
    }
}
