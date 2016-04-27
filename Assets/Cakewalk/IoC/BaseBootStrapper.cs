using UnityEngine;
using CakewalkIoC.Injection;

namespace CakewalkIoC.Core {

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
