using UnityEngine;
using System.Collections;

public abstract class BaseBootStrapper : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(gameObject);

        Container container = new Container();
        Extensions.Container = container;
        Configure(container);
    }

    public abstract void Configure(Container container);

}
