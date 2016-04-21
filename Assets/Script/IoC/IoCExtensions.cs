using UnityEngine;
using CakewalkIoC.Core;

namespace CakewalkIoC.Injection {

    public static class IoCExtentions {

        public static Container Container { get; set; }

        public static void Inject<T>(this T monoBehaviour) where T : MonoBehaviour {
            Debug.Log("Injected");
            Container.InjectProperties(monoBehaviour);
        }
    } 

}
