using UnityEngine;
using System.Collections;

public static class Extensions {

    public static Container Container { get; set; }

    public static void Inject<T>(this T monoBehaviour) where T : MonoBehaviour {
        Debug.Log("Injected");
        Container.InjectProperties(monoBehaviour);

    }    
}
