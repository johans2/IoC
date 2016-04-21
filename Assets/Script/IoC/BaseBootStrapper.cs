using UnityEngine;
using CakewalkIoC.Injection;

namespace CakewalkIoC.Core {

    public abstract class BaseBootStrapper : MonoBehaviour {

        GameObject Instance;

        void Awake() {
            if(Instance != null) {
                Destroy(gameObject);
            }
            else {
                Instance = gameObject;
                DontDestroyOnLoad(gameObject);
                Container container = new Container();
                IoCExtentions.Container = container;
                Configure(container);
            }
        }


        public abstract void Configure(Container container);

    } 
}
