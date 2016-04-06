using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Reflection;
using System;

[TestFixture]
internal class ContainerTest {


    [TestFixtureSetUp]
    public void Setup() {

    }

    internal interface IMockClass1 {
    }
    internal interface IMockClass2 {
    }
    internal interface IMockClass3 {
    }

    internal class MockClass1 : IMockClass1 {
        public int constructorCalls = 0;
        public IMockClass2 mock2;
        public IMockClass3 mock3;

        public MockClass1(IMockClass2 mock2, IMockClass3 mock3) {
            this.mock2 = mock2;
            this.mock3 = mock3;
            constructorCalls++;
        }
    }

    internal class MockClass2 : IMockClass2 {
        public int constructorCalls = 0;
        public IMockClass3 mock3;

        public MockClass2(IMockClass3 mock3) {
            this.mock3 = mock3;
            constructorCalls++;
        }
    }

    internal class MockClass3 : IMockClass3 {
        public int constructorCalls = 0;
        public MockClass3() {
            constructorCalls++;
        }
    }
    
    [Test]
    public void Register() {
        Container container = new Container();
        container.Register<MockClass1>();
        container.Register<MockClass2>();

        Assert.IsTrue(container.Registrations.ContainsKey(typeof(MockClass1)));
        Assert.IsTrue(container.Registrations.ContainsKey(typeof(MockClass2)));
        Assert.IsTrue(container.Registrations[typeof(MockClass1)] == typeof(MockClass1));
        Assert.IsTrue(container.Registrations[typeof(MockClass2)] == typeof(MockClass2));
    }

    [Test]
    public void RegisterWithInterface() {
        Container container = new Container();
        container.Register<IMockClass1, MockClass1>();
        container.Register<IMockClass2, MockClass2>();

        Assert.IsTrue(container.Registrations.ContainsKey(typeof(IMockClass1)));
        Assert.IsTrue(container.Registrations.ContainsKey(typeof(IMockClass2)));
        Assert.IsTrue(container.Registrations[typeof(IMockClass1)] == typeof(MockClass1));
        Assert.IsTrue(container.Registrations[typeof(IMockClass2)] == typeof(MockClass2));
    }

    [Test]
    public void GetInstance() {
        Container container = new Container();
        container.Register<IMockClass1, MockClass1>();
        container.Register<IMockClass2, MockClass2>();
        container.Register<IMockClass3, MockClass3>();

        MockClass1 mock1 = container.GetInstance<IMockClass1>() as MockClass1;
        MockClass2 mock2 = container.GetInstance<IMockClass2>() as MockClass2;
        MockClass3 mock3 = container.GetInstance<IMockClass3>() as MockClass3;

        Assert.AreEqual(1, mock1.constructorCalls);
        Assert.AreEqual(1, mock2.constructorCalls);
        Assert.AreEqual(1, mock3.constructorCalls);



    }

    internal class MockClass4 {
        public MockClass4(MockClass5 mock5) { }
    }

    internal class MockClass5 {
        public MockClass5(MockClass6 mock6) { }
    }

    internal class MockClass6 {
        public MockClass6(MockClass4 mock4) { }


    }

    [Test]
    [ExpectedException(typeof(CircularDependencyException))]
    public void CircularDependency() {
        Container container = new Container();
        container.Register<MockClass4>();
        container.Register<MockClass5>();
        container.Register<MockClass6>();

        MockClass4 o = container.GetInstance<MockClass4>() as MockClass4;
    }

}
