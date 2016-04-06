using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System;

public class Container {

    public Dictionary<Type, Type> Registrations { get; private set; }
    public Dictionary<Type, object> Instances { get; private set; }
    private List<Type> dependencyChain;

    public Container() {
        Registrations = new Dictionary<Type, Type>();
        Instances = new Dictionary<Type, object>();
        dependencyChain = new List<Type>();
    }

    /// <summary>
    /// Register a type and its mapped interface for injection.
    /// </summary>
    /// <typeparam name="TInterface">Mapped interface for registered type.</typeparam>
    /// <typeparam name="TClass">Type to register.</typeparam>
    public void Register<TInterface, TClass>() where TClass : class, TInterface {
        ValidateRegistration<TInterface, TClass>();
        Registrations.Add(typeof(TInterface), typeof(TClass));
    }

    /// <summary>
    /// Register a Type for injection.
    /// </summary>
    /// <typeparam name="TClass">Type to register.</typeparam>
    public void Register<TClass>() where TClass : class {
        if(Registrations.Values.Contains(typeof(TClass))) {
            throw new Exception(typeof(TClass).ToString() + "is already registered and bound to an interface.");
        }

        Registrations.Add(typeof(TClass), typeof(TClass));
    }
    
    /// <summary>
    /// Get the properties marked with [Dependency] attribute and inject them.
    /// </summary>
    /// <param name="obj">Object to inject into.</param>
    public void InjectProperties(object obj) {
        Type type = obj.GetType();

        PropertyInfo[] properties = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.IsDefined(typeof(Dependency), false)).ToArray();
        
        foreach(PropertyInfo property in properties) {
            object instance = GetInstance(property.PropertyType);
            property.SetValue(obj, instance, null);
        }
    }

    /// <summary>
    /// Returns a resolved instance of T type. Either find a previously resolved instance or resolve a new one. 
    /// </summary>
    /// <typeparam name="T">Instance of type of T will be returned.</typeparam>
    public object GetInstance<T>() {
        Type type = typeof(T);
        return GetInstance(type);
    }

    /// <summary>
    /// Returns an instance of the given type. Either find a previously resolved instance or resolve a new one.
    /// </summary>
    /// <param name="type">Instance of type will be returned.</param>
    private object GetInstance(Type type) {
        if(dependencyChain.Contains(type)) {
            throw new CircularDependencyException("Circular dependency in " + type.ToString());
        }
        dependencyChain.Add(type);

        object instance;
        if(!Instances.TryGetValue(type, out instance)) {
            instance = Resolve(type);
            Instances.Add(type, instance);
        }
        return instance;
    }

    /// <summary>
    /// Check that type are valid for injection. 
    /// </summary>
    private void ValidateRegistration<TInterface, TClass>() {
        if(!typeof(TInterface).IsInterface) {
            throw new Exception(typeof(TInterface).ToString() + "is not an interface.");
        }
        if(typeof(TClass).IsAbstract) {
            throw new Exception(typeof(TClass).ToString() + "is abstract and cannot be instantiated.");
        }
    }

    /// <summary>
    /// Get an object of type with all its constructor dependencies resolved. 
    /// </summary>
    /// <param name="type">Type to resolve.</param>
    private object Resolve(Type type) {
        Type implementation;
        if(!Registrations.TryGetValue(type, out implementation)) {
            throw new Exception("Attempt to instanciate a null-binding. " + type.ToString() + " is not registered.");
        }

        ConstructorInfo constructor = GetConstructor(implementation);
        object[] constructorParameters = GetConstructorParameters(constructor);

        if(constructorParameters.Length < 1) {
            dependencyChain.Clear();
        }
        
        return constructor.Invoke(constructorParameters);
    }

    /// <summary>
    /// Get the non-default constructor with the most parameters for an object.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private ConstructorInfo GetConstructor(Type type) {
        ConstructorInfo[] constructors = type.GetConstructors();
        if(constructors.Length < 1) {
            throw new MissingMemberException(type.ToString() + " has no public constructors and cannot be instanciated.");
        }
        // Get constructor with the most parameters
        ConstructorInfo correctConstructor = constructors.Aggregate((c1, c2) => c1.GetParameters().Count() > c2.GetParameters().Count() ? c1 : c2);
        return correctConstructor;
    }

    /// <summary>
    /// Get the resolved constructor-parameters for a given constructor.
    /// </summary>
    /// <param name="constructorInfo">The constructor to get and resolve parameters for.</param>
    /// <returns></returns>
    private object[] GetConstructorParameters(ConstructorInfo constructorInfo) {
        List<object> parameters = new List<object>();

        ParameterInfo[] parameterInfo = constructorInfo.GetParameters();

        foreach(ParameterInfo parameter in parameterInfo) {
            if(parameter.ParameterType.IsValueType) {
                throw new Exception("Parameter " + parameter.ToString() + " in " + constructorInfo.ReflectedType  + " is a valuetype. This is not allowed in injected classes");
            }
            
            Type parameterType = parameter.ParameterType;
            object instance = GetInstance(parameterType);
            parameters.Add(instance);
        }

        return parameters.ToArray();
    }
    
}
