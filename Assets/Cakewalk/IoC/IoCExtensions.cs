﻿using UnityEngine;
using CakewalkIoC.Core;

namespace CakewalkIoC.Injection {

    public static class IoCExtentions {

        public static Container Container { get; set; }

        public static void InjectDependencies<T>(this T monoBehaviour) where T : MonoBehaviour {
            Container.InjectDependencies(monoBehaviour);
        }
    } 

}
