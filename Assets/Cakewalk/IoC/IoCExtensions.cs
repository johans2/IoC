using UnityEngine;
using Cakewalk.IoC.Core;

namespace Cakewalk.IoC {

    public static class IoCExtentions {

        public static Container Container { get; set; }

        public static void Inject<T>(this T monoBehaviour) where T : MonoBehaviour {
            Debug.Log("Injected");
            Container.InjectProperties(monoBehaviour);
        }
    } 

}
