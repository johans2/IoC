using NUnit.Framework;
using Cakewalk.IoC.Core;
using Cakewalk.Exceptions;

[TestFixture]
internal class ContainerTest {

    private Container container;

    /// <summary>
    /// Run before every test.
    /// </summary>
    [SetUp]
    public void Setup() {
        container = new Container();
        MockClass1.constructorCalls = 0;
        MockClass2.constructorCalls = 0;
        MockClass3.constructorCalls = 0;
        MockClass4.constructorCalls = 0;
        MockClass5.constructorCalls = 0;
        MockClass6.constructorCalls = 0;
    }

    internal interface IMockClass1 {}
    internal interface IMockClass2 {}
    internal interface IMockClass3 {}

    internal class MockClass1 : IMockClass1 {
        public static int constructorCalls = 0;
        public IMockClass2 mock2;
        public IMockClass3 mock3;

        public MockClass1(IMockClass2 mock2, IMockClass3 mock3) {
            this.mock2 = mock2;
            this.mock3 = mock3;
            constructorCalls++;
        }
    }

    internal class MockClass2 : IMockClass2 {
        public static int constructorCalls = 0;
        public IMockClass3 mock3;

        public MockClass2(IMockClass3 mock3) {
            this.mock3 = mock3;
            constructorCalls++;
        }
    }

    internal class MockClass3 : IMockClass3 {
        public static int constructorCalls = 0;
        public MockClass3() {
            constructorCalls++;
        }
    }
    
    [Test]
    public void Register() {
        container.Register<MockClass1>();
        container.Register<MockClass2>();

        Assert.IsTrue(container.Registrations.ContainsKey(typeof(MockClass1)));
        Assert.IsTrue(container.Registrations.ContainsKey(typeof(MockClass2)));
        Assert.IsTrue(container.Registrations[typeof(MockClass1)] == typeof(MockClass1));
        Assert.IsTrue(container.Registrations[typeof(MockClass2)] == typeof(MockClass2));
    }

    [Test]
    public void RegisterWithInterface() {
        container.Register<IMockClass1, MockClass1>();
        container.Register<IMockClass2, MockClass2>();

        Assert.IsTrue(container.Registrations.ContainsKey(typeof(IMockClass1)));
        Assert.IsTrue(container.Registrations.ContainsKey(typeof(IMockClass2)));
        Assert.IsTrue(container.Registrations[typeof(IMockClass1)] == typeof(MockClass1));
        Assert.IsTrue(container.Registrations[typeof(IMockClass2)] == typeof(MockClass2));
    }

    [Test]
    public void ResolveWithInterface() {
        container.Register<IMockClass1, MockClass1>();
        container.Register<IMockClass2, MockClass2>();
        container.Register<IMockClass3, MockClass3>();

        MockClass1 mock1 = container.Resolve<IMockClass1>() as MockClass1;
        MockClass2 mock2 = container.Resolve<IMockClass2>() as MockClass2;
        MockClass3 mock3 = container.Resolve<IMockClass3>() as MockClass3;

        // Check that contructors have only been called once.
        Assert.AreEqual(1, MockClass1.constructorCalls);
        Assert.AreEqual(1, MockClass2.constructorCalls);
        Assert.AreEqual(1, MockClass3.constructorCalls);

        // Check that references in obejcts has been set with the correct references.
        Assert.IsNotNull(mock1.mock2);
        Assert.IsNotNull(mock1.mock3);
        Assert.IsNotNull(mock2.mock3);
        Assert.AreSame(mock1.mock2, mock2);
        Assert.AreSame(mock1.mock3, mock3);
        Assert.AreSame(mock1.mock3, mock2.mock3);
    }

    internal class MockClass4 {
        public static int constructorCalls = 0;
        public MockClass5 mock5;
        public MockClass6 mock6;

        public MockClass4(MockClass5 mock5, MockClass6 mock6) {
            this.mock5 = mock5;
            this.mock6 = mock6;
            constructorCalls++;
        }
    }

    internal class MockClass5 {
        public static int constructorCalls = 0;
        public MockClass6 mock6;

        public MockClass5(MockClass6 mock6) {
            this.mock6 = mock6;
            constructorCalls++;
        }
    }

    internal class MockClass6 {
        public static int constructorCalls = 0;

        public MockClass6() {
            constructorCalls++;
        }
    }
    
    [Test]
    public void ResolveWithOutInterface() {
        container.Register<MockClass4>();
        container.Register<MockClass5>();
        container.Register<MockClass6>();

        MockClass4 mock4 = container.Resolve<MockClass4>() as MockClass4;
        MockClass5 mock5 = container.Resolve<MockClass5>() as MockClass5;
        MockClass6 mock6 = container.Resolve<MockClass6>() as MockClass6;

        // Check that contructors have only been called once.
        Assert.AreEqual(1, MockClass4.constructorCalls);
        Assert.AreEqual(1, MockClass5.constructorCalls);
        Assert.AreEqual(1, MockClass6.constructorCalls);

        // Check that references in obejcts has been set with the correct references.
        Assert.IsNotNull(mock4.mock5);
        Assert.IsNotNull(mock4.mock6);
        Assert.IsNotNull(mock5.mock6);
        Assert.AreSame(mock4.mock5, mock5);
        Assert.AreSame(mock4.mock6, mock6);
        Assert.AreSame(mock4.mock6, mock5.mock6);
    }



    internal class MockClass7 {
        public MockClass7(MockClass8 mock8) { }
    }

    internal class MockClass8 {
        public MockClass8(MockClass9 mock9) { }
    }

    internal class MockClass9 {
        // MockClass 9 creates a circular dependency to MockClass7
        public MockClass9(MockClass10 mock10, MockClass7 mock7) { }
    }

    internal class MockClass10 {
        public MockClass10() { }
    }

    [Test]
    public void CircularDependency() {
        Assert.Throws<CircularDependencyException>(() => {
            // MockClass7 has a circular dependency.
            container.Register<MockClass7>();
            container.Register<MockClass8>();
            container.Register<MockClass9>();
            container.Register<MockClass10>();

            container.Resolve<MockClass7>();

        });
    }

    [Test]
    public void NullBinding() {
        Assert.Throws<NullBindingException>(() => {
            // Don't register nessessary dependency IMockClass3
            container.Register<IMockClass1, MockClass1>();
            container.Register<IMockClass2, MockClass2>();

            container.Resolve<IMockClass1>();
        });
    }
}
