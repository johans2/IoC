using UnityEngine;
using CakewalkIoC.Injection;

namespace CakewalkIoC.Core {

    public abstract class BaseBootStrapper : MonoBehaviour {

        void Awake() {
            DontDestroyOnLoad(gameObject);

            Container container = new Container();
            IoCExtentions.Container = container;
            Configure(container);
        }

        public abstract void Configure(Container container);

    } 
}
