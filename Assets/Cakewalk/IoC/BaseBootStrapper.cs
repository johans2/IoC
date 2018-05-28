using System.Collections.Generic;
using UnityEngine;
using Cakewalk.IoC;
using System;
using Cakewalk.IoC.Core;

namespace Cakewalk.IoC {
    
    public abstract class BaseBootStrapper : MonoBehaviour {
        
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
