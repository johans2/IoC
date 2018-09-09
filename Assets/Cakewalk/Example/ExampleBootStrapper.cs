using Cakewalk.IoC.Core;

public class ExampleBootStrapper : BaseBootStrapper {

    public override void Configure(Container container) {

        container.Register<IExampleClass, ExampleClass>();
        container.Register<ExampleClass2>();

    }
    
}
