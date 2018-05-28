using UnityEngine;
using Cakewalk.IoC.Core;

namespace Cakewalk.IoC {

    public static class IoCExtentions {

        public static Container Container { get; set; }

        public static void InjectDependencies<T>(this T monoBehaviour) where T : MonoBehaviour {
            Container.InjectDependencies(monoBehaviour);
        }
    } 

}
