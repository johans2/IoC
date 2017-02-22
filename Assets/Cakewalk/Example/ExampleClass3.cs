using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class ExampleClass3 {
    
    public ExampleClass3(ExampleDependencyBehaviour dependencyBehaviour) {
        Assert.IsNotNull(dependencyBehaviour);
    }

}
